using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Student;

namespace LearningManagementSystem.BLL.Services.StudentRetakeExam;

public class StudentRetakeExamValidator: AbstractValidator<StudentRetakeExamDto>
{
    public StudentRetakeExamValidator()
    {
        RuleFor(x => x.StudentId).NotNull();
        RuleFor(x => x.RetakeExamId).NotNull();
        RuleFor(x => x.NewPoint).GreaterThanOrEqualTo(0);
    }   
}