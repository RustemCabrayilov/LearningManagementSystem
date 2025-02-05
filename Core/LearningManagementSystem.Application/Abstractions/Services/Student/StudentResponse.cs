using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.Lesson;
using LearningManagementSystem.Application.Abstractions.Services.StudentExam;
using LearningManagementSystem.Application.Abstractions.Services.StudentRetakeExam;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Entities.Identity;

namespace LearningManagementSystem.Application.Abstractions.Services.Student;

public record StudentResponse(
    Guid Id,
    string Name,
    string Surname,
    string StudentNo,
    AppUser AppUser,
    string QrCodeUrl="",
    string FileUrl=null,
    IList<GroupResponse> Groups=null,
    IList<StudentExamResponse> StudentExams=null,
    IList<StudentRetakeExamResponse> StudentRetakeExams=null ); 