using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.StudentGroup;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.StudentGroup;

public class StudentGroupService(
    IGenericRepository<Domain.Entities.StudentGroup> _StudentGroupRepository,
    IGenericRepository<Domain.Entities.Transcript> _transcriptRepository,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IStudentGroupService
{
    public async Task<StudentGroupResponse> CreateAsync(StudentGroupDto dto)
    {
        var entity = _mapper.Map<Domain.Entities.StudentGroup>(dto);
        await _StudentGroupRepository.AddAsync(entity);
        var transcript = new Domain.Entities.Transcript()
        {
            StudentId = entity.StudentId,
            GroupId = entity.GroupId
        };
      await  _transcriptRepository.AddAsync(transcript);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<StudentGroupResponse>(entity);
    }


    public async Task<StudentGroupResponse> UpdateAsync(Guid id, StudentGroupDto dto)
    {
        var entity = await _StudentGroupRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Student's Group not found");
        _mapper.Map(dto, entity);
        _StudentGroupRepository.Update(entity);
        _unitOfWork.SaveChanges();
        var transcript = await _transcriptRepository.GetAsync(x => !x.IsDeleted && x.GroupId == entity.GroupId&&x.StudentId == entity.StudentId);
        _transcriptRepository.Remove(transcript);
        var transcriptToCreate = new Domain.Entities.Transcript()
        {
            StudentId = entity.StudentId,
            GroupId = entity.GroupId
        };
        await  _transcriptRepository.AddAsync(transcriptToCreate);
        return _mapper.Map<StudentGroupResponse>(entity);
    }

    public async Task<StudentGroupResponse> RemoveAsync(Guid id)
    {
        var entity = await _StudentGroupRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Student's Group not found");
        _StudentGroupRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<StudentGroupResponse>(entity);
    }

    public async Task<StudentGroupResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<StudentGroupResponse>(key);
        if (data is not null)
            return data;
        var entity = await _StudentGroupRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Student's Group not found");
        var outDto=_mapper.Map<StudentGroupResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<StudentGroupResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _StudentGroupRepository.GetAll(x => !x.IsDeleted, filter)
            .Include(x => x.Student)
            .Include(x => x.Group).ToListAsync();
        return _mapper.Map<IList<StudentGroupResponse>>(entities);
    }
}