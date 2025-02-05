using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Services.Chat;
using LearningManagementSystem.Application.Abstractions.Services.Hubs;
using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.BLL.Helpers;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace LearningManagementSystem.BLL.Services.Chat;

public class ChatService(UserManager<AppUser> _userManager,
    IMapper _mapper,
    IChatHubService _chatHubService):IChatService
{
    public async Task<ChatResponse> GetChatUsersAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new NotFoundException("User not found");
        bool isAdmin = await _userManager.IsInRoleAsync(user, RoleHelper.Admin);
        bool isTeacher = await _userManager.IsInRoleAsync(user, RoleHelper.Teacher);
        bool isStudent = await _userManager.IsInRoleAsync(user, RoleHelper.Student);
        ChatResponse response = new();
        if (!isAdmin && !isTeacher && !isStudent)
        {
            throw new Exception("User doesnt contain any role to chat");
        }
        if (isAdmin)
        {
            var students = await _userManager.GetUsersInRoleAsync(RoleHelper.Student);
            var teachers = await _userManager.GetUsersInRoleAsync(RoleHelper.Teacher);
            response = new ChatResponse
            {
                Users = (await GetUsersWithRoles(students))
            };
                response.Users.AddRange(await GetUsersWithRoles(teachers));
            
    
        }
        else if (isTeacher)
        {
            var students = await _userManager.GetUsersInRoleAsync(RoleHelper.Student);
            var admins = await _userManager.GetUsersInRoleAsync(RoleHelper.Admin);
            response = new ChatResponse
            {
                Users = (await GetUsersWithRoles(students))
            };
            response.Users.AddRange(await GetUsersWithRoles(admins));
        }
        else if(isStudent)
        {
            var admins = await _userManager.GetUsersInRoleAsync(RoleHelper.Admin);
            var teachers = await _userManager.GetUsersInRoleAsync(RoleHelper.Teacher);
            response = new ChatResponse
            {
                Users = (await GetUsersWithRoles(admins))
            };
            response.Users.AddRange(await GetUsersWithRoles(teachers));
        }

        return response;
    }

    public async Task<ChatRequest> SendChatMessageAsync(ChatRequest request)
    {

        await _chatHubService.SendMessage(request);
        return request;
    }
    private async Task<List<UserResponse>> GetUsersWithRoles(IEnumerable<AppUser> users)
    {
        List<UserResponse> userResponses = new List<UserResponse>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userResponses.Add(_mapper.Map<UserResponse>(user) with{Roles = roles.ToList()});
        }

        return userResponses;
    }
    
    
}