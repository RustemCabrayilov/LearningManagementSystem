using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Services.ElasticService;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.User;

public class UserService(
    UserManager<AppUser> _userManager,
    RoleManager<AppRole> _roleManager,
    IElasticService _elasticService,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IUserService
{
    public async Task<UserResponse> CreateAsync(UserRequest dto)
    {
        var entity = _mapper.Map<AppUser>(dto);
        entity.Id = Guid.NewGuid().ToString();
        await _userManager.CreateAsync(entity,dto.Password);
        await _unitOfWork.SaveChangesAsync();
        var outDto = _mapper.Map<UserResponse>(entity);
        await _elasticService.AddOrUpdateAsync(outDto);
        return outDto;
    }

    public async Task<UserResponse> UpdateAsync(string id, UserRequest dto)
    {
        var entity = await _userManager.FindByIdAsync(id.ToString());
        if (entity is null) throw new NotFoundException("User not found");
        _mapper.Map(dto, entity);
        await _userManager.RemovePasswordAsync(entity);
        await _userManager.AddPasswordAsync(entity, dto.Password);
        var result = await _userManager.UpdateAsync(entity);
        if (!result.Succeeded) throw new Exception("Failed to update user");
        var outDto = _mapper.Map<UserResponse>(entity);
        var userRoles = await _userManager.GetRolesAsync(entity);
        outDto.Roles.AddRange(userRoles);
        return outDto;
    }

    public async Task<UserResponse> RemoveAsync(string id)
    {
        var entity = await _userManager.FindByIdAsync(id);
        if (entity is null) throw new NotFoundException("AppUser not found");
        await _userManager.DeleteAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserResponse>(entity);
    }

    public async Task<UserResponse> GetAsync(string id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<UserResponse>(key);
        if (data is not null)
            return data;   
        var entity = await _userManager.FindByIdAsync(id);
        if (entity is null) throw new NotFoundException("AppUser not found");
        var outDto = _mapper.Map<UserResponse>(entity);
        var roles = await _userManager.GetRolesAsync(entity);
        outDto.Roles.AddRange(roles);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<UserResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = new List<AppUser>();
        if (filter.AllUsers)
            entities = await _userManager.Users.ToListAsync();
        else
        {
            entities = await _userManager.Users
                .Skip((filter.Page - 1) * filter.Count)
                .Take(filter.Count).ToListAsync();
        }

        var outDtos = _mapper.Map<IList<UserResponse>>(entities);
        foreach (var response in outDtos)
        {
            var user=await _userManager.FindByIdAsync(response.Id.ToString());
            var roles = await _userManager.GetRolesAsync(user);
            response.Roles.AddRange(roles);
        }
        return outDtos;
    }

    public async Task<UserResponse> AssignRoleAsync(UserRoleDto dto)
    {
        var entity = await _userManager.FindByIdAsync(dto.UserId);
        if (entity is null) throw new NotFoundException("User not found");
        var role = await _roleManager.FindByNameAsync(dto.RoleName);
        if (role is null) throw new NotFoundException("Role not found");
        var result = await _userManager.AddToRoleAsync(entity, dto.RoleName);
        if (!result.Succeeded) throw new Exception("Failed to assign role");
        var outDto = _mapper.Map<UserResponse>(entity);
        var userRoles = await _userManager.GetRolesAsync(entity);
        outDto.Roles.AddRange(userRoles);
        return outDto;
    }

    public async Task<UserClaim> GetUserInfosByToken(string token)
    {
        string key = $"member-{token}";
        var data = _redisCachingService.GetData<UserClaim>(key);
        if (data is not null)
            return data;
        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
        {
            throw new Exception("Invalid token");
        }

        var jwtToken = handler.ReadJwtToken(token);

        var claims = jwtToken.Claims.ToList();

        var id = claims.FirstOrDefault(x => x.Type == "sub")?.Value;
        var roles = claims.Where(x => x.Type == "role").Select(x => x.Value).ToList();
        var userClaim = new UserClaim(id, roles);
        _redisCachingService.SetData(key, userClaim);
        return userClaim;
    }
}