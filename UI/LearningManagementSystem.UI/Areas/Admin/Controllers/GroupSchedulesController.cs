using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Application.Abstractions.Services.GroupSchedule;
using LearningManagementSystem.Application.Helper;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;
[Area("Admin")]
public class GroupSchedulesController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    public async Task<IActionResult> Index([FromQuery] RequestFilter? filter)
    {
        var response = await _learningManagementSystem.GroupScheduleList(filter);
        int totalGroupSchedules = _learningManagementSystem.GroupScheduleList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalGroupSchedules / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await _learningManagementSystem.GetGroupSchedule(id);
        var model = new GroupScheduleRequest(response.Group.Id,response.ClassTime,
            response.DayOfWeek);
        ViewBag.TimeList = TimeHelper.TimeList;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute]Guid id, GroupScheduleRequest request)
    {
        var response = await _learningManagementSystem.UpdateGroupSchedule(id, request);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _learningManagementSystem.RemoveGroupSchedule(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.DayList = new List<string>();
        foreach (var day in Enum.GetNames(typeof(DayOfWeek)))
        {
            ViewBag.DayList.Add(day);
        }
        ViewBag.Groups = await _learningManagementSystem.GroupList(null);
        ViewBag.TimeList = TimeHelper.TimeList;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupScheduleRequest request)
    {
        var response = await _learningManagementSystem.CreateGroupSchedule(request);
        return RedirectToAction("Index");
    }
}