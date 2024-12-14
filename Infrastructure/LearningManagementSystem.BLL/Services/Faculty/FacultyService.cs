using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Faculty;

public class FacultyService(
    IGenericRepository<Domain.Entities.Faculty> _facultyRepository,
    IMapper _mapper,
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
        var entity = await _facultyRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Faculty not found");
        await _facultyRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<FacultyResponse>(entity);
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
        var entity = await _facultyRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
        if(entity is null) throw new NotFoundException("Faculty not found");
        return _mapper.Map<FacultyResponse>(entity);
    }

    public async Task<IList<FacultyResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities =await _facultyRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
        return _mapper.Map<IList<FacultyResponse>>(entities);
    }   
}