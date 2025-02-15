using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Chat;

namespace LearningManagementSystem.BLL.Services.Chat;

public class ChatValidator:AbstractValidator<ChatRequest>
{
    public ChatValidator()
    {
        RuleFor(x=>x.UserId).NotNull();
        RuleFor(x=>x.ToUserId).NotNull();
        RuleFor(x=>x.Message).NotEmpty();
    }
}