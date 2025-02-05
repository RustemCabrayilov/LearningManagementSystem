using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.Lesson;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Group;

public class GroupService(
    IGenericRepository<Domain.Entities.Group> _groupRepository,
    IGenericRepository<Domain.Entities.StudentGroup> _studentGroupRepository,
    IGenericRepository<Domain.Entities.Student> _studentRepository,
    IGenericRepository<Domain.Entities.Lesson> _lessonRepository,
    IGenericRepository<Domain.Entities.Term> _termRepository,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IGroupService
{
    public async Task<GroupResponse> CreateAsync(GroupRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Group>(dto);
        var term = await _termRepository.GetAsync(x => !x.IsDeleted && x.IsActive);
        entity.TermId = term.Id;
        await _groupRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<GroupResponse>(entity);
    }

    public async Task<GroupResponse> UpdateAsync(Guid id, GroupRequest dto)
    {
        var entity = await _groupRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Group not found");
        _mapper.Map(dto, entity);
        _groupRepository.Update(entity);
        _unitOfWork.SaveChanges();
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
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<GroupResponse>(key);
        if (data is not null)
            return data;
        var entity = await _groupRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Group not found");
        var lessons = await _lessonRepository.GetAll(x => x.GroupId == id && !x.IsDeleted, null)
            .ToListAsync();
        var students = await _studentRepository.GetAll(
            x => _studentGroupRepository.GetAll(x=>!x.IsDeleted,null).Any(sg => sg.GroupId == id) && !x.IsDeleted,
            null).ToListAsync();
        var outDto = _mapper.Map<GroupResponse>(entity);
        outDto.Lessons.AddRange(_mapper.Map<List<LessonResponse>>(lessons));
        outDto.Students.AddRange(_mapper.Map<List<StudentResponse>>(lessons));
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<GroupResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _groupRepository.GetAll(x => !x.IsDeleted, filter).ToListAsync();
        return _mapper.Map<IList<GroupResponse>>(entities);
    }

    public async Task<GroupResponse> Activate(Guid id)
    {
        var entity = await _groupRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        entity.CanApply=!entity.CanApply;
        _groupRepository.Update(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<GroupResponse>(entity);
    }
}