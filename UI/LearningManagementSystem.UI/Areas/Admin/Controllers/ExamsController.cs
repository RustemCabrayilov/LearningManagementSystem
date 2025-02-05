using LearningManagementSystem.Application.Abstractions.Services.Exam;

using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Extensions;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;
[Area("Admin")]
public class ExamsController(ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
   
    public async Task<IActionResult> Index([FromQuery]RequestFilter? filter)
    {
        var response = await _learningManagementSystem.ExamList(filter);
        int totalExams = _learningManagementSystem.ExamList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalExams / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }
    public async Task<IActionResult> Edit(Guid id)
    {
        ViewBag.ExamTypeList = new List<string>();
        foreach (var term in Enum.GetNames(typeof(ExamType)))
        {
            ViewBag.ExamTypeList.Add(term);
        }
        ViewBag.Groups = await _learningManagementSystem.GroupList(null);
        var response = await _learningManagementSystem.GetExam(id);
        var model = new ExamRequest(response.MaxPoint, response.ExamType, response.Group.Id, response.StartDate,response.EndDate);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute]Guid id,ExamRequest request)
    {
        try
        {
            var response = await _learningManagementSystem.UpdateExam(id,request);

            return RedirectToAction("Index");
        }
        catch (ValidationApiException e)
        {
            ModelState.AddValidationError(e);
            ViewBag.ExamTypeList = new List<string>();
            foreach (var term in Enum.GetNames(typeof(ExamType)))
            {
                ViewBag.ExamTypeList.Add(term);
            }
            ViewBag.Groups = await _learningManagementSystem.GroupList(null);
            return View();
        }
        catch (Exception e)
        {
            _toastNotification.AddAlertToastMessage(e.Message);
            ViewBag.ExamTypeList = new List<string>();
            foreach (var term in Enum.GetNames(typeof(ExamType)))
            {
                ViewBag.ExamTypeList.Add(term);
            }

            ViewBag.Groups = await _learningManagementSystem.GroupList(null);
            return View();
        }
    }
    
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _learningManagementSystem.RemoveExam(id);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.ExamTypeList = new List<string>();
        foreach (var term in Enum.GetNames(typeof(ExamType)))
        {
            ViewBag.ExamTypeList.Add(term);
        }

        ViewBag.Groups = await _learningManagementSystem.GroupList(null);
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(ExamRequest request)
    {
        
        try
        {
            var response = await _learningManagementSystem.CreateExam(request);
            return RedirectToAction("Index");
        }
        catch (ValidationApiException e)
        {
            ModelState.AddValidationError(e);
            ViewBag.ExamTypeList = new List<string>();
            foreach (var term in Enum.GetNames(typeof(ExamType)))
            {
                ViewBag.ExamTypeList.Add(term);
            }

            ViewBag.Groups = await _learningManagementSystem.GroupList(null);
            return View();
        }
        catch (Exception e)
        {
            _toastNotification.AddAlertToastMessage(e.Message);
            ViewBag.ExamTypeList = new List<string>();
            foreach (var term in Enum.GetNames(typeof(ExamType)))
            {
                ViewBag.ExamTypeList.Add(term);
            }

            ViewBag.Groups = await _learningManagementSystem.GroupList(null);
            return View();
        }
    }
    public async Task<IActionResult> Details(Guid id)
    {
        var response = await _learningManagementSystem.GetExam(id);
        return Json(new
            {
                exam = response,
            }
        );
    }
}