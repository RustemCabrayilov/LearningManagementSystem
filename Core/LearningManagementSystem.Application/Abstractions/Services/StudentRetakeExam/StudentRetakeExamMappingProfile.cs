using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Services.Student;

namespace LearningManagementSystem.Application.Abstractions.Services.StudentRetakeExam;

public class StudentRetakeExamMappingProfile: Profile
{
    public StudentRetakeExamMappingProfile()
    {
        CreateMap<StudentRetakeExamDto, Domain.Entities.StudentRetakeExam>();
        CreateMap<Domain.Entities.StudentExam, StudentRetakeExamResponse>();   
    }
}