using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Chat;

namespace LearningManagementSystem.BLL.Services.Chat;

public class ChatMessageValidator:AbstractValidator<ChatMessageDto>
{
    public ChatMessageValidator()
    {
        RuleFor(x => x.UserId).NotNull();
        RuleFor(x => x.ToUserId).NotNull();
    }    
}