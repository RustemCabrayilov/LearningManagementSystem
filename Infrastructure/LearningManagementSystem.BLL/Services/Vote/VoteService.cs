using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Vote;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Vote;

public class VoteService(
    IGenericRepository<Domain.Entities.Vote> _voteRepository,
    IGenericRepository<Domain.Entities.Teacher> _teacherRepository,
    IGenericRepository<Domain.Entities.Question> _questionRepository,
    IGenericRepository<Domain.Entities.Survey> _surveyRepository,
    IRedisCachingService _redisCachingService,
    IMapper _mapper,
    IUnitOfWork _unitOfWork) : IVoteService
{
    public async Task<VoteResponse> CreateAsync(VoteRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Vote>(dto);
        await _voteRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        var question = await _questionRepository.GetAsync(x => x.Id == entity.QuestionId && !x.IsDeleted);
        var survey = await _surveyRepository.GetAsync(x => x.Id == question.SurveyId && !x.IsDeleted);
        var teacher = await _teacherRepository.GetAsync(x => x.Id == survey.TeacherId && !x.IsDeleted);
        var questionsCount = _questionRepository
            .GetAll(x => x.SurveyId == survey.Id, new RequestFilter() { AllUsers = true }).Count();
        teacher.Rate = (float)dto.Point / questionsCount;
        _teacherRepository.Update(teacher);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<VoteResponse>(entity);
    }

    public async Task<VoteResponse[]> CreateAsync(VoteRequest[] dtos)
    {
        var entities = _mapper.Map<Domain.Entities.Vote[]>(dtos);
        var question = await _questionRepository.GetAsync(x => x.Id == entities[0].QuestionId && !x.IsDeleted);
        var survey = await _surveyRepository.GetAsync(x => x.Id == question.SurveyId && !x.IsDeleted);
        var questions = _questionRepository
            .GetAll(x => x.SurveyId == survey.Id, new RequestFilter() { AllUsers = true });
        var teacher = await _teacherRepository.GetAsync(x => x.Id == survey.TeacherId && !x.IsDeleted);
        foreach (var entity in entities)
        {
            var vote = await _voteRepository.GetAsync(x =>
                x.QuestionId == entity.QuestionId && x.StudentId == entity.StudentId && !x.IsDeleted);
            if (vote != null)
            {
                vote.Point = entity.Point;
                _voteRepository.Update(vote);
                _unitOfWork.SaveChanges();
            }
            else
            {
                await _voteRepository.AddAsync(entity);
                await _unitOfWork.SaveChangesAsync();
            }
        }

        int totalvotePoint = questions.Select(x => x.MaxPoint).Sum();
        int totalStudentPoint = entities.Select(x => x.Point).Sum();
        double rate = Math.Round((((float)totalStudentPoint / totalvotePoint) + teacher.Rate) / 2, 2);
        teacher.Rate = (float)rate;
        _teacherRepository.Update(teacher);
        _unitOfWork.SaveChanges();
        return _mapper.Map<VoteResponse[]>(entities);
    }

    public async Task<VoteResponse> UpdateAsync(Guid id, VoteRequest dto)
    {
        var entity = await _voteRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Vote not found");
        _mapper.Map(dto, entity);
        _voteRepository.Update(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<VoteResponse>(entity);
    }

    public async Task<VoteResponse> RemoveAsync(Guid id)
    {
        var entity = await _voteRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Vote not found");
        _voteRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<VoteResponse>(entity);
    }

    public async Task<VoteResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<VoteResponse>(key);
        if (data is not null)
            return data;
        var entity = await _voteRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Vote not found");
        var outDto = _mapper.Map<VoteResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<VoteResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _voteRepository.GetAll(x => !x.IsDeleted, filter).ToListAsync();
        return _mapper.Map<IList<VoteResponse>>(entities);
    }
}