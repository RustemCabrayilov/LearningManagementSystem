using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class TeachersController(ILearningManagementSystem _learningManagementSystem ) : Controller
{
    public async Task<IActionResult> Details(string userId)
    {
        var teachers=await _learningManagementSystem.TeacherList(new RequestFilter{FilterField = "AppUserId",FilterValue = userId});
        var teacher=teachers.FirstOrDefault();
        ViewBag.User=await _learningManagementSystem.GetUser(userId);
        return View(teacher);
    }
}