using System.Text.RegularExpressions;
using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

[Area("admin")]
public class GroupsController(ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
    public async Task<IActionResult> Index([FromQuery] RequestFilter? filter)
    {
        var response = await _learningManagementSystem.GroupList(filter);
        int totalGroups = _learningManagementSystem.GroupList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalGroups / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        ViewBag.DayList = new List<string>();
        foreach (var day in Enum.GetNames(typeof(DayOfWeek)))
        {
            ViewBag.DayList.Add(day);
        }

        ViewBag.Teachers = await _learningManagementSystem.TeacherList(new ()
        {
            AllUsers = true
        });
        ViewBag.Subjects = await _learningManagementSystem.SubjectList(new ()
        {
            AllUsers = true
        });
        ViewBag.Majors = await _learningManagementSystem.MajorList(new ()
        {
            AllUsers = true
        });
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, GroupRequest request)
    {
        try
        {
            var response = await _learningManagementSystem.UpdateGroup(id, request);
        }
        catch (ValidationApiException e)
        {
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().Value.ToString());
            return RedirectToAction("Edit",new {id = id});
        }
        catch (ApiException e)
        {
            var errorContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Content);
            if (errorContent != null && errorContent.ContainsKey("detail"))
            {
                var errorMessage = errorContent["detail"];
                _toastNotification.AddErrorToastMessage(errorMessage);
            }
            return RedirectToAction("Edit",new {id = id});
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage("An unexpected error occured.");
            return RedirectToAction("Edit",new {id = id});
        }
        
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.DayList = new List<string>();
        foreach (var day in Enum.GetNames(typeof(DayOfWeek)))
        {
            ViewBag.DayList.Add(day);
        }

        ViewBag.Teachers = await _learningManagementSystem.TeacherList(new ()
        {
            AllUsers = true
        });
        ViewBag.Subjects = await _learningManagementSystem.SubjectList(new ()
        {
            AllUsers = true
        });
        ViewBag.Majors = await _learningManagementSystem.MajorList(new ()
        {
            AllUsers = true
        });
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(GroupRequest request)
    {
        var response = await _learningManagementSystem.CreateGroup(request);
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _learningManagementSystem.RemoveGroup(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> ActivateGroup(Guid id)
    {
     var response = await _learningManagementSystem.ActivateGroup(id);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Details(Guid id)
    {
        try
        {
            var response = await _learningManagementSystem.GetGroup(id);
            return Json(response);
        }
        catch (ApiException apiEx)
        {
            // Log or handle the exception
            _toastNotification.AddErrorToastMessage("API error occurred: " + apiEx.Content);
            return StatusCode(500, "Internal Server Error");
        }
        catch (Exception ex)
        {
            _toastNotification.AddErrorToastMessage("Unexpected error occurred: " + ex.Message);
            return StatusCode(500, "Internal Server Error");
        }
    }
}