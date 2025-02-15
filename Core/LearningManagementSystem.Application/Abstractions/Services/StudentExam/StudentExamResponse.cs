using LearningManagementSystem.Application.Abstractions.Services.Exam;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.Term;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.StudentExam;

public record StudentExamResponse(
    Guid Id,
    ExamResponse Exam,
    StudentResponse Student,
    float Point,
    List<string> FileUrls=null,
    IFormFile[] Files=null);