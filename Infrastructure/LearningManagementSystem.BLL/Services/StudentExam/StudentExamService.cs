using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Application.Abstractions.Services.Exam;
using LearningManagementSystem.Application.Abstractions.Services.Hubs;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.StudentExam;
using LearningManagementSystem.Application.Abstractions.Services.Transcript;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.StudentExam;

public class StudentExamService(
    IGenericRepository<Domain.Entities.StudentExam> _studentExamRepository,
    ITranscriptService _transcriptService,
    IGenericRepository<Domain.Entities.Exam> _examRepository,
    IGenericRepository<Domain.Entities.Transcript> _transcriptRepository,
    IGenericRepository<Domain.Entities.Group> _groupRepository,
    IGenericRepository<Domain.Entities.Student> _studentRepository,
    IRedisCachingService _redisCachingService,
    IStudentExamHubService _studentExamHubService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IStudentExamService
{
    public async Task<StudentExamResponse> CreateAsync(StudentExamRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.StudentExam>(dto);
        entity.Id = Guid.NewGuid();
        await _studentExamRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        await _studentExamHubService.StudentExamAddedService(dto.StudentId, $"You got {dto.Point} from exam");
        return _mapper.Map<StudentExamResponse>(entity);
    }

    public async Task<StudentExamResponse[]> UpdateRangeAsync(StudentExamRequest[] dtos)
    {
        var entities = new List<Domain.Entities.StudentExam>();
        foreach (var dto in dtos)
        {
            string key = $"member-{dto.Id}";
            var data = _redisCachingService.GetData<StudentExamResponse>(key);
            var entity = await _studentExamRepository.GetAsync(x => x.Id == dto.Id && !x.IsDeleted);
            var exam = await _examRepository.GetAsync(x => !x.IsDeleted && x.Id == entity.ExamId);
            if ((decimal)dto.Point > exam.MaxPoint)
                throw new Exception($"Student Point cannot be greater than {exam.MaxPoint}");
            _mapper.Map(dto, entity);
            _studentExamRepository.Update(entity);
            _unitOfWork.SaveChanges();
            var outDto = _mapper.Map<StudentExamResponse>(entity);
            _redisCachingService.SetData(key, outDto);
            await _studentExamHubService.StudentExamAddedService(dto.StudentId, $"You got {dto.Point} from {exam.Name} {exam.ExamType}");
            var group = await _groupRepository.GetAsync(x => !x.IsDeleted && x.Id == exam.GroupId);
            var transcript = await _transcriptRepository.GetAsync(x =>
                !x.IsDeleted && x.GroupId == group.Id && x.StudentId == dto.StudentId);
            _transcriptRepository.Remove(transcript);
            _unitOfWork.SaveChanges();
            List<float> points = new();
            var exams = await _examRepository.GetAll(x => x.GroupId == group.Id, new()
            {
                AllUsers = true
            }).ToListAsync();
            foreach (var item in exams)
            {
                var studentExam = await _studentExamRepository.GetAsync(
                    x => !x.IsDeleted
                         && x.StudentId == transcript.StudentId
                         && x.ExamId == item.Id);
                points.Add(studentExam.Point);
            }

            await _transcriptService.CreateAsync(new(dto.StudentId, group.Id, points.Sum()));
        }

        return _mapper.Map<StudentExamResponse[]>(entities);
    }

    public async Task<StudentExamResponse> UpdateAsync(Guid id, StudentExamRequest dto)
    {
        var entity = await _studentExamRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("StudentExam not found");
        _mapper.Map(dto, entity);
        _studentExamRepository.Update(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<StudentExamResponse>(entity);
    }

    public async Task<StudentExamResponse> RemoveAsync(Guid id)
    {
        var entity = await _studentExamRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("StudentExam not found");
        _studentExamRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<StudentExamResponse>(entity);
    }

    public async Task<StudentExamResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<StudentExamResponse>(key);
        if (data is not null)
            return data;
        var entity = await _studentExamRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("StudentExam not found");
        var outDto = _mapper.Map<StudentExamResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<StudentExamResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _studentExamRepository.GetAll(x => !x.IsDeleted, filter)
            .Include(x => x.Student)
            .Include(x => x.Exam)
            .ToListAsync();
        IList<StudentExamResponse> data = new List<StudentExamResponse>();
        foreach (var entity in entities)
        {
            var exam = await _examRepository.GetAsync(x => x.Id == entity.ExamId && !x.IsDeleted);
            var student = await _studentRepository.GetAsync(x => x.Id == entity.StudentId && !x.IsDeleted);
            var examResponse = _mapper.Map<ExamResponse>(exam);
            var studentResponse = _mapper.Map<StudentResponse>(student);
            data.Add(new(entity.Id, examResponse, studentResponse, entity.Point));
        }

        return data;
    }
}