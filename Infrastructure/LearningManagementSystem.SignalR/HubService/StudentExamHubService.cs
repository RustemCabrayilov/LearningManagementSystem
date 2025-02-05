using LearningManagementSystem.Application.Abstractions.Services.Hubs;
using LearningManagementSystem.SignalR.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace LearningManagementSystem.SignalR.HubService;

public class StudentExamHubService(IHubContext<StudentExamHub> _hubContext):IStudentExamHubService
{

    public async Task StudentExamAddedService(Guid studentId, string message)
    {
        await _hubContext.Clients.All.SendAsync("ReceiveExamResult",studentId,message);
    }
}