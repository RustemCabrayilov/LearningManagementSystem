using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Major;

namespace LearningManagementSystem.BLL.Services.Major;

public class MajorValidator:AbstractValidator<MajorRequest>
{
    public MajorValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(250);
        RuleFor(x=>x.Point).NotEmpty();
        RuleFor(x=>x.EducationLanguage).NotEmpty();
        RuleFor(x=>x.FacultyId).NotNull();
        RuleFor(x=>x.TuitionFee).NotEmpty().GreaterThan(0);
    }
}