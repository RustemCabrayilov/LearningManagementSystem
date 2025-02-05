using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Question;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Question;

public class QuestionService(
    IGenericRepository<Domain.Entities.Question> _questionRepository,
    IRedisCachingService _redisCachingService,
    IUnitOfWork _unitOfWork,
    IMapper _mapper) : IQuestionService
{
    public async Task<QuestionResponse> CreateAsync(QuestionRequest dto)
    {
        var entity = _mapper.Map<Domain.Entities.Question>(dto);
        await _questionRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();
        return _mapper.Map<QuestionResponse>(entity);
    }

    public async Task<QuestionResponse> UpdateAsync(Guid id, QuestionRequest dto)
    {
        var entity = await _questionRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Question not found");
        _mapper.Map(dto, entity);
        _questionRepository.Update(entity);
        _unitOfWork.SaveChanges();
        var outDto = _mapper.Map<QuestionResponse>(entity);
        return outDto;
    }

    public async Task<QuestionResponse> RemoveAsync(Guid id)
    {
        var entity = await _questionRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Question not found");
        _questionRepository.Remove(entity);
        _unitOfWork.SaveChanges();
        return _mapper.Map<QuestionResponse>(entity);
    }

    public async Task<QuestionResponse> GetAsync(Guid id)
    {
        string key = $"member-{id}";
        var data = _redisCachingService.GetData<QuestionResponse>(key);
        if (data is not null)
            return data;
        var entity = await _questionRepository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException("Question not found");
        var outDto = _mapper.Map<QuestionResponse>(entity);
        _redisCachingService.SetData(key, outDto);
        return outDto;
    }

    public async Task<IList<QuestionResponse>> GetAllAsync(RequestFilter? filter)
    {
        var entities = await _questionRepository.GetAll(x => !x.IsDeleted, filter).ToListAsync();
        return _mapper.Map<IList<QuestionResponse>>(entities);
    }
}