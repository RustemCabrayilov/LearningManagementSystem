using AutoMapper;
using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using NuGet.Packaging;
using Refit;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

    [Area("admin")]
public class MajorsController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    public async Task<IActionResult> Index([FromQuery]RequestFilter? filter)
    {
        var response = await _learningManagementSystem.MajorList(filter);
        int totalMajors = _learningManagementSystem.MajorList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalMajors / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }
    public async Task<IActionResult> Edit(Guid id)
    {
        ViewBag.Faculties = await _learningManagementSystem.FacultyList(null);
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute]Guid id,MajorRequest request)
    {
        var response = await _learningManagementSystem.UpdateMajor(id,request);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Faculties = await _learningManagementSystem.FacultyList(null);
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(MajorRequest request)
    {
        try
        {
            var response = await _learningManagementSystem.CreateMajor(request);
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
        await _learningManagementSystem.RemoveMajor(id);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Details(Guid id)
    {
        var response = await _learningManagementSystem.GetMajor(id);
        return Json(response);
    }
}