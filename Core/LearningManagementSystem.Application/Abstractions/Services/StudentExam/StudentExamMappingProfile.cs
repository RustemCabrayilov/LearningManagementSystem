using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.StudentExam;

public class StudentExamMappingProfile:Profile
{
    public StudentExamMappingProfile()
    {
        CreateMap<StudentExamRequest, Domain.Entities.StudentExam>();
        CreateMap<Domain.Entities.StudentExam, StudentExamResponse>();
    }
}