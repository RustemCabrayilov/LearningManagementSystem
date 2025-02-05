using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class AIChatController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    public  IActionResult Chat()
    {
        return View();
    }
    public async Task<IActionResult> Ask([FromBody]string prompt)
    {
        var response = await _learningManagementSystem.AskGeminiAI(prompt);
        return Json(response);
    }
}