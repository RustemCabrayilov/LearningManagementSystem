using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class DeansController(ILearningManagementSystem _learningManagementSystem,
    IHttpContextAccessor _httpContextAccessor) : Controller
{
    public async Task<IActionResult> Details(string userId)
    {
        var deans=await _learningManagementSystem.DeanList(new RequestFilter{FilterField = "AppUserId",FilterValue = userId});
        var dean=deans.FirstOrDefault();
        ViewBag.User=await _learningManagementSystem.GetUser(userId);
        return View(dean);
    }
}