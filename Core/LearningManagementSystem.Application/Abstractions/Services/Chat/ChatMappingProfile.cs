using AutoMapper;

namespace LearningManagementSystem.Application.Abstractions.Services.Chat;

public class ChatMappingProfile: Profile
{
    public ChatMappingProfile()
    {
        CreateMap<Domain.Entities.Chat, ChatResponse>();
        CreateMap<ChatRequest,Domain.Entities.Chat>();
    }
}