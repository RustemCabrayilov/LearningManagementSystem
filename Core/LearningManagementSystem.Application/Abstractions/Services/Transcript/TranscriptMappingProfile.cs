using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Transcript;

public class TranscriptMappingProfile:Profile
{
    public TranscriptMappingProfile()
    {
        CreateMap<TranscriptRequest, Domain.Entities.Transcript>();
        CreateMap<Domain.Entities.Transcript, TranscriptResponse>();
    }
}