using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Email;
using LearningManagementSystem.Application.Abstractions.Services.Hubs;
using LearningManagementSystem.Domain.Entities;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.SignalR.Hubs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;

namespace LearningManagementSystem.SignalR.HubService;

public class StudentExamHubService(IHubContext<StudentExamHub> _hubContext,
    IEmailService _emailService,
    UserManager<AppUser> _userManager,
    IGenericRepository<Student> _studentRepository):IStudentExamHubService
{

    public async Task StudentExamAddedService(Guid studentId, string message)
    {
        var student=await _studentRepository.GetAsync(x=>!x.IsDeleted&&x.Id==studentId);
        var user = await _userManager.FindByIdAsync(student.AppUserId);
       await _emailService.SendEmailAsync(message,"Exam Result",user?.Email);
        await _hubContext.Clients.All.SendAsync("ReceiveExamResult",studentId,message);
    }
}