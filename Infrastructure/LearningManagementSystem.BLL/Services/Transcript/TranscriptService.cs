using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Transcript;
using LearningManagementSystem.Application.Abstractions.Services.Transcript;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Transcript;

public class TranscriptService(
    IGenericRepository<Domain.Entities.Transcript> _transcriptRepository,
    IGenericRepository<Domain.Entities.StudentSubject> _studentSubjectRepository,
    IGenericRepository<Domain.Entities.Group> _groupRepository,
    IGenericRepository<Domain.Entities.StudentExam> _studentExamRepository,
    IGenericRepository<Domain.Entities.Exam> _examRepository,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : ITranscriptService
{
    public async Task<TranscriptResponse> CreateAsync(TranscriptRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Transcript>(dto);
        await _transcriptRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        var group = await _groupRepository.GetAsync(x => !x.IsDeleted && x.Id == entity.GroupId);
        var studentsubject =
            await _studentSubjectRepository.GetAsync(x => !x.IsDeleted && x.StudentId==entity.StudentId);
        if (studentsubject != null)
        {
            _studentSubjectRepository.Remove(studentsubject);
            _unitOfWork.SaveChanges();
        }

        var studentSubjectToCreate = new StudentSubject()
        {
            StudentId = dto.StudentId,
            SubjectId = group.SubjectId,
            SubjectTakingType = SubjectTakingType.Taking
        };
        var studentExams = await _studentExamRepository
            .GetAll(x => x.StudentId == entity.StudentId && !x.IsDeleted, new() { AllUsers = true }).ToListAsync();
        foreach (var studentExam in studentExams)
        {
            var exams = await _examRepository
                .GetAll(x => x.Id == studentExam.ExamId && !x.IsDeleted, new() { AllUsers = true }).ToListAsync();
            if (exams.Any(x => x.ExamType == ExamType.Final))
            {
                studentSubjectToCreate.SubjectTakingType =
                    entity.TotalPoint >= 60 ? SubjectTakingType.Passed : SubjectTakingType.Failed;
                break;
            }
        }

        await _studentSubjectRepository.AddAsync(studentSubjectToCreate);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TranscriptResponse>(entity);
    }

    public async Task<TranscriptResponse> UpdateAsync(Guid id, TranscriptRequest dto)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<TranscriptResponse>(key);
        var entity = await _transcriptRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Transcript not found");
        _mapper.Map(dto, entity);
        _transcriptRepository.Update(entity);
        _unitOfWork.SaveChanges();
        var group = await _groupRepository.GetAsync(x => !x.IsDeleted && x.Id == entity.GroupId);
        var studentsubject =
            await _studentSubjectRepository.GetAsync(x => !x.IsDeleted && x.SubjectId == group.SubjectId);
        if (studentsubject != null)
        {
            _studentSubjectRepository.Remove(studentsubject);
            _unitOfWork.SaveChanges();
        }
        var studentSubject = new StudentSubject()
        {
            Id = id,
            StudentId = dto.StudentId,
            SubjectId = studentsubject.SubjectId,
            SubjectTakingType = SubjectTakingType.Taking,
        };
        var studentExams = await _studentExamRepository
            .GetAll(x => x.StudentId == entity.StudentId && !x.IsDeleted, new() { AllUsers = true }).ToListAsync();
        foreach (var studentExam in studentExams)
        {
            var exams = await _examRepository
                .GetAll(x => x.Id == studentExam.ExamId && !x.IsDeleted, new() { AllUsers = true }).ToListAsync();
            if (exams.Any(x => x.ExamType == ExamType.Final))
            {
                studentSubject.SubjectTakingType =
                    entity.TotalPoint >= 60 ? SubjectTakingType.Passed : SubjectTakingType.Failed;
                break;
            }
        }

        _studentSubjectRepository.Update(studentSubject);
        _unitOfWork.SaveChanges();
        var outDto = _mapper.Map<TranscriptResponse>(entity);
        if(data is not null) _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<TranscriptResponse> RemoveAsync(Guid id)
    {
        var entity = await _transcriptRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Transcript not found");
        _transcriptRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<TranscriptResponse>(entity);
    }

    public async Task<TranscriptResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<TranscriptResponse>(key);
        if (data is not null)
            return data;
        var entity = await _transcriptRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Transcript not found");
        var outDto = _mapper.Map<TranscriptResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<TranscriptResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _transcriptRepository.GetAll(x => !x.IsDeleted, filter).ToListAsync();
        return _mapper.Map<IList<TranscriptResponse>>(entities);
    }
}