using LearningManagementSystem.Application.Abstractions.Services.Attendance;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Controllers;

public class AttendancesController(
    ILearningManagementSystem _learningManagementSystem,
    IHttpContextAccessor _httpContextAccessor,
    IToastNotification _toastNotification) : Controller
{
    public async Task<IActionResult> Index(Guid groupId)
    {
        var token = _httpContextAccessor?.HttpContext?.Request.Cookies["access_token"];
        var userclaim = await _learningManagementSystem.GetUserInfosByToken(token);
        var students = await _learningManagementSystem.StudentList(new RequestFilter()
            { FilterField = "AppUserId", FilterValue = userclaim.Id });
        var student = students.FirstOrDefault();
        var response = await _learningManagementSystem.GetStudent(student.Id);
        var attendances = await _learningManagementSystem.GetStudentAttendance(new(response.Id,groupId));
        return View(attendances);
    }
}