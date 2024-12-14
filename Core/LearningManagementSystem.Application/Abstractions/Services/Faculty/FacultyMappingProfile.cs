using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Faculty;

public class FacultyMappingProfile:Profile
{
    public FacultyMappingProfile()
    {
        CreateMap<FacultyRequest, Domain.Entities.Faculty>();
        CreateMap<Domain.Entities.Faculty, FacultyResponse>();
    }
}