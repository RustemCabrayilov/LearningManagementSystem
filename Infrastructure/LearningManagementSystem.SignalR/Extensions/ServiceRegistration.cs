using LearningManagementSystem.Application.Abstractions.Services.Chat;
using LearningManagementSystem.Application.Abstractions.Services.Hubs;
using LearningManagementSystem.SignalR.HubService;
using Microsoft.Extensions.DependencyInjection;

namespace LearningManagementSystem.SignalR.Extensions;

public static class ServiceRegistration
{
    public static void  AddSignalRServices(this IServiceCollection services)
    {
        services.AddTransient<IStudentExamHubService, StudentExamHubService>();
        services.AddTransient<IChatHubService, ChatHubService>();
        services.AddSignalR();
    }
}