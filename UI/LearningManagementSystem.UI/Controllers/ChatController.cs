using LearningManagementSystem.Application.Abstractions.Services.Chat;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class ChatController(
    ILearningManagementSystem _learningManagementSystem,
    IHttpContextAccessor _httpContextAccessor) : Controller
{
    public async Task<IActionResult> Chat()
    {
        var token = _httpContextAccessor?.HttpContext?.Request.Cookies["access_token"];
        var userclaim = await _learningManagementSystem.GetUserInfosByToken(token);
        string userId = userclaim.Id;
        var response = await _learningManagementSystem.GetChatUsers(userId);
        return View(response);
    }

    public async Task<IActionResult> SendMessage(ChatRequest request)
    {
        var response = await _learningManagementSystem.SendMessage(request);
        return Json(response);
    }

    public async Task<IActionResult> GetChatMessages([FromBody]ChatMessageDto request)
    {
        var response = await _learningManagementSystem.GetChatMessages(new(request.UserId,request.ToUserId));
        return Json(response);
    }
}