using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class StudentExamsController(HttpContextAccessor _httpContextAccessor,
    ILearningManagementSystem _learningManagementSystem) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var token = _httpContextAccessor?.HttpContext?.Request.Cookies["access_token"];
        var userclaim = await _learningManagementSystem.GetUserInfosByToken(token);
        var students = await _learningManagementSystem.StudentList(new RequestFilter()
            { FilterField = "AppUserId", FilterValue = userclaim.Id });
        var student = students.FirstOrDefault();
        ViewBag.Groups = student.Groups;
        ViewBag.StudentExams = student.StudentExams;
        return View();
    }
}