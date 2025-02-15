using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Term;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Term;

public class TermService(
    IGenericRepository<Domain.Entities.Term> _termRepository,
    IGenericRepository<Domain.Entities.Group> _groupRepository,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : ITermService
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
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<TermResponse>(key);
        var entity = await _termRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Term not found");
        _mapper.Map(dto, entity);
        _termRepository.Update(entity);
        _unitOfWork.SaveChanges();
        var outDto=_mapper.Map<TermResponse>(entity);
        if(data is not null) _redisCachingService.SetData(key, outDto);
        return outDto;
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
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<TermResponse>(key);
        if (data is not null)
            return data;
        var entity = await _termRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Term not found");
        var groups = await _groupRepository.GetAsync(x => x.TermId == entity.Id);
        var outDto = _mapper.Map<TermResponse>(entity) with
        {
            Groups = _mapper.Map<List<GroupResponse>>(groups)
        };
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<TermResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _termRepository.GetAll(x => !x.IsDeleted, filter).ToListAsync();
        return _mapper.Map<IList<TermResponse>>(entities);
    }

    public async Task<TermResponse> Activate(Guid id)
    {
        var entity = await _termRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        entity.IsActive = !entity.IsActive;
        _termRepository.Update(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<TermResponse>(entity);
    }
}