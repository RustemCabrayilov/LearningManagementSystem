using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class StudentGroupsController(ILearningManagementSystem _learningManagementSystem,
    IHttpContextAccessor _httpContextAccessor) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var token = _httpContextAccessor?.HttpContext?.Request.Cookies["access_token"];
        var userclaim = await _learningManagementSystem.GetUserInfosByToken(token);
        var students = await _learningManagementSystem.StudentList(new RequestFilter()
            { FilterField = "AppUserId", FilterValue = userclaim.Id });
        var student = students.FirstOrDefault();
        var response = await _learningManagementSystem.GetStudent(student.Id);
        ViewBag.Groups = response.Groups;
        return View();
    }
}