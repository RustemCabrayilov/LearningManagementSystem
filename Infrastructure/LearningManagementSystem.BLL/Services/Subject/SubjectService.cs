using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Subject;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Subject;

public class SubjectService(IGenericRepository<Domain.Entities.Subject> _subjectRepository,
    IMapper _mapper,
    IUnitOfWork _unitOfWork):ISubjectService
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
        var entity = await _subjectRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Subject not found");
        await _subjectRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<SubjectResponse>(entity);
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
        var entity = await _subjectRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
        if(entity is null) throw new NotFoundException("Subject not found");
        return _mapper.Map<SubjectResponse>(entity);
    }

    public async Task<IList<SubjectResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities =await _subjectRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
        return _mapper.Map<IList<SubjectResponse>>(entities);
    }  
    
}