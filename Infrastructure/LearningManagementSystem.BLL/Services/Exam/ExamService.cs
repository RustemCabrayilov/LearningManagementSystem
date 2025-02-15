using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Exam;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Exam;

public class ExamService(
    IGenericRepository<Domain.Entities.Exam> _examRepository, 
    IGenericRepository<Domain.Entities.StudentExam> _studentExamRepository, 
    IGenericRepository<Domain.Entities.StudentGroup> _studentGroupRepository, 
    IGenericRepository<Domain.Entities.Group> _groupRepository, 
    IGenericRepository<Domain.Entities.RetakeExam> _retakeExamRepository, 
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork):IExamService
{
    public async Task<ExamResponse> CreateAsync(ExamRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Exam>(dto);
        entity.Name = $"{dto.StartDate}-{dto.EndDate}-{dto.ExamType}";
        await _examRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        var studentGroups = await _studentGroupRepository.GetAll(x=>x.GroupId==entity.GroupId&& !x.IsDeleted,null).ToListAsync();
        foreach (var studentGroup in  studentGroups)
        {
            await _studentExamRepository.AddAsync(new()
            {
                StudentId = studentGroup.StudentId,
                ExamId = entity.Id
            });
            await _unitOfWork.SaveChangesAsync();
        }
        
        return _mapper.Map<ExamResponse>(entity);
    }

    public async Task<ExamResponse> UpdateAsync(Guid id, ExamRequest dto)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<ExamResponse>(key);
        var entity = await _examRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Exam not found");
        _mapper.Map(dto, entity);
        _examRepository.Update(entity); 
        _unitOfWork.SaveChanges();
        var outDto = _mapper.Map<ExamResponse>(entity);
        if(data is not null) _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<ExamResponse> RemoveAsync(Guid id)
    {
        var entity = await _examRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Exam not found");
        _examRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<ExamResponse>(entity);
    }

    public async Task<ExamResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<ExamResponse>(key);
        if (data is not null)
            return data;
        var entity = await _examRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
        if(entity is null) throw new NotFoundException("Exam not found");
        entity.Group=await _groupRepository.GetAsync(x=>x.Id==entity.GroupId&&!x.IsDeleted);
        entity.RetakeExams=await _retakeExamRepository.GetAll(x=>x.ExamId==entity.Id&&!x.IsDeleted,new RequestFilter()
        {
            AllUsers = true
        }).ToListAsync();
        var outDto = _mapper.Map<ExamResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<ExamResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities =await _examRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
        return _mapper.Map<IList<ExamResponse>>(entities);
    }   
}