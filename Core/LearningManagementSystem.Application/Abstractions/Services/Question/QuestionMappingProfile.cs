
using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Question;

public class QuestionMappingProfile:Profile
{
    public QuestionMappingProfile()
    {
        CreateMap<QuestionRequest, Domain.Entities.Question>();
        CreateMap<Domain.Entities.Question, QuestionResponse>();   
    }   
}