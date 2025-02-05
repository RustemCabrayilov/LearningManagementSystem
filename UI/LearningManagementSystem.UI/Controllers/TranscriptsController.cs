using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class TranscriptsController(ILearningManagementSystem _learningManagementSystem,
    IHttpContextAccessor _httpContextAccessor) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var token = _httpContextAccessor?.HttpContext?.Request.Cookies["access_token"];
        var userclaim = await _learningManagementSystem.GetUserInfosByToken(token);
        var students = await _learningManagementSystem.StudentList(new()
        {
            FilterField = "AppUserId",
            FilterValue = userclaim.Id
        });
        var student = students.FirstOrDefault();
        var response = await _learningManagementSystem.TranscriptList(new()
        {
            AllUsers = true,
            FilterField = "StudentId",
            FilterValue =$"{student?.Id}"
        });
        return View(response);
    }
}