using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Major;

public class MajorMappingProfile:Profile
{
    public MajorMappingProfile()
    {
        CreateMap<MajorRequest, Domain.Entities.Major>();
        CreateMap<Domain.Entities.Major, MajorResponse>();
        CreateMap<MajorResponse,MajorRequest>()
            .ForMember(des => des.FacultyId,
                opt => opt.MapFrom(src => src.Faculty.Id));
        
    }   
}