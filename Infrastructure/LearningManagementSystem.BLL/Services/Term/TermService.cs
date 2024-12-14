using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.Term;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Term;

public class TermService(IGenericRepository<Domain.Entities.Term> _termRepository,
    IMapper _mapper,
    IUnitOfWork _unitOfWork):ITermService
{
    public async Task<TermResponse> CreateAsync(TermRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Term>(dto);
        await _termRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TermResponse>(entity);
    }

    public async Task<TermResponse> UpdateAsync(Guid id, TermRequest dto)
    {
        var entity = await _termRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Term not found");
        await _termRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TermResponse>(entity);
    }

    public async Task<TermResponse> RemoveAsync(Guid id)
    {
        var entity = await _termRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Term not found");
        _termRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<TermResponse>(entity);
    }

    public async Task<TermResponse> GetAsync(Guid id)
    {
        var entity = await _termRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
        if(entity is null) throw new NotFoundException("Term not found");
        return _mapper.Map<TermResponse>(entity);
    }

    public async Task<IList<TermResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities =await _termRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
        return _mapper.Map<IList<TermResponse>>(entities);
    }   
}