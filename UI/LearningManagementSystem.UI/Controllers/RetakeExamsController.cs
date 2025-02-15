using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class RetakeExamsController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var response = await _learningManagementSystem.RetakeExamList(new() { AllUsers = true });
        return View(response);
    }
    public async Task<IActionResult> Details(Guid id)
    {
        var response = await _learningManagementSystem.GetRetakeExam(id);
        string retakeExamType = response.RetakeExamType.ToString();
        string examType = response.Exam.ExamType.ToString();
        return Json(
            new
            {
                retakeExam=response,
                retakeExamType=retakeExamType,
                examType=examType
            });
    }
}