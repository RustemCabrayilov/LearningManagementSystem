using LearningManagementSystem.Application.Abstractions.Services.Attendance;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Controllers;

public class ExamsController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    public async Task<IActionResult> Index([FromQuery]RequestFilter filter)
    {
        var responses = await _learningManagementSystem.ExamList(filter);
        return View(responses);
    }
    public IActionResult Edit()
    {
        return View();
    }
    [HttpPut]
    public async Task<IActionResult> Edit(AttendanceRequest[] requests)
    {
        var response=await _learningManagementSystem.UpdateAttendanceList(requests);
        return View();
    }
}