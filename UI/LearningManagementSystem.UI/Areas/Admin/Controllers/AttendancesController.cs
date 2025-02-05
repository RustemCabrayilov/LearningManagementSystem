using LearningManagementSystem.Application.Abstractions.Services.Attendance;
using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;
[Area("admin")]
public class AttendancesController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    public async Task<IActionResult> Index([FromQuery]RequestFilter? filter)
    {
        var response = await _learningManagementSystem.AttendanceList(filter);
        int totalAttendance = _learningManagementSystem.AttendanceList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalAttendance / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }
    public async Task<IActionResult> Edit(Guid id)
    {
        var model = await _learningManagementSystem.GetAttendance(id);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit(Guid id,AttendanceRequest request)
    {
        var response = await _learningManagementSystem.UpdateAttendance(id,request);
        return RedirectToAction("Index");
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(AttendanceRequest request)
    {
        var response = await _learningManagementSystem.CreateAttendance(request);
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _learningManagementSystem.RemoveAttendance(id);
        return RedirectToAction("Index");
    }
}