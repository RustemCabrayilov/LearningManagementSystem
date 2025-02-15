using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;
[Area("admin")]
public class FacultiesController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    public async Task<IActionResult> Index([FromQuery]RequestFilter? filter)
    {
        var response = await _learningManagementSystem.FacultyList(filter);
        int totalFaculty = _learningManagementSystem.FacultyList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalFaculty / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }
    public async Task<IActionResult> Edit(Guid id)
    {
        var model = await _learningManagementSystem.GetFaculty(id);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(Guid id,FacultyRequest request)
    {
        var response = await _learningManagementSystem.UpdateFaculty(id,request);
        return RedirectToAction("Index");
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
         await _learningManagementSystem.RemoveFaculty(id);
        return RedirectToAction("Index");
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(FacultyRequest request)
    {
        var response = await _learningManagementSystem.CreateFaculty(request);
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Details(Guid id)
    {
        var response = await _learningManagementSystem.GetFaculty(id);
        return Json(response);
    }
}