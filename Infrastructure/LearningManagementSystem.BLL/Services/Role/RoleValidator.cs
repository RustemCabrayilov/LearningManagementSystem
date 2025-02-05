using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Role;

namespace LearningManagementSystem.BLL.Services.Role;

public class RoleValidator:AbstractValidator<RoleRequest>
{
    public RoleValidator()
    {
        RuleFor(x=>x.Name).NotEmpty();
    }
}