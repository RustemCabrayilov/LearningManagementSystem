using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Vote;

namespace LearningManagementSystem.BLL.Services.Vote;

public class VoteValidator:AbstractValidator<VoteRequest>
{
    public VoteValidator()
    {
        RuleFor(x => x.StudentId).NotNull();
        RuleFor(x => x.Point).NotEmpty().GreaterThanOrEqualTo(0).LessThan(5);
    }
}