using LearningManagementSystem.SignalR.Hubs;
using Microsoft.AspNetCore.Builder;

namespace LearningManagementSystem.SignalR.Extensions
{
    public static class HubRegistration
    {
        public static void MapHubs(this WebApplication app)
        {
            app.MapHub<StudentExamHub>("/studentExamHub");
            app.MapHub<ChatHub>("/chatHub");
        }
    }
}