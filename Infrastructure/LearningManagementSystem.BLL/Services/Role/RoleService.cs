using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Services.Role;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Role;

public class RoleService(RoleManager<AppRole> _roleManager,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IRoleService
{
    public async Task<RoleResponse> CreateAsync(RoleRequest dto)
    {
        var entity = _mapper.Map<AppRole>(dto);
        await _roleManager.CreateAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<RoleResponse>(entity);
    }

    public async Task<RoleResponse> UpdateAsync(string id, RoleRequest dto)
    {
        var entity = await _roleManager.FindByIdAsync(id.ToString());
        if (entity is null) throw new NotFoundException("Role not found");
        _mapper.Map(dto, entity);
        var result = await _roleManager.UpdateAsync(entity);
        if (!result.Succeeded) throw new Exception("Failed to update role");
        var outDto = _mapper.Map<RoleResponse>(entity);
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
        var entity = await _roleManager.FindByIdAsync(id);
        if (entity is null) throw new NotFoundException("Role not found");
        return _mapper.Map<RoleResponse>(entity);
    }

    public async Task<IList<RoleResponse>> GetAllAsync()
    {
        var entities = await _roleManager.Roles.ToListAsync();
        return _mapper.Map<IList<RoleResponse>>(entities);
    }
    
}