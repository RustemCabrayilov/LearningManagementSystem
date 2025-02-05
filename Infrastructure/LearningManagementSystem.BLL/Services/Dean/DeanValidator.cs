using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Dean;

namespace LearningManagementSystem.BLL.Services.Dean;

public class DeanValidator:AbstractValidator<DeanRequest>
{
    public DeanValidator()
    {
        RuleFor(x=>x.Name).Length(1,100).NotEmpty();
        RuleFor(x=>x.Surname).Length(1,100).NotEmpty();
        RuleFor(x=>x.Salary).GreaterThanOrEqualTo(300).NotEmpty();
        RuleFor(x=>x.File).NotEmpty();
        RuleFor(x=>x.FacultyId).NotNull();
        RuleFor(x=>x.AppUserId).NotNull();
    }
}