using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.GroupSchedule;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.GroupSchedule;

public class GroupScheduleService(
    IGenericRepository<Domain.Entities.GroupSchedule> _groupScheduleRepository,
    IGenericRepository<Domain.Entities.Group> _groupRepository,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork):IGroupScheduleService
{
    public async Task<GroupScheduleResponse> CreateAsync(GroupScheduleRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.GroupSchedule>(dto);
        await _groupScheduleRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<GroupScheduleResponse>(entity);
    }

    public async Task<GroupScheduleResponse> UpdateAsync(Guid id, GroupScheduleRequest dto)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<GroupScheduleResponse>(key);
        var entity = await _groupScheduleRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("GroupSchedule not found");
        _mapper.Map(dto, entity);
        _groupScheduleRepository.Update(entity);
        _unitOfWork.SaveChanges();
        var outDto= _mapper.Map<GroupScheduleResponse>(entity);
        if(data is not null) _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<GroupScheduleResponse> RemoveAsync(Guid id)
    {
        var entity = await _groupScheduleRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("GroupSchedule not found");
        _groupScheduleRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<GroupScheduleResponse>(entity);
    }

    public async Task<GroupScheduleResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<GroupScheduleResponse>(key);
        if (data is not null)
            return data;
        var entity = await _groupScheduleRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
        if(entity is null) throw new NotFoundException("GroupSchedule not found");
        entity.Group=await _groupRepository.GetAsync(x => x.Id == entity.GroupId && !x.IsDeleted);
        var outDto = _mapper.Map<GroupScheduleResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<GroupScheduleResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities =await _groupScheduleRepository.GetAll(x=>!x.IsDeleted,filter)
            .ToListAsync();
        return _mapper.Map<IList<GroupScheduleResponse>>(entities);
    }   
}