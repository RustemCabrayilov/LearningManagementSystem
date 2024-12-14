using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Subject;

public class SubjectMappingProfile:Profile
{
    public SubjectMappingProfile()
    {
        CreateMap<SubjectRequest, Domain.Entities.Subject>();
        CreateMap<Domain.Entities.Subject, SubjectResponse>();
    }
}