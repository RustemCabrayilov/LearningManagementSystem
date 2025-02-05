using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class RetakeExamsController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var response = await _learningManagementSystem.RetakeExamList(new() { AllUsers = true });
        return View(response);
    }
}