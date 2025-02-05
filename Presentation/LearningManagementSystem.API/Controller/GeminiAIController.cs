using LearningManagementSystem.Application.Abstractions.Services.GeminiAI;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class GeminiAIController(IGeminiAIService geminiAiService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(string prompt)
    {
        var response = await geminiAiService.GenerateTextAsync(prompt);
        return Ok(response);
    }
}