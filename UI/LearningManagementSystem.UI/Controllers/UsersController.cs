using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class UsersController(ILearningManagementSystem _learningManagementSystem,
    IHttpContextAccessor _httpContextAccessor) : Controller
{
    // GET
    public async Task<IActionResult> Details([FromQuery]string? userId)
    {
        if (userId == string.Empty)
        {
            var token = _httpContextAccessor?.HttpContext?.Request.Cookies["access_token"];
            var userclaim = await _learningManagementSystem.GetUserInfosByToken(token);
            userId = userclaim.Id;
        }
        var deans = await _learningManagementSystem.DeanList(new RequestFilter
            { AllUsers = true, FilterField = "AppUserId", FilterValue = userId });
        var teachers = await _learningManagementSystem.TeacherList(new RequestFilter
            { AllUsers = true, FilterField = "AppUserId", FilterValue = userId });
        var students = await _learningManagementSystem.StudentList(new RequestFilter
            { AllUsers = true, FilterField = "AppUserId", FilterValue = userId });
        object response;
        if (deans.Count > 0)
        {
            return RedirectToAction("Details","Deans", new { userId=userId });

        }
        if (teachers.Count > 0)
        {
            return RedirectToAction("Details","Teachers", new { userId=userId });
        }

        if (students.Count > 0)
        {
            return RedirectToAction("Details","Students", new { userId=userId });

        }
        var student = await _learningManagementSystem.GetUser(userId);
        return View(student);
    }
}