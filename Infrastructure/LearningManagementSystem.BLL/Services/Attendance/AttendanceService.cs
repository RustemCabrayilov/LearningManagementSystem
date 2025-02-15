using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Attendance;
using LearningManagementSystem.Application.Abstractions.Services.Exam;
using LearningManagementSystem.Application.Abstractions.Services.Lesson;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Attendance;

public class AttendanceService(
    IGenericRepository<Domain.Entities.Attendance> _AttendanceRepository,
    IGenericRepository<Domain.Entities.StudentGroup> _studentGroupRepository,
    IGenericRepository<Domain.Entities.Lesson> _lessonRepository,
    IGenericRepository<Domain.Entities.Group> _groupRepository,
    IGenericRepository<Domain.Entities.Student> _studentRepository,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IAttendanceService
{
    public async Task<AttendanceResponse> CreateAsync(AttendanceRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Attendance>(dto);
        await _AttendanceRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<AttendanceResponse>(entity);
    }

    public async Task<AttendanceResponse[]> UpdateRangeAsync(AttendanceRequest[] dtos)
    {
        var entities = new List<Domain.Entities.Attendance>();
        foreach (var dto in dtos)
        {
            var entity = await _AttendanceRepository.GetAsync(x => x.Id == dto.Id && !x.IsDeleted);
            _mapper.Map(dto, entity);
            _AttendanceRepository.Update(entity);
            _unitOfWork.SaveChanges();
            var lesson = await _lessonRepository.GetAsync(x => x.Id == dto.LessonId && !x.IsDeleted);
            var group = await _groupRepository.GetAsync(x => !x.IsDeleted && x.Id == lesson.GroupId);
            var studentGroup = await _studentGroupRepository.GetAsync(x =>
                x.StudentId == dto.StudentId && x.GroupId == lesson.GroupId && !x.IsDeleted);
            var absences = await _AttendanceRepository.GetAll(
                x => !x.IsDeleted && x.StudentId == studentGroup.StudentId && !x.Absence, new()
                {
                    AllUsers = true
                }).ToListAsync();
            var attendances = await _AttendanceRepository.GetAll(
                x => !x.IsDeleted && x.StudentId == studentGroup.StudentId && x.Absence, new()
                {
                    AllUsers = true
                }).ToListAsync();
            studentGroup.AbsenceCount = attendances.Count();
            studentGroup.AttendanceCount = absences.Count();
            _studentGroupRepository.Update(studentGroup);
            _unitOfWork.SaveChanges();
        }

        return _mapper.Map<AttendanceResponse[]>(entities);
    }

    public async Task<AttendanceResponse> UpdateAsync(Guid id, AttendanceRequest dto)
    {
        var entity = await _AttendanceRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Attendance not found");
        _mapper.Map(dto, entity);
        _AttendanceRepository.Update(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<AttendanceResponse>(entity);
    }

    public async Task<AttendanceResponse> RemoveAsync(Guid id)
    {
        var entity = await _AttendanceRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Attendance not found");
        _AttendanceRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<AttendanceResponse>(entity);
    }

    public async Task<AttendanceResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<AttendanceResponse>(key);
        if (data is not null)
            return data;
        var entity = await _AttendanceRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Attendance not found");
        var outDto = _mapper.Map<AttendanceResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<AttendanceResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _AttendanceRepository.GetAll(x => !x.IsDeleted, filter)
            .Include(x => x.Student)
            .Include(x=>x.Lesson)
            .ToListAsync();
        IList<AttendanceResponse> data = new List<AttendanceResponse>();
        foreach (var entity in entities)
        {
            var lesson = await _lessonRepository.GetAsync(x => x.Id == entity.LessonId && !x.IsDeleted);
            var student = await _studentRepository.GetAsync(x => x.Id == entity.StudentId && !x.IsDeleted);
            var lessonResponse = _mapper.Map<LessonResponse>(lesson);
            var studentResponse = _mapper.Map<StudentResponse>(student);
            data.Add(new(entity.Id, studentResponse, lessonResponse,entity.Absence));
        }
        return data;
    }

    public async Task<IList<AttendanceResponse>> StudentAttendances(StudentAttendanceDto dto)
    {
        var lessons = await _lessonRepository
            .GetAll(x => !x.IsDeleted && x.GroupId == dto.GroupId, new() { AllUsers = true }).ToListAsync();
        List<Domain.Entities.Attendance> entities = new();
        foreach (var lesson in lessons)
        {
            var attendance = await _AttendanceRepository.GetAsync(x =>
                !x.IsDeleted &&
                x.LessonId == lesson.Id && x.StudentId == dto.StudentId);
            attendance.Lesson = lesson;
            entities.Add(attendance);
        }

        return _mapper.Map<IList<AttendanceResponse>>(entities);
    }
}