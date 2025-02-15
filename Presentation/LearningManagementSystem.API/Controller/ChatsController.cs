using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.Application.Abstractions.Services.Chat;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class ChatsController(IChatService _chatService) : ControllerBase
{
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Student,Teacher")]
    public async Task<IActionResult> Get([FromRoute] string id)
    {
        var response = await _chatService.GetChatUsersAsync(id);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Student,Teacher")]
    [ServiceFilter(typeof(ValidationFilter<ChatRequest>))]
    public async Task<IActionResult> Post(ChatRequest request)
    {
        var response = await _chatService.SendChatMessageAsync(request);
        return Ok(response);
    }

    [HttpPost("get-chat-messages")]
    [ServiceFilter(typeof(ValidationFilter<ChatMessageDto>))]
    public async Task<IActionResult> Post(ChatMessageDto request)
    {
        var response = await _chatService.GetChatMessages(request);
        return Ok(response);
    }
}