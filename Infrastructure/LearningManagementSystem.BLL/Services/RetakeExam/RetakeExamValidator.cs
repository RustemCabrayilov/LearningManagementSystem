using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;

namespace LearningManagementSystem.BLL.Services.RetakeExam;

public class RetakeExamValidator:AbstractValidator<RetakeExamRequest>
{
    public RetakeExamValidator()
    {
        RuleFor(x => x.ExamId).NotNull();
        RuleFor(x => x.Deadline).NotNull();
        RuleFor(x => x.Price).GreaterThanOrEqualTo(0);
    }
}