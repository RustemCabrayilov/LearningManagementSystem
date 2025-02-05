using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;

namespace LearningManagementSystem.BLL.Services.Teacher;

public class TeacherValidator:AbstractValidator<TeacherRequest>
{
    public TeacherValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        RuleFor(x => x.Surname).NotEmpty().MaximumLength(300);
        RuleFor(x => x.File).NotEmpty();
        RuleFor(x => x.Occupation).NotEmpty();
        RuleFor(x => x.FacultyId).NotNull();
        RuleFor(x => x.AppUserId).NotEmpty();
        RuleFor(x => x.Salary).NotEmpty().GreaterThanOrEqualTo(300);
    }
}