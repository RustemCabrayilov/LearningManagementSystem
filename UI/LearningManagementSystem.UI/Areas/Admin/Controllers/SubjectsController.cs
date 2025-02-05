using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Application.Abstractions.Services.Subject;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;
[Area("Admin")]
public class SubjectsController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    public async Task<IActionResult> Index([FromQuery]RequestFilter? filter)
    {
        var response = await _learningManagementSystem.SubjectList(filter);
        int totalSubjects = _learningManagementSystem.SubjectList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalSubjects / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }
    public async Task<IActionResult> Edit(Guid id)
    {
        var response =await _learningManagementSystem.GetSubject(id);
        var model = new SubjectRequest(response.Name, response.SubjectCode, response.AttendanceLimit);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute]Guid id,SubjectRequest request)
    {
        var response = await _learningManagementSystem.UpdateSubject(id,request);
        return RedirectToAction("Index");
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(SubjectRequest request)
    {
        var response = await _learningManagementSystem.CreateSubject(request);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _learningManagementSystem.RemoveSubject(id);
        return RedirectToAction("Index");
    }
}