using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Term;

namespace LearningManagementSystem.BLL.Services.Term;

public class TermValidator:AbstractValidator<TermRequest>
{
    public TermValidator()
    {
        RuleFor(x => x.StartDate).NotEmpty();
        RuleFor(x => x.EndDate).NotEmpty();
    }   
}