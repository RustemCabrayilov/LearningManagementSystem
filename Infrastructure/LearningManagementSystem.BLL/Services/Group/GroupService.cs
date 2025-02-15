using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.GroupSchedule;
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
    IGenericRepository<Domain.Entities.Teacher> _teacherRepository,
    IGenericRepository<Domain.Entities.GroupSchedule> _groupScheduleRepository,
    IGenericRepository<Domain.Entities.Subject> _subjectRepository,
    IGenericRepository<Domain.Entities.Major> _majorRepository,
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
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<GroupResponse>(key);
        var entity = await _groupRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Group not found");
        _mapper.Map(dto, entity);
        _groupRepository.Update(entity);
        _unitOfWork.SaveChanges();
        var outDto=_mapper.Map<GroupResponse>(entity);
        if(data is not null) _redisCachingService.SetData(key, outDto);
        return outDto;
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
        var subject=await _subjectRepository.GetAsync(x=>x.Id == entity.SubjectId && !x.IsDeleted);
        var term=await _termRepository.GetAsync(x=>x.Id == entity.TermId && !x.IsDeleted);
        var teacher=await _teacherRepository.GetAsync(x=>x.Id == entity.TeacherId && !x.IsDeleted);
        var major=await _majorRepository.GetAsync(x=>x.Id == entity.MajorId && !x.IsDeleted);
        entity.Subject = subject;
        entity.Term = term;
        entity.Teacher = teacher;
        entity.Major = major;
        if (entity is null) throw new NotFoundException("Group not found");

        var lessons = await _lessonRepository.GetAll(
            x => x.GroupId == id && !x.IsDeleted, 
            new() { AllUsers = true }
        ).ToListAsync();

        // Fetch student group associations first
        var studentGroupIds = await _studentGroupRepository.GetAll(
                x => !x.IsDeleted, 
                new() { AllUsers = true }
            )
            .Where(sg => sg.GroupId == id)
            .Select(sg => sg.StudentId)
            .ToListAsync();

        // Now, fetch students based on the student group ids
        var students = await _studentRepository.GetAll(
            x => studentGroupIds.Contains(x.Id) && !x.IsDeleted, 
            new() { AllUsers = true }
        ).ToListAsync();
        var groupSchedules = await _groupScheduleRepository.GetAll(x => !x.IsDeleted&&x.GroupId==entity.Id, new()
        {
            AllUsers = true
        }).ToListAsync();
        var outDto = _mapper.Map<GroupResponse>(entity);
        outDto.Lessons.AddRange(_mapper.Map<List<LessonResponse>>(lessons));
        outDto.Students.AddRange(_mapper.Map<List<StudentResponse>>(students));
        outDto.GroupSchedules.AddRange(_mapper.Map<List<GroupScheduleResponse>>(groupSchedules));

        // Cache the result
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