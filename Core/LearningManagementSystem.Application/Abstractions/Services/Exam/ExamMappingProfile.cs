using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Exam;

public class ExamMappingProfile: Profile
{
    public ExamMappingProfile()
    {
        CreateMap<ExamRequest, Domain.Entities.Exam>();
        CreateMap<Domain.Entities.Exam, ExamResponse>();
    }
}