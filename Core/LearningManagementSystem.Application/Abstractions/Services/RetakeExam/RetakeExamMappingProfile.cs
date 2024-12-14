using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.RetakeExam;

public class RetakeExamMappingProfile:Profile
{
    public RetakeExamMappingProfile()
    {
        CreateMap<RetakeExamRequest, Domain.Entities.RetakeExam>();
        CreateMap<Domain.Entities.RetakeExam, RetakeExamResponse>();   
    }
}