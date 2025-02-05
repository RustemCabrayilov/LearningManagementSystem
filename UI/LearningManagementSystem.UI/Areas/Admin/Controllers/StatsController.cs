using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

public class StatsController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}