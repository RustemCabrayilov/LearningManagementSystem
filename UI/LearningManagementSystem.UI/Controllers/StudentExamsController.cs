using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class StudentExamsController(
    ILearningManagementSystem _learningManagementSystem) : Controller
{
    // GET
    public async Task<IActionResult> Index(Guid studentId)
    {
        var student =await _learningManagementSystem.GetStudent(studentId);
        ViewBag.Groups = student.Groups;
        var response= student.StudentExams;
        return View(response);
    }
}