using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Hubs;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.StudentExam;
using LearningManagementSystem.Application.Abstractions.Services.Transcript;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.StudentExam;

public class StudentExamService(
    IGenericRepository<Domain.Entities.StudentExam> _studentExamRepository,
    ITranscriptService _transcriptService,
    IGenericRepository<Domain.Entities.Exam> _examRepository,
    IGenericRepository<Domain.Entities.Transcript> _transcriptRepository,
    IGenericRepository<Domain.Entities.Group> _groupRepository,
    IRedisCachingService _redisCachingService,
    IStudentExamHubService _studentExamHubService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IStudentExamService
{
    public async Task<StudentExamResponse> CreateAsync(StudentExamRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.StudentExam>(dto);
        await _studentExamRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        var exam=await _examRepository.GetAsync(x=>x.Id == dto.ExamId&&!x.IsDeleted);
      await  _studentExamHubService.StudentExamAddedService(dto.StudentId,$"You got {dto.Point} from exam");
      var group = await _groupRepository.GetAsync(x => !x.IsDeleted && x.Id == exam.GroupId);
      var transcript = await _transcriptRepository.GetAsync(x => !x.IsDeleted && x.GroupId == group.Id&&x.StudentId == dto.StudentId);
      _transcriptRepository.Remove(transcript);

      var exams = await _examRepository.GetAll(x => x.GroupId == transcript.GroupId && !x.IsDeleted,
          new() { AllUsers = true }).ToListAsync();
      foreach (var item in exams)
      {
          var points = await _studentExamRepository.GetAll(
                  x => !x.IsDeleted
                       && x.StudentId == transcript.StudentId
                       && x.ExamId == exam.Id, new() { AllUsers = true })
              .Select(x => x.Point).ToListAsync();
            
          await _transcriptRepository.AddAsync(new()
          {
              StudentId = dto.StudentId,
              GroupId = group.Id,
              TotalPoint = points.Sum()
          });
      }
        return _mapper.Map<StudentExamResponse>(entity);
    }

    public async Task<StudentExamResponse[]> UpdateRangeAsync(StudentExamRequest[] dtos)
    {
        var entities = new List<Domain.Entities.StudentExam>();
        foreach (var dto in dtos)
        {
            var entity = await _studentExamRepository.GetAsync(x => x.Id == dto.Id && !x.IsDeleted);
            _mapper.Map(dto, entity);
            _studentExamRepository.Update(entity);
            _unitOfWork.SaveChanges();
            /*foreach (var exam in await _examRepository.GetAll(x => x.Id == dto.ExamId && !x.IsDeleted,
                         new() { AllUsers = true }).ToListAsync())
            {
                float point = 0;
                var studentExams = await _studentExamRepository.GetAll(
                        x => !x.IsDeleted&&x.ExamId==exam.Id, new() { AllUsers = true })
                    .ToListAsync();
                foreach (var studentExam in studentExams)
                {
                    point += studentExam.Point;
                }*/

            var exam = await _examRepository.GetAsync(x => !x.IsDeleted && x.Id == entity.ExamId);
            var group = await _groupRepository.GetAsync(x => !x.IsDeleted && x.Id == exam.GroupId);
            var transcript = await _transcriptRepository.GetAsync(x => !x.IsDeleted && x.GroupId == group.Id&&x.StudentId == dto.StudentId);
            _transcriptRepository.Remove(transcript);

            var exams = await _examRepository.GetAll(x => x.GroupId == transcript.GroupId && !x.IsDeleted,
                new() { AllUsers = true }).ToListAsync();
            foreach (var item in exams)
            {
                var points = await _studentExamRepository.GetAll(
                        x => !x.IsDeleted
                             && x.StudentId == transcript.StudentId
                             && x.ExamId == exam.Id, new() { AllUsers = true })
                    .Select(x => x.Point).ToListAsync();
           await _transcriptService.CreateAsync(new(dto.StudentId,group.Id,points.Sum()));
            }
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
            .Include(x => x.Exam).ToListAsync();
        return _mapper.Map<IList<StudentExamResponse>>(entities);
    }
}