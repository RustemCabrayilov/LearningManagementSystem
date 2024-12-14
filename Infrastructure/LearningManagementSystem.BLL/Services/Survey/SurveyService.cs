using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Survey;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Survey;

public class SurveyService(IGenericRepository<Domain.Entities.Survey> _SurveyRepository,
IMapper _mapper,
IUnitOfWork _unitOfWork) : ISurveyService
{
public async Task<SurveyResponse> CreateAsync(SurveyRequest dto)
{
    var entity = _mapper.Map<Domain.Entities.Survey>(dto);
    await _SurveyRepository.AddAsync(entity);
    await _unitOfWork.SaveChangesAsync();
    return _mapper.Map<SurveyResponse>(entity);
}

public async Task<SurveyResponse> UpdateAsync(Guid id, SurveyRequest dto)
{
    var entity = await _SurveyRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
    if (entity is null) throw new NotFoundException("Survey not found");
    await _SurveyRepository.AddAsync(entity);
    await _unitOfWork.SaveChangesAsync();
    return _mapper.Map<SurveyResponse>(entity);
}

public async Task<SurveyResponse> RemoveAsync(Guid id)
{
    var entity = await _SurveyRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
    if (entity is null) throw new NotFoundException("Survey not found");
    _SurveyRepository.Remove(entity);
    _unitOfWork.SaveChanges();
    return _mapper.Map<SurveyResponse>(entity);
}

public async Task<SurveyResponse> GetAsync(Guid id)
{
    var entity = await _SurveyRepository.GetAsync(x=>x.Id==id&&!x.IsDeleted);
    if(entity is null) throw new NotFoundException("Survey not found");
    return _mapper.Map<SurveyResponse>(entity);
}

public async Task<IList<SurveyResponse>> GetAllAsync(RequestFilter? filter)
{
    var entities =await _SurveyRepository.GetAll(x=>!x.IsDeleted,filter).ToListAsync();
    return _mapper.Map<IList<SurveyResponse>>(entities);
} 
}