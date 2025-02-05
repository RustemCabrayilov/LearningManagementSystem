using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Services.Student;

namespace LearningManagementSystem.Application.Abstractions.Services.StudentGroup;

public class StudentGroupMappingProfile:Profile
{
    public StudentGroupMappingProfile()
    {
        CreateMap<StudentGroupDto, Domain.Entities.StudentGroup>();
        CreateMap<Domain.Entities.StudentGroup, StudentGroupResponse>();
    }
}