using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Vote;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Vote;

public class VoteService(IGenericRepository<Domain.Entities.Vote> _voteRepository,
    IMapper _mapper,
    IUnitOfWork _unitOfWork):IVoteService
{
    public async Task<VoteResponse> CreateAsync(VoteRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Vote>(dto);
        await _voteRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<VoteResponse>(entity);
    }

    public async Task<VoteResponse> UpdateAsync(Guid id, VoteRequest dto)
    {
        var entity = await _voteRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Vote not found");
        await _voteRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<VoteResponse>(entity);
    }

    public async Task<VoteResponse> RemoveAsync(Guid id)
    {
        var entity = await _voteRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Vote not found");
        _voteRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<VoteResponse>(entity);
    }

    public async Task<VoteResponse> GetAsync(Guid id)
    {
        var entity = await _voteRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
        if(entity is null) throw new NotFoundException("Vote not found");
        return _mapper.Map<VoteResponse>(entity);
    }

    public async Task<IList<VoteResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities =await _voteRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
        return _mapper.Map<IList<VoteResponse>>(entities);
    }  
    
}