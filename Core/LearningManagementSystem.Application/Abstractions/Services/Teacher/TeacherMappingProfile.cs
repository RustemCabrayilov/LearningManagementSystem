using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Teacher;

public class TeacherMappingProfile:Profile
{
    public TeacherMappingProfile()
    {
        CreateMap<TeacherRequest, Domain.Entities.Teacher>();
        CreateMap<Domain.Entities.Teacher, TeacherResponse>();
    }   
}