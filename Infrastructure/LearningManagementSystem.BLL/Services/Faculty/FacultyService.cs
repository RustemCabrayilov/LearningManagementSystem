using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Faculty;

public class FacultyService(
    IGenericRepository<Domain.Entities.Faculty> _facultyRepository,
    IGenericRepository<Domain.Entities.Teacher> _teacherRepository,
    IGenericRepository<Domain.Entities.Major> _majorRepository,
    IMapper _mapper,
    IRedisCachingService _redisCachingService,
    IUnitOfWork _unitOfWork):IFacultyService
{
    public async Task<FacultyResponse> CreateAsync(FacultyRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Faculty>(dto);
        await _facultyRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<FacultyResponse>(entity);
    }

    public async Task<FacultyResponse> UpdateAsync(Guid id, FacultyRequest dto)
    {
       string key = $"member-{id}";
       var data = _redisCachingService.GetData<FacultyResponse>(key);
        var entity = await _facultyRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Faculty not found");
        _mapper.Map(dto, entity);
         _facultyRepository.Update(entity);
         _unitOfWork.SaveChanges();
        var outDto = _mapper.Map<FacultyResponse>(entity);
        if(data is not null) _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<FacultyResponse> RemoveAsync(Guid id)
    {
        var entity = await _facultyRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Faculty not found");
        _facultyRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<FacultyResponse>(entity);
    }

    public async Task<FacultyResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<FacultyResponse>(key);
        if (data is not null)
            return data;
        var entity = await _facultyRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
        if(entity is null) throw new NotFoundException("Faculty not found");
        entity.Majors = await _majorRepository.GetAll(x=>x.FacultyId==entity.Id&&!x.IsDeleted, new RequestFilter() { AllUsers = true }).ToListAsync();
        entity.Teachers = await _teacherRepository.GetAll(x=>x.FacultyId==entity.Id&&!x.IsDeleted, new RequestFilter() { AllUsers = true }).ToListAsync();
        var outDto = _mapper.Map<FacultyResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<FacultyResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities =await _facultyRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
        return _mapper.Map<IList<FacultyResponse>>(entities);
    }   
}