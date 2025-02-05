using LearningManagementSystem.Application.Abstractions.Services.Survey;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;
[Area("Admin")]
public class SurveysController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    public async Task<IActionResult> Index([FromQuery]RequestFilter? filter)
    {
        var response = await _learningManagementSystem.SurveyList(filter);
        int totalSurveys = _learningManagementSystem.SurveyList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalSurveys / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }
    public async Task<IActionResult> Edit(Guid id)
    {
        ViewBag.Terms = await _learningManagementSystem.TermList(new (){AllUsers=true});
        ViewBag.Teachers = await _learningManagementSystem.TeacherList(new (){AllUsers=true});
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Edit(Guid id,SurveyRequest request)
    {
        var response = await _learningManagementSystem.UpdateSurvey(id,request);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Terms = await _learningManagementSystem.TermList(null);
        ViewBag.Teachers = await _learningManagementSystem.TeacherList(null);
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(SurveyRequest request)
    {
        try
        {
            var response = await _learningManagementSystem.CreateSurvey(request);
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
        await _learningManagementSystem.RemoveSurvey(id);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> ActivateSurvey(Guid id)
    {
        var response = await _learningManagementSystem.ActivateSurvey(id);
        return RedirectToAction("Index");
    }
    
}