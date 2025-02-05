using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.GroupSchedule;

namespace LearningManagementSystem.BLL.Services.GroupSchedule;

public class GroupScheduleValidator:AbstractValidator<GroupScheduleRequest>
{
    public GroupScheduleValidator()
    {
        RuleFor(x => x.GroupId).NotNull();
        RuleFor(x => x.ClassTime).NotEmpty();
    }
}