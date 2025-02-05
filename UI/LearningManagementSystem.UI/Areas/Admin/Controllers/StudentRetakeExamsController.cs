using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Extensions;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

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
        var model = new StudentRetakeExamDto(response.Student.Id, response.RetakeExam.Id, response.Status,response.NewPoint);
        ViewBag.StatusList = new List<string>();
        foreach (var term in Enum.GetNames(typeof(Status)))
        {
            ViewBag.StatusList.Add(term);
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute]Guid id,StudentRetakeExamDto request)
    {
        try
        {
            var response = await _learningManagementSystem.UpdateStudentRetakeExam(id,request);
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
        ViewBag.StatusList = new List<string>();
        foreach (var term in Enum.GetNames(typeof(Status)))
        {
            ViewBag.StatusList.Add(term);
        }

        return View(request);
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
        ViewBag.StatusList = new List<string>();
        foreach (var term in Enum.GetNames(typeof(Status)))
        {
            ViewBag.StatusList.Add(term);
        }

        return View(request);
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _learningManagementSystem.RemoveStudentRetakeExam(id);
        return RedirectToAction("Index");
    }
}