using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Student;

namespace LearningManagementSystem.Application.Abstractions.Services.Student;

public class StudentMappingProfile: Profile
{
    public StudentMappingProfile()
    {
        CreateMap<StudentRequest, Domain.Entities.Student>();
        CreateMap<Domain.Entities.Student, StudentResponse>();
        CreateMap<StudentGroupDto,Domain.Entities.StudentGroup>();
        CreateMap<StudentMajorDto,Domain.Entities.StudentMajor>();
        CreateMap<StudentSubjectDto,Domain.Entities.StudentSubject>();
        CreateMap<StudentLessonDto,Domain.Entities.StudentLesson>();
        CreateMap<StudentExamDto,Domain.Entities.StudentExam>();
        CreateMap<Domain.Entities.StudentLesson,StudentResponse>();
        CreateMap<Domain.Entities.StudentGroup,StudentResponse>();
        CreateMap<Domain.Entities.StudentMajor,StudentResponse>();
        CreateMap<Domain.Entities.StudentSubject,StudentResponse>();
        CreateMap<Domain.Entities.StudentExam,StudentResponse>();
    }
}