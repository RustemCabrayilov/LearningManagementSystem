using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Dean;
using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Dean;

public class DeanService(
    IGenericRepository<Domain.Entities.Dean> _deanRepository,
    IMapper _mapper,
    IDocumentService _documentService,
    IRedisCachingService _redisCachingService,
    IUnitOfWork _unitOfWork):IDeanService
{
    public async Task<DeanResponse> CreateAsync(DeanRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Dean>(dto);
        entity.FacultyId=Guid.Parse(dto.FacultyId);
        await _deanRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
      await  _documentService.CreateByOwnerAsync(new DocumentByOwner(new(){dto.File},entity.Id,DocumentType.Dean));
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<DeanResponse>(entity);
    }

    public async Task<DeanResponse> UpdateAsync(Guid id, DeanRequest dto)
    {
        var entity = await _deanRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Dean not found");
        _mapper.Map(dto, entity);
         _deanRepository.Update(entity);
        _unitOfWork.SaveChanges();
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
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<DeanResponse>(key);
        if (data is not null)
            return data;
        var entity = await _deanRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
        if(entity is null) throw new NotFoundException("Dean not found");
        var outDto = _mapper.Map<DeanResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<DeanResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities =await _deanRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
        return _mapper.Map<IList<DeanResponse>>(entities);
    }  
}