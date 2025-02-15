using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Dean;
using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Dean;

public class DeanService(
    IGenericRepository<Domain.Entities.Dean> _deanRepository,
    IGenericRepository<Domain.Entities.Faculty> _facultyRepository,
    UserManager<AppUser> _userManager,
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
      await  _documentService.CreateByOwnerAsync(new DocumentByOwner(new (){dto.File},entity.Id,DocumentType.Dean));
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<DeanResponse>(entity);
    }

    public async Task<DeanResponse> UpdateAsync(Guid id, DeanRequest dto)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<DeanResponse>(key);
        var entity = await _deanRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Dean not found");
        _mapper.Map(dto, entity);
         _deanRepository.Update(entity);
        _unitOfWork.SaveChanges();
        var document = await _documentService.GetByOwnerId(id);
        await _documentService.UpdateAsync(document.Id, new(document.Id,document.DocumentType,document.Path,document.Key,
            document.FileName,document.OriginName,document.OwnerId,new(){dto.File}));
        var outDto=_mapper.Map<DeanResponse>(entity);
        _redisCachingService.SetData(key,outDto);
        return outDto;
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
        var faculty=await _facultyRepository.GetAsync(x=>x.Id==entity.FacultyId&&!x.IsDeleted);
        var user = await _userManager.FindByIdAsync(entity.AppUserId);
        entity.Faculty = faculty;
        entity.AppUser = user;
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