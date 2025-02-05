using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Question;

namespace LearningManagementSystem.BLL.Services.Question;

public class QuestionValidator:AbstractValidator<QuestionRequest>
{
    public QuestionValidator()
    {
        RuleFor(x => x.Description).NotEmpty().MaximumLength(250);
        RuleFor(x => x.MaxPoint).NotEmpty().LessThanOrEqualTo(5);
        RuleFor(x => x.SurveyId).NotNull();
    }   
}