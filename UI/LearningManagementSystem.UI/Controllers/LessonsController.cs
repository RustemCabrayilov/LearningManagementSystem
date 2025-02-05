using LearningManagementSystem.Application.Abstractions.Services.Lesson;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Controllers;

public class LessonsController(ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
    // GET
    public async Task<IActionResult> Index()
    {
        var response = await _learningManagementSystem.LessonList(new()
        {
            AllUsers = true
        });
        return View(response);
    }

    public async Task<IActionResult> Create()
    {
        var groups =await _learningManagementSystem.GroupList(new RequestFilter() { AllUsers = true });
        ViewBag.Groups = groups;
        return View();
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(LessonRequest request)
    {
        try
        {
            var response = await _learningManagementSystem.CreateLesson(request);
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
}