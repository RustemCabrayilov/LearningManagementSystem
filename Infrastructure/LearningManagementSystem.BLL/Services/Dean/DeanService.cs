using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Dean;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Dean;

public class DeanService(
    IGenericRepository<Domain.Entities.Dean> _deanRepository,
    IMapper _mapper,
    IUnitOfWork _unitOfWork):IDeanService
{
    public async Task<DeanResponse> CreateAsync(DeanRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Dean>(dto);
        await _deanRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<DeanResponse>(entity);
    }

    public async Task<DeanResponse> UpdateAsync(Guid id, DeanRequest dto)
    {
        var entity = await _deanRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Dean not found");
        await _deanRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<DeanResponse>(entity);
    }

    public async Task<DeanResponse> RemoveAsync(Guid id)
    {
        var entity = await _deanRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Dean not found");
        _deanRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<DeanResponse>(entity);
    }

    public async Task<DeanResponse> GetAsync(Guid id)
    {
        var entity = await _deanRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
        if(entity is null) throw new NotFoundException("Dean not found");
        return _mapper.Map<DeanResponse>(entity);
    }

    public async Task<IList<DeanResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities =await _deanRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
        return _mapper.Map<IList<DeanResponse>>(entities);
    }  
}