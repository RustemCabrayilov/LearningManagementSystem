using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.User;

namespace LearningManagementSystem.BLL.Services.User;

public class UserValidator : AbstractValidator<UserRequest>
{
    public UserValidator()
    {
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+994\d{9}$");
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}