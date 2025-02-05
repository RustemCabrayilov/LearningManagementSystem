using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Subject;

namespace LearningManagementSystem.BLL.Services.Subject;

public class SubjectValidator:AbstractValidator<SubjectRequest>
{
    public SubjectValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        RuleFor(x => x.AttendanceLimit).NotEmpty();
        RuleFor(x => x.SubjectCode).NotEmpty();
    }
}