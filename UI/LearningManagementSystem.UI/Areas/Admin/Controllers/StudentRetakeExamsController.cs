using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Extensions;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class StudentRetakeExamsController(
    ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
    // GET
    public async Task<IActionResult> Index([FromQuery] RequestFilter? filter)
    {
        var response = await _learningManagementSystem.StudentRetakeExamList(filter);
        int totalExams = _learningManagementSystem.StudentRetakeExamList(new RequestFilter() { AllUsers = true }).Result
            .Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalExams / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await _learningManagementSystem.GetStudentRetakeExam(id);
        var model = new StudentRetakeExamDto(response.Student.Id, response.RetakeExam.Id, response.Status,
            response.NewPoint);
        ViewBag.StatusList = new List<string>();
        foreach (var term in Enum.GetNames(typeof(Status)))
        {
            ViewBag.StatusList.Add(term);
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] Guid id, StudentRetakeExamDto request)
    {
        try
        {
            var response = await _learningManagementSystem.UpdateStudentRetakeExam(id, request);
            return RedirectToAction("Index");
        }
        catch (ValidationApiException e)
        {
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().Value.FirstOrDefault());
        }
        catch (ApiException e)
        {
            var errorContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Content);
            if (errorContent != null && errorContent.ContainsKey("detail"))
            {
                var errorMessage = errorContent["detail"];
                _toastNotification.AddErrorToastMessage(errorMessage);
            }

            return RedirectToAction("Index", "Home");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage("Something Went Wrong");
        }

        return RedirectToAction("Edit");
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.StatusList = new List<string>();
        foreach (var term in Enum.GetNames(typeof(Status)))
        {
            ViewBag.StatusList.Add(term);
        }

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(StudentRetakeExamDto request)
    {
        try
        {
            var response = await _learningManagementSystem.CreateStudentRetakeExam(request);
            return RedirectToAction("Index");
        }
        catch (ValidationApiException e)
        {
            ModelState.AddValidationError(e);
        }
        catch (Exception e)
        {
            _toastNotification.AddAlertToastMessage(e.Message);
        }

        return RedirectToAction("Create");
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _learningManagementSystem.RemoveStudentRetakeExam(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> RetakeExamRequests(RequestFilter? filter)
    {
        var responses = await _learningManagementSystem.ActiveTermRequests(filter);
        int totalRequests = _learningManagementSystem.StudentRetakeExamList(new RequestFilter() { AllUsers = true })
            .Result
            .Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalRequests / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(responses);
    }

    public async Task<IActionResult> Details([FromQuery] Guid id)
    {
        try
        {
            var response = await _learningManagementSystem.GetStudentRetakeExam(id);
            return Json(new
            {
                response = response,
                retakeExamType = response.RetakeExam.RetakeExamType.ToString(),
            });
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage("Something Went Wrong");
        }

        return Json(null);
    }
}