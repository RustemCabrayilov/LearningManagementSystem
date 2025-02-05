using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Application.Abstractions.Services.Question;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;
[Area("Admin")]
public class QuestionsController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    public async Task<IActionResult> Index([FromQuery] RequestFilter? filter)
    {
        var response = await _learningManagementSystem.QuestionList(filter);
        int totalQuestions = _learningManagementSystem.QuestionList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalQuestions / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        ViewBag.Surveys  = await _learningManagementSystem.SurveyList(new RequestFilter(){AllUsers = true});
        return View();  
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] Guid id, QuestionRequest request)
    {
        var response = await _learningManagementSystem.UpdateQuestion(id, request);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Create()
    {
        var surveys = await _learningManagementSystem.SurveyList(new RequestFilter(){AllUsers = true});
        ViewBag.Surveys = surveys; 
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(QuestionRequest request)
    {
        try
        {
            var response = await _learningManagementSystem.CreateQuestion(request);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _learningManagementSystem.RemoveQuestion(id);
        return RedirectToAction("Index");
    }
}