using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.Services.Chat;
using LearningManagementSystem.Application.Abstractions.Services.Hubs;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.BLL.Helpers;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.BLL.Services.Chat;

public class ChatService(
    UserManager<AppUser> _userManager,
    IGenericRepository<Domain.Entities.Chat> _chatRepository,
    IUnitOfWork _unitOfWork,
    IMapper _mapper,
    IChatHubService _chatHubService) : IChatService
{
    public async Task<List<UserResponse>> GetChatUsersAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user is null) throw new NotFoundException("User not found");
        bool isAdmin = await _userManager.IsInRoleAsync(user, RoleHelper.Admin);
        bool isTeacher = await _userManager.IsInRoleAsync(user, RoleHelper.Teacher);
        bool isStudent = await _userManager.IsInRoleAsync(user, RoleHelper.Student);
       List<UserResponse> users = new List<UserResponse>();
        if (!isAdmin && !isTeacher && !isStudent)
        {
            throw new Exception("User doesnt contain any role to chat");
        }

        if (isAdmin)
        {
            var students = await _userManager.GetUsersInRoleAsync(RoleHelper.Student);
            var teachers = await _userManager.GetUsersInRoleAsync(RoleHelper.Teacher);

            users = await GetUsersWithRoles(students);
            users.AddRange(await GetUsersWithRoles(teachers));
        }
        else if (isTeacher)
        {
            var students = await _userManager.GetUsersInRoleAsync(RoleHelper.Student);
            var admins = await _userManager.GetUsersInRoleAsync(RoleHelper.Admin);

            users = await GetUsersWithRoles(students);
            users.AddRange(await GetUsersWithRoles(admins));
        }
        else if (isStudent)
        {
            var admins = await _userManager.GetUsersInRoleAsync(RoleHelper.Admin);
            var teachers = await _userManager.GetUsersInRoleAsync(RoleHelper.Teacher);
            users = await GetUsersWithRoles(admins);
            users.AddRange(await GetUsersWithRoles(teachers));
        }

        return users;
    }

    public async Task<List<ChatResponse>> GetChatMessages(ChatMessageDto request)
    {
        var chats = await _chatRepository.GetAll(p =>
                p.UserId == request.UserId && p.ToUserId ==request.ToUserId ||
                p.ToUserId == request.UserId && p.UserId ==request.ToUserId,new RequestFilter(){AllUsers = true})
            .OrderBy(p => p.Date)
            .ToListAsync();
          return _mapper.Map<List<ChatResponse>>(chats);
    }

    public async Task<ChatResponse> SendChatMessageAsync(ChatRequest request)
    {
        var chat=_mapper.Map<Domain.Entities.Chat>(request);
         await _chatRepository.AddAsync(new()
        {
            UserId = request.UserId,
            ToUserId = request.ToUserId,
            Date = DateTime.Now,
            Message = request.Message,
        });
        await _unitOfWork.SaveChangesAsync();
        await _chatHubService.SendMessage(request);
        return _mapper.Map<ChatResponse>(chat);
    }

    private async Task<List<UserResponse>> GetUsersWithRoles(IEnumerable<AppUser> users)
    {
        List<UserResponse> userResponses = new List<UserResponse>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            userResponses.Add(_mapper.Map<UserResponse>(user) with { Roles = roles.ToList() });
        }

        return userResponses;
    }
}