using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Subject;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Subject;

public class SubjectService(
    IGenericRepository<Domain.Entities.Subject> _subjectRepository,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : ISubjectService
{
    public async Task<SubjectResponse> CreateAsync(SubjectRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Subject>(dto);
        await _subjectRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<SubjectResponse>(entity);
    }

    public async Task<SubjectResponse> UpdateAsync(Guid id, SubjectRequest dto)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<SubjectResponse>(key);
        var entity = await _subjectRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Subject not found");
        _mapper.Map(dto, entity);
        _subjectRepository.Update(entity);
        _unitOfWork.SaveChanges();
        var outDto=_mapper.Map<SubjectResponse>(entity);
        if(data is not null) _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<SubjectResponse> RemoveAsync(Guid id)
    {
        var entity = await _subjectRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Subject not found");
        _subjectRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<SubjectResponse>(entity);
    }

    public async Task<SubjectResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<SubjectResponse>(key);
        if (data is not null)
            return data;
        var entity = await _subjectRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Subject not found");
        var outDto = _mapper.Map<SubjectResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<SubjectResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _subjectRepository.GetAll(x => !x.IsDeleted, filter).ToListAsync();
        return _mapper.Map<IList<SubjectResponse>>(entities);
    }
}