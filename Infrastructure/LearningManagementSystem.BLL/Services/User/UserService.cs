using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.User;

public class UserService(
    UserManager<AppUser> _userManager,
    UserManager<AppUser> _roleManager,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IUserService
{
    public async Task<UserResponse> CreateAsync(UserRequest dto)
    {
        var entity = _mapper.Map<AppUser>(dto);
        await _userManager.CreateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<UserResponse>(entity);
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
        var entity = await _userManager.FindByIdAsync(id);
        if (entity is null) throw new NotFoundException("AppUser not found");
        return _mapper.Map<UserResponse>(entity);
    }

    public async Task<IList<UserResponse>> GetAllAsync()
    {
        var entities = await _userManager.Users.ToListAsync();
        return _mapper.Map<IList<UserResponse>>(entities);
    }

    public async Task<UserResponse> AssignRoleAsync(UserRoleDto dto)
    {
        var entity = await _userManager.FindByIdAsync(dto.UserId);
        if(entity is null) throw new NotFoundException("User not found");
        var role=await _roleManager.FindByNameAsync(dto.RoleName);
        if(role is null) throw new NotFoundException("Role not found");
        var result = await _userManager.AddToRoleAsync(entity, dto.RoleName);
        if(!result.Succeeded) throw new Exception("Failed to assign role");
        var outDto = _mapper.Map<UserResponse>(entity);
        var userRoles = await _userManager.GetRolesAsync(entity);
        outDto.Roles.AddRange(userRoles);
        return outDto;
    }
}