using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Term;

public class TermMappingProfile:Profile
{
    public TermMappingProfile()
    {
        CreateMap<TermRequest, Domain.Entities.Term>();
        CreateMap<Domain.Entities.Term, TermResponse>();
    }   
}