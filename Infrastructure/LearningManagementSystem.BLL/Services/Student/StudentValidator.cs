using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Student;

namespace LearningManagementSystem.BLL.Services.Student;

public class StudentValidator:AbstractValidator<StudentRequest>
{
    public StudentValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        RuleFor(x=>x.Surname).NotEmpty().MaximumLength(300);
        RuleFor(x=>x.File).NotEmpty();
    }
}