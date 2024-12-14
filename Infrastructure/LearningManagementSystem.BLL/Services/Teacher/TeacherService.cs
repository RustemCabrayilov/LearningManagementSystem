using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Teacher;

public class TeacherService(
    IGenericRepository<Domain.Entities.Teacher> _teacherRepository,
    IGenericRepository<Domain.Entities.TeacherGroup> _teacherGroupRepository,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : ITeacherService
{
    public async Task<TeacherResponse> CreateAsync(TeacherRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Teacher>(dto);
        await _teacherRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TeacherResponse>(entity);
    }

    public async Task<TeacherResponse> UpdateAsync(Guid id, TeacherRequest dto)
    {
        var entity = await _teacherRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Teacher not found");
        await _teacherRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<TeacherResponse>(entity);
    }

    public async Task<TeacherResponse> RemoveAsync(Guid id)
    {
        var entity = await _teacherRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Teacher not found");
        _teacherRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<TeacherResponse>(entity);
    }

    public async Task<TeacherResponse> GetAsync(Guid id)
    {
        var entity = await _teacherRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Teacher not found");
        return _mapper.Map<TeacherResponse>(entity);
    }

    public async Task<IList<TeacherResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _teacherRepository.GetAll(x => !x.IsDeleted, filter).ToListAsync();
        return _mapper.Map<IList<TeacherResponse>>(entities);
    }

    public async Task<TeacherResponse> AssignGroupAsync(TeacherGroupDto dto)
    {
            var entity = _mapper.Map<Domain.Entities.TeacherGroup>(dto);
            await _teacherGroupRepository.AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<TeacherResponse>(entity);
    }
}