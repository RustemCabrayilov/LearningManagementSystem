using LearningManagementSystem.Application.Abstractions.Services.Chat;
using LearningManagementSystem.Application.Abstractions.Services.Hubs;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.SignalR.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace LearningManagementSystem.SignalR.HubService;

public class ChatHubService:IChatHubService
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly UserManager<AppUser> _userManager;

    public ChatHubService(IHubContext<ChatHub> hubContext, UserManager<AppUser> userManager)
    {
        _hubContext = hubContext;
        _userManager = userManager;
    }

    public async Task SendMessage(ChatRequest request)
    {
        var receiver = await _userManager.FindByIdAsync(request.ToUserId);
        if (receiver is not null)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", request.UserId, request.Message);
        }
    }
}