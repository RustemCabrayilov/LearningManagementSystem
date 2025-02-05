using LearningManagementSystem.Application.Abstractions.Services.Lesson;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class LessonsController(
    ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
    public async Task<IActionResult> Index([FromQuery] RequestFilter? filter)
    {
        var response = await _learningManagementSystem.LessonList(filter);
        int totalLessons = _learningManagementSystem.LessonList(new RequestFilter() { AllUsers = true }).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalLessons / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await _learningManagementSystem.GetLesson(id);
        var model = new LessonRequest(response.Group.Id);
        var groups = await _learningManagementSystem.GroupList(new RequestFilter() { AllUsers = true });
        ViewBag.Groups = groups;
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, LessonRequest request)
    {
        try
        {
            var response = await _learningManagementSystem.UpdateLesson(id, request);
            return RedirectToAction("Index", "Home");
        }
        catch (ValidationApiException e)
        {
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().Value.FirstOrDefault());
            return RedirectToAction("Create");
        }
        catch (ApiException e)
        {
            var errorContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Content);
            if (errorContent != null && errorContent.ContainsKey("detail"))
            {
                var errorMessage = errorContent["detail"];
                _toastNotification.AddErrorToastMessage(errorMessage);
            }

            _toastNotification.AddErrorToastMessage(e.Message);
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
        }

        return RedirectToAction("Create");
    }


    public async Task<IActionResult> Create()
    {
        var groups = await _learningManagementSystem.GroupList(new RequestFilter() { AllUsers = true });
        ViewBag.Groups = groups;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(LessonRequest request)
    {
        try
        {
            var response = await _learningManagementSystem.CreateLesson(request);
            return RedirectToAction("Index", "Lessons");
        }
        catch (ValidationApiException e)
        {
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().Value.FirstOrDefault());
            return RedirectToAction("Create");
        }
        catch (ApiException e)
        {
            var errorContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Content);
            if (errorContent != null && errorContent.ContainsKey("detail"))
            {
                var errorMessage = errorContent["detail"];
                _toastNotification.AddErrorToastMessage(errorMessage);
            }

            _toastNotification.AddErrorToastMessage(e.Message);
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
        }

        return RedirectToAction("Create");
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _learningManagementSystem.RemoveLesson(id);
        }
        catch (ApiException e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage("Something Went wrong");
        }

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details([FromQuery] Guid id)
    {
        var response = await _learningManagementSystem.GetLesson(id);
        return Json(response);
    }
}