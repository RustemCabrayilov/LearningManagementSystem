using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Attendance;
using LearningManagementSystem.Application.Abstractions.Services.Lesson;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Lesson;

public class LessonService(
    IGenericRepository<Domain.Entities.Lesson> _lessonRepository,
    IGenericRepository<Domain.Entities.Attendance> _attendanceRepository,
    IGenericRepository<Domain.Entities.StudentGroup> _studentGroupRepository,
    IGenericRepository<Domain.Entities.Group> _groupRepository,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : ILessonService
{
    public async Task<LessonResponse> CreateAsync(LessonRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Lesson>(dto);
        var group=await _groupRepository.GetAsync(x => x.Id == dto.GroupId&&!x.IsDeleted);
        entity.Name = $"{entity.CreateDate}-{group.Name}";
        await _lessonRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        var studentGroups = await _studentGroupRepository.GetAll(x=>x.GroupId==entity.GroupId&& !x.IsDeleted,null).ToListAsync();
        var attendances = new List<Domain.Entities.Attendance>();
        var outDto=_mapper.Map<LessonResponse>(entity);
        foreach (var studentGroup in  studentGroups) 
        {
           attendances.Add(new()
            {
            StudentId = studentGroup.StudentId,
            LessonId = outDto.Id
            });
        }
       await _attendanceRepository.AddRangeAsync(attendances);
        await _unitOfWork.SaveChangesAsync();
        return outDto;
    }

    public async Task<LessonResponse> UpdateAsync(Guid id, LessonRequest dto)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<LessonResponse>(key);
        var entity = await _lessonRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Lesson not found");
        _mapper.Map(dto, entity);
        _lessonRepository.Update(entity);
        _unitOfWork.SaveChanges();
        var outDto= _mapper.Map<LessonResponse>(entity);
        return outDto;
    }

    public async Task<LessonResponse> RemoveAsync(Guid id)
    {
        var entity = await _lessonRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Lesson not found");
        _lessonRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<LessonResponse>(entity);
    }

    public async Task<LessonResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<LessonResponse>(key);
        if (data is not null)
            return data;
        var entity = await _lessonRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Lesson not found");
        var group=await _groupRepository.GetAsync(x => x.Id == entity.GroupId && !x.IsDeleted);
        entity.Group = group;
        var attendances = await _attendanceRepository.GetAsync(x => x.LessonId == id);
        var outDto = _mapper.Map<LessonResponse>(entity);
        outDto.Attendances.AddRange(_mapper.Map<IList<AttendanceResponse>>(attendances));
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<LessonResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _lessonRepository.GetAll(x => !x.IsDeleted, filter).ToListAsync();
        return _mapper.Map<IList<LessonResponse>>(entities);
    }
}