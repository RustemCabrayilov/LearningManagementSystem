using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Group;

namespace LearningManagementSystem.BLL.Services.Group;

public class GroupValidator:AbstractValidator<GroupRequest>
{
    public GroupValidator()
    {
        RuleFor(x=>x.Name).NotEmpty();
        RuleFor(x=>x.Code).NotEmpty();
        RuleFor(x=>x.SubjectId).NotNull();
        RuleFor(x=>x.MajorId).NotNull();
        RuleFor(x=>x.TeacherId).NotNull();
    }
}