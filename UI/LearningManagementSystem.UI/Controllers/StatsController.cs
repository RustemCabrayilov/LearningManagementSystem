using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class StatsController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}