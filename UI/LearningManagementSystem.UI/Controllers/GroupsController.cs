using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class GroupsController(ILearningManagementSystem _learningManagementSystem,
    IHttpContextAccessor _httpContextAccessor) : Controller
{
    // GET
    public async Task<IActionResult> Get(Guid groupId)
    {
        var response = await _learningManagementSystem.GetGroup(groupId);

        // Return the schedule data in a format that can be easily consumed by JavaScript
        return Json(response);
    }

    public async Task<IActionResult> Index([FromQuery]RequestFilter filter)
    {
        var token = _httpContextAccessor.HttpContext.Request.Cookies["access_token"];
        var userclaim = await _learningManagementSystem.GetUserInfosByToken(token);
        var teachers = await _learningManagementSystem.TeacherList(new()
        {
            FilterField = "AppUserId",
            FilterValue = userclaim.Id
        });
        var teacher=teachers.FirstOrDefault();
        var groups = await _learningManagementSystem.GroupList(new RequestFilter()
        {
            FilterField = "TeacherId",
            FilterGuidValue = teacher?.Id
        });
        return View(groups);
    }
}