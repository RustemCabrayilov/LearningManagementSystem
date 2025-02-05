using LearningManagementSystem.Application.Abstractions.Services.Exam;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.Term;

namespace LearningManagementSystem.Application.Abstractions.Services.StudentExam;

public record StudentExamResponse(
    Guid Id,
    ExamResponse Exam,
    StudentResponse Student,
    TermResponse Term,
    float Point ,
    List<string> FileUrls=null);