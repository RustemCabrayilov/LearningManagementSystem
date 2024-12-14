using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Vote;

public class VoteMappingProfile:Profile
{
    public VoteMappingProfile()
    {
        CreateMap<VoteRequest, Domain.Entities.Vote>();
        CreateMap<Domain.Entities.Vote, VoteResponse>();
    }   
}