using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.RetakeExam;

public class RetakeExamService(  
    IGenericRepository<Domain.Entities.RetakeExam> _RetakeExamRepository,
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
            var entity = await _RetakeExamRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
            if (entity is null) throw new NotFoundException("RetakeExam not found");
            await _RetakeExamRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<RetakeExamResponse>(entity);
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
            var entity = await _RetakeExamRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
            if(entity is null) throw new NotFoundException("RetakeExam not found");
            return _mapper.Map<RetakeExamResponse>(entity);
        }

        public async Task<IList<RetakeExamResponse>> GetAllAsync(RequestFilter? filter)
        {
            var entities =await _RetakeExamRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
            return _mapper.Map<IList<RetakeExamResponse>>(entities);
        }
        
}