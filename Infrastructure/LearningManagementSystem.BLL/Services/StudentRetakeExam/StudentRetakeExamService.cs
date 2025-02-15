using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.StudentRetakeExam;
using LearningManagementSystem.Application.Abstractions.Services.Transcript;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.StudentRetakeExam;

public class StudentRetakeExamService(
    IGenericRepository<Domain.Entities.StudentRetakeExam> _studentRetakeExamRepository,
    IGenericRepository<Domain.Entities.StudentExam> _studentExamRepository,
    IGenericRepository<Domain.Entities.RetakeExam> _retakeExamRepository,
    IGenericRepository<Domain.Entities.Exam> _examRepository,
    IGenericRepository<Domain.Entities.Group> _groupRepository,
    IRedisCachingService _redisCachingService,
    IGenericRepository<Domain.Entities.Transcript> _transcriptRepository,
    ITranscriptService _transcriptService,
    IGenericRepository<Domain.Entities.Term> _termRepository,
    IGenericRepository<Domain.Entities.Student> _studentRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper) : IStudentRetakeExamService
{
    public async Task<StudentRetakeExamResponse> CreateAsync(StudentRetakeExamDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.StudentRetakeExam>(dto);
        var term = await _termRepository.GetAsync(x => !x.IsDeleted && x.IsActive == true);
        await _studentRetakeExamRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        var retakeExam = await _retakeExamRepository.GetAsync(x => !x.IsDeleted && x.Id == entity.RetakeExamId);
        var studentExam = await _studentExamRepository.GetAsync(x =>
            !x.IsDeleted && x.ExamId == retakeExam.ExamId && x.StudentId == dto.StudentId);
        _studentExamRepository.Remove(studentExam);
        await _studentExamRepository.AddAsync(new()
        {
            StudentId = dto.StudentId,
            ExamId = retakeExam.ExamId,
            Point = dto.NewPoint
        });
        var exam = await _examRepository.GetAsync(x => !x.IsDeleted && x.Id == retakeExam.ExamId);
        var group = await _groupRepository.GetAsync(x => !x.IsDeleted && x.Id == exam.GroupId);
        var transcript =
            await _transcriptRepository.GetAsync(x =>
                !x.IsDeleted && x.GroupId == group.Id && x.StudentId == dto.StudentId);
        _transcriptRepository.Remove(transcript);
        _unitOfWork.SaveChanges();
        var exams = await _examRepository.GetAll(x => x.GroupId == transcript.GroupId && !x.IsDeleted,
            new() { AllUsers = true }).ToListAsync();
        foreach (var item in exams)
        {
            var points = await _studentExamRepository.GetAll(
                    x => !x.IsDeleted
                         && x.StudentId == transcript.StudentId
                         && x.ExamId == exam.Id, new() { AllUsers = true })
                .Select(x => x.Point).ToListAsync();

            await _transcriptService.CreateAsync(new(dto.StudentId, group.Id, points.Sum()));
        }

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<StudentRetakeExamResponse>(entity);
    }


    public async Task<StudentRetakeExamResponse> UpdateAsync(Guid id, StudentRetakeExamDto dto)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<StudentRetakeExamResponse>(key);
        var entity = await _studentRetakeExamRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Student's retake exam not found");
        var retakeExam = await _retakeExamRepository.GetAsync(x => !x.IsDeleted && x.Id == entity.RetakeExamId);
        var exam = await _examRepository.GetAsync(x => !x.IsDeleted && x.Id == retakeExam.ExamId);
        if ((decimal)dto.NewPoint > exam.MaxPoint)
            throw new Exception($"Student Point cannot be greater than {exam.MaxPoint}");
        _mapper.Map(dto, entity);
        _studentRetakeExamRepository.Update(entity);
        _unitOfWork.SaveChanges();
        var studentExamToRemove = await _studentExamRepository.GetAsync(x =>
            !x.IsDeleted && x.ExamId == retakeExam.ExamId && x.StudentId == dto.StudentId);
        if (studentExamToRemove is null)
        {
            throw new NotFoundException("Student's exam result not found");
        }

        _studentExamRepository.Remove(studentExamToRemove);
        _unitOfWork.SaveChanges();
        await _studentExamRepository.AddAsync(new()
        {
            ExamId = studentExamToRemove.ExamId,
            StudentId = studentExamToRemove.StudentId,
            Point = dto.NewPoint
        });
        await _unitOfWork.SaveChangesAsync();

        var group = await _groupRepository.GetAsync(x => !x.IsDeleted && x.Id == exam.GroupId);
        var transcript = await _transcriptRepository.GetAsync(x => !x.IsDeleted && x.GroupId == group.Id&&x.StudentId==dto.StudentId);
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
                     && x.StudentId == dto.StudentId
                     && x.ExamId == item.Id);
            points.Add(studentExam.Point);
        }

        await _transcriptService.CreateAsync(new(dto.StudentId, group.Id, points.Sum()));

        var outDto = _mapper.Map<StudentRetakeExamResponse>(entity);
        _redisCachingService.SetData(key,outDto);
        return outDto;
    }

    public async Task<StudentRetakeExamResponse> RemoveAsync(Guid id)
    {
        var entity = await _studentRetakeExamRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Student's retake exam not found");
        _studentRetakeExamRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<StudentRetakeExamResponse>(entity);
    }

    public async Task<StudentRetakeExamResponse> GetAsync(Guid id)
    {
        string key = $"studentRetakeExam-{id}";
        var data = _redisCachingService.GetData<StudentRetakeExamResponse>(key);
        if (data is not null)
            return data;
        var entity = await _studentRetakeExamRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        var student = await _studentRepository.GetAsync(x => x.Id == entity.StudentId & !x.IsDeleted);
        var retakeExam = await _retakeExamRepository.GetAsync(x => x.Id == entity.RetakeExamId & !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Student's retake exam not found");
        var outDto = _mapper.Map<StudentRetakeExamResponse>(entity) with
        {
            Student = _mapper.Map<StudentResponse>(student),
            RetakeExam = _mapper.Map<RetakeExamResponse>(retakeExam)
        };
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<StudentRetakeExamResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _studentRetakeExamRepository.GetAll(x => !x.IsDeleted, filter)
            .ToListAsync();
        var outDtos = _mapper.Map<IList<StudentRetakeExamResponse>>(entities);
        return outDtos;
    }

    public async Task<List<StudentRetakeExamResponse>> ActiveTermRetakeRequests(RequestFilter? filter)
    {
        var terms = await _termRepository.GetAll(x => !x.IsDeleted && x.IsActive, new()
        {
            AllUsers = true
        }).ToListAsync();
        var term = terms.FirstOrDefault();
        var groups = await _groupRepository.GetAll(x => !x.IsDeleted && x.TermId == term.Id, new()
        {
            AllUsers = true,
        }).ToListAsync();
        List<Domain.Entities.StudentRetakeExam> entities = new List<Domain.Entities.StudentRetakeExam>();
        foreach (var group in groups)
        {
            var exams = await _examRepository.GetAll(x => !x.IsDeleted && x.GroupId == group.Id, new()
            {
                AllUsers = true
            }).ToListAsync();
            foreach (var exam in exams)
            {
                var retakeExams = await _retakeExamRepository.GetAll(x => !x.IsDeleted && x.ExamId == exam.Id, new()
                {
                    AllUsers = true
                }).ToListAsync();
                foreach (var retakeExam in retakeExams)
                {
                    var studentRetakeExams = await _studentRetakeExamRepository.GetAll(
                        x => !x.IsDeleted && x.RetakeExamId == retakeExam.Id,filter).ToListAsync();
                    entities.AddRange(studentRetakeExams);
                }
            }
        }


        return _mapper.Map<List<StudentRetakeExamResponse>>(entities);
    }
}