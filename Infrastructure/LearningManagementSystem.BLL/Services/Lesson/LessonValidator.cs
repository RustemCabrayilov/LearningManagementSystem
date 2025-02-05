using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Lesson;

namespace LearningManagementSystem.BLL.Services.Lesson;

public class LessonValidator:AbstractValidator<LessonRequest>
{
    public LessonValidator()
    {
        RuleFor(x => x.GroupId).NotEmpty();
    }
}