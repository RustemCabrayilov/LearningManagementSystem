using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Application.Abstractions.Services.Term;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

    [Area("Admin")]
public class TermsController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    public async Task<IActionResult> Index([FromQuery]RequestFilter? filter)
    {
        var response = await _learningManagementSystem.TermList(filter);
        int totalTerms = _learningManagementSystem.TermList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalTerms / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }
    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await _learningManagementSystem.GetTerm(id);
        ViewBag.Terms = new List<string>();
        foreach (var term in Enum.GetNames(typeof(TermSeason)))
        {
            ViewBag.Terms.Add(term);
        }
        var model = new TermRequest(response.Name,response.StartDate, response.EndDate,false,response.TermSeason);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute]Guid id,TermRequest request)
    {
        try
        {
            var response = await _learningManagementSystem.UpdateTerm(id, request);
        }
        catch (ValidationApiException e)
        {
            Console.WriteLine(e.Content);
            throw;
        }
        return RedirectToAction("Index");
    }
    public IActionResult Create()
    {
        ViewBag.Terms = new List<string>();
        foreach (var term in Enum.GetNames(typeof(TermSeason)))
        {
            ViewBag.Terms.Add(term);
        }
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(TermRequest request)
    {
        try
        {
            var response = await _learningManagementSystem.CreateTerm(request);
        }
        catch (ValidationApiException e)
        {
            Console.WriteLine(e.Content);
            throw;
        }
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _learningManagementSystem.RemoveFaculty(id);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> ActivateTerm(Guid id)
    {
        var response = await _learningManagementSystem.ActivateTerm(id);
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Details([FromQuery]Guid id)
    {
        var response = await _learningManagementSystem.GetTerm(id);
        return Json(response);
    }
}