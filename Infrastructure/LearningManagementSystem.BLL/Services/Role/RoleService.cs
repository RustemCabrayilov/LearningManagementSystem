using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Role;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Role;

public class RoleService(
    RoleManager<AppRole> _roleManager,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IRoleService
{
    public async Task<RoleResponse> CreateAsync(RoleRequest dto)
    {
        var entity = _mapper.Map<AppRole>(dto);
        entity.Id = Guid.NewGuid().ToString();
        await _roleManager.CreateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<RoleResponse>(entity);
    }

    public async Task<RoleResponse> UpdateAsync(string id, RoleRequest dto)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<RoleResponse>(key);
        var entity = await _roleManager.FindByIdAsync(id.ToString());
        if (entity is null) throw new NotFoundException("Role not found");
        _mapper.Map(dto, entity);
        var result = await _roleManager.UpdateAsync(entity);
        if (!result.Succeeded) throw new Exception("Failed to update role");
        var outDto = _mapper.Map<RoleResponse>(entity);
        if(data is not null) _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<RoleResponse> RemoveAsync(string id)
    {
        var entity = await _roleManager.FindByIdAsync(id.ToString());
        if (entity is null) throw new NotFoundException("Role not found");
        await _roleManager.DeleteAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<RoleResponse>(entity);
    }

    public async Task<RoleResponse> GetAsync(string id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<RoleResponse>(key);
        if (data is not null)
            return data;
        var entity = await _roleManager.FindByIdAsync(id);
        if (entity is null) throw new NotFoundException("Role not found");
        var outDto = _mapper.Map<RoleResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<RoleResponse>> GetAllAsync()
    {
        var entities = await _roleManager.Roles.ToListAsync();
        return _mapper.Map<IList<RoleResponse>>(entities);
    }
}