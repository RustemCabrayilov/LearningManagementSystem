using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Survey;

public class SurveyMappingProfile:Profile
{
    public SurveyMappingProfile()
    {
        CreateMap<SurveyRequest, Domain.Entities.Survey>();
        CreateMap<Domain.Entities.Survey, SurveyResponse>();
    } 
}