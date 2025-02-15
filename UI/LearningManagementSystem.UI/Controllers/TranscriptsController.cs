using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class TranscriptsController(ILearningManagementSystem _learningManagementSystem
 ) : Controller
{
    // GET
    public async Task<IActionResult> Index(Guid studentId)
    {
        var student = await _learningManagementSystem.GetStudent(studentId);
        var response = student?.Transcripts;
        return View(response);
    }
}