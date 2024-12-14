using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Major;

public class MajorService(IGenericRepository<Domain.Entities.Major> _majorRepository,
IMapper _mapper,
IUnitOfWork _unitOfWork):IMajorService
{
    public async Task<MajorResponse> CreateAsync(MajorRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Major>(dto);
        await _majorRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<MajorResponse>(entity);
    }

    public async Task<MajorResponse> UpdateAsync(Guid id, MajorRequest dto)
    {
        var entity = await _majorRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Major not found");
        await _majorRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<MajorResponse>(entity);
    }

    public async Task<MajorResponse> RemoveAsync(Guid id)
    {
        var entity = await _majorRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Major not found");
        _majorRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<MajorResponse>(entity);
    }

    public async Task<MajorResponse> GetAsync(Guid id)
    {
        var entity = await _majorRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
        if(entity is null) throw new NotFoundException("Major not found");
        return _mapper.Map<MajorResponse>(entity);
    }

    public async Task<IList<MajorResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities =await _majorRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
        return _mapper.Map<IList<MajorResponse>>(entities);
    }
}