using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.RetakeExam;

public class RetakeExamService(  
    IGenericRepository<Domain.Entities.RetakeExam> _RetakeExamRepository,
    IGenericRepository<Domain.Entities.Exam> _examRepository,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) :IRetakeExamService
{
        public async Task<RetakeExamResponse> CreateAsync(RetakeExamRequest dto)
        {
            var entity = _mapper.Map<Domain.Entities.RetakeExam>(dto);
            await _RetakeExamRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
   
            return _mapper.Map<RetakeExamResponse>(entity);
        }

        public async Task<RetakeExamResponse> UpdateAsync(Guid id, RetakeExamRequest dto)
        {
            string key = $"member-{id}";
            var data = _redisCachingService.GetData<RetakeExamResponse>(key);
            var entity = await _RetakeExamRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
            if (entity is null) throw new NotFoundException("RetakeExam not found");
            _mapper.Map(dto, entity);
             _RetakeExamRepository.Update(entity);
             _unitOfWork.SaveChanges();
             var outDto=_mapper.Map<RetakeExamResponse>(entity);
             if(data is not null) _redisCachingService.SetData(key, outDto);
             return outDto;
        }

        public async Task<RetakeExamResponse> RemoveAsync(Guid id)
        {
            var entity = await _RetakeExamRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
            if (entity is null) throw new NotFoundException("RetakeExam not found");
            _RetakeExamRepository.Remove(entity);
            _unitOfWork.SaveChanges();
            return _mapper.Map<RetakeExamResponse>(entity);
        }

        public async Task<RetakeExamResponse> GetAsync(Guid id)
        {
            string key = $"member-{id}";
            var data = _redisCachingService.GetData<RetakeExamResponse>(key);
            if (data is not null)
                return data;
            var entity = await _RetakeExamRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
            if(entity is null) throw new NotFoundException("RetakeExam not found");
            entity.Exam=await _examRepository.GetAsync(x => x.Id == entity.ExamId&&!x.IsDeleted);
            var outDto = _mapper.Map<RetakeExamResponse>(entity);
            _redisCachingService.SetData(key, outDto);
            return outDto;
        }

        public async Task<IList<RetakeExamResponse>> GetAllAsync(RequestFilter? filter)
        {
            var entities =await _RetakeExamRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
            return _mapper.Map<IList<RetakeExamResponse>>(entities);
        }
        
}