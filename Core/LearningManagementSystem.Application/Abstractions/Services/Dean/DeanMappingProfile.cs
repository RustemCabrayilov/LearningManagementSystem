using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Dean;

public class DeanMappingProfile:Profile
{
    public DeanMappingProfile()
    {
        CreateMap<DeanRequest, Domain.Entities.Teacher>();
        CreateMap<Domain.Entities.Teacher, DeanResponse>();
    }
}