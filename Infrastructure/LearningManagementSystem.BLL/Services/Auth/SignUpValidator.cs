using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Auth;

namespace LearningManagementSystem.BLL.Services.Auth;

public class SignUpValidator:AbstractValidator<SignUpRequest>
{
    public SignUpValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .Length(6, 50);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty()
            .Length(6, 50)
            .Must((x, confirmPassword) => confirmPassword == x.Password);
        RuleFor(x => x.PhoneNumber)
            .NotEmpty()
            .Matches(@"^\+994\d{9}$");

    }
}