using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Result;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Result;

public class ResultService(
IGenericRepository<Domain.Entities.StudentExam> _ResultRepository,
    IMapper _mapper,
IUnitOfWork _unitOfWork) : IResultService
{
    public async Task<ResultResponse> CreateAsync(ResultRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.StudentExam>(dto);
        await _ResultRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ResultResponse>(entity);
    }

    public async Task<ResultResponse> UpdateAsync(Guid id, ResultRequest dto)
    {
        var entity = await _ResultRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Result not found");
        await _ResultRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<ResultResponse>(entity);
    }

    public async Task<ResultResponse> RemoveAsync(Guid id)
    {
        var entity = await _ResultRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Result not found");
        _ResultRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<ResultResponse>(entity);
    }

    public async Task<ResultResponse> GetAsync(Guid id)
    {
        var entity = await _ResultRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
        if(entity is null) throw new NotFoundException("Result not found");
        return _mapper.Map<ResultResponse>(entity);
    }

    public async Task<IList<ResultResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities =await _ResultRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
        return _mapper.Map<IList<ResultResponse>>(entities);
    }
    
}