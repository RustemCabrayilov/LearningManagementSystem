using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Result;

public class ResultMappingProfile:Profile
{
    public ResultMappingProfile()
    {
        CreateMap<ResultRequest, Domain.Entities.StudentExam>();
        CreateMap<Domain.Entities.StudentExam, ResultResponse>();
    }   
}