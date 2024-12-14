﻿using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Group;

public class GroupService(
    IGenericRepository<Domain.Entities.Group> _groupRepository,
    IGenericRepository<Domain.Entities.ExamGroup> _examGroupRepository,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IGroupService
{
    public async Task<GroupResponse> CreateAsync(GroupRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Group>(dto);
        await _groupRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<GroupResponse>(entity);
    }

    public async Task<GroupResponse> UpdateAsync(Guid id, GroupRequest dto)
    {
        var entity = await _groupRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Group not found");
        await _groupRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<GroupResponse>(entity);
    }

    public async Task<GroupResponse> RemoveAsync(Guid id)
    {
        var entity = await _groupRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Group not found");
        _groupRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<GroupResponse>(entity);
    }

    public async Task<GroupResponse> GetAsync(Guid id)
    {
        var entity = await _groupRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
        if(entity is null) throw new NotFoundException("Group not found");
        return _mapper.Map<GroupResponse>(entity);
    }

    public async Task<IList<GroupResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities =await _groupRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
        return _mapper.Map<IList<GroupResponse>>(entities);
    }

    public async Task<GroupResponse> AssignExamAsync(ExamGroupDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.ExamGroup>(dto);
        await _examGroupRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<GroupResponse>(entity);
    }
}