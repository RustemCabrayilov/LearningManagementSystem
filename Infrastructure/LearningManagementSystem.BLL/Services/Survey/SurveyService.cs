using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Survey;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Survey;

public class SurveyService(
    IGenericRepository<Domain.Entities.Survey> _surveyRepository,
    IGenericRepository<Domain.Entities.Question> _questionRepository,
    IGenericRepository<Domain.Entities.Vote> _voteRepository,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : ISurveyService
{
    public async Task<SurveyResponse> CreateAsync(SurveyRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Survey>(dto);
        await _surveyRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<SurveyResponse>(entity);
    }

    public async Task<SurveyResponse> UpdateAsync(Guid id, SurveyRequest dto)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<SurveyResponse>(key);
        var entity = await _surveyRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Survey not found");
        _mapper.Map(dto, entity);
        _surveyRepository.Update(entity);
        _unitOfWork.SaveChanges();
        var outDto = _mapper.Map<SurveyResponse>(entity);
        if(data is not null) _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<SurveyResponse> RemoveAsync(Guid id)
    {
        var entity = await _surveyRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Survey not found");
        _surveyRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<SurveyResponse>(entity);
    }

    public async Task<SurveyResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<SurveyResponse>(key);
        if (data is not null)
            return data;
        var entity = await _surveyRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Survey not found");
        var questions = await _questionRepository.GetAll(x => x.SurveyId == entity.Id && !x.IsDeleted, new()
        {
            AllUsers = true
        }).ToListAsync();
        var votes = await _voteRepository.GetAll(x => x.SurveyId == entity.Id && !x.IsDeleted, new()
        {
            AllUsers = true
        }).ToListAsync();
        entity.Questions = questions;
        entity.Votes = votes;
        var outDto=_mapper.Map<SurveyResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<SurveyResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _surveyRepository.GetAll(x => !x.IsDeleted, filter).ToListAsync();
        return _mapper.Map<IList<SurveyResponse>>(entities);
    }

    public async Task<SurveyResponse> Activate(Guid id)
    {
        var entity = await _surveyRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        entity.IsActive = !entity.IsActive;
        _surveyRepository.Update(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<SurveyResponse>(entity);
    }
}