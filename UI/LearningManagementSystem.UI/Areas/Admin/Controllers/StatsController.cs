using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class StatsController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    [HttpGet]
    public async Task<IActionResult> AverageOfStudents()
    {
        var response = await _learningManagementSystem.AvergaeOfStudents();
        return Json(response);
    }

    [HttpGet]
    public async Task<IActionResult> TopRatedTeachers()
    {
        var response = await _learningManagementSystem.TopRatedTeachers();
        return Json(response);
    }
    
    [HttpGet]
    public async Task<IActionResult> MostEfficientTeachers()
    {
        var response = await _learningManagementSystem.MostEfficientTeachers();
        return Json(response);
    }

    public IActionResult Index()
    {
        return View();
    }
}