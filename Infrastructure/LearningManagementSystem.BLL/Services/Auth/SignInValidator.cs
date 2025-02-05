using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Auth;

namespace LearningManagementSystem.BLL.Services.Auth;

public class SignInValidator:AbstractValidator<SignInRequest>
{
    public SignInValidator()
    {
        RuleFor(x => x.Password).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty().MaximumLength(250);
    }   
}