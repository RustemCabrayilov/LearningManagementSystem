using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Dean;

public class DeanMappingProfile:Profile
{
    public DeanMappingProfile()
    {
        CreateMap<DeanRequest, Domain.Entities.Dean>();
        CreateMap<Domain.Entities.Dean, DeanResponse>();
    }
}