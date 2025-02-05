using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Survey;

namespace LearningManagementSystem.BLL.Services.Survey;

public class SurveyValidator:AbstractValidator<SurveyRequest>
{
    public SurveyValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(250);
        RuleFor(x => x.TermId).NotNull();
    }
}