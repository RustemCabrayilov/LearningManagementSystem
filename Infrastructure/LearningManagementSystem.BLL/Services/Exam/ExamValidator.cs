using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Exam;

namespace LearningManagementSystem.BLL.Services.Exam;

public class ExamValidator:AbstractValidator<ExamRequest>
{
    public ExamValidator()
    {
        RuleFor(x=>x.StartDate).NotEmpty();
        RuleFor(x=>x.EndDate).NotEmpty();
        RuleFor(x=>x.GroupId).NotEmpty();
    }
}