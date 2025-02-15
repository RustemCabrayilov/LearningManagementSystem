using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Extensions;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Newtonsoft.Json;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

[Area("admin")]
public class RetakeExamsController(
    ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
    public async Task<IActionResult> Index([FromQuery] RequestFilter? filter)
    {
        var response = await _learningManagementSystem.RetakeExamList(filter);
        int totalRetakeExams = _learningManagementSystem.RetakeExamList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalRetakeExams / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await _learningManagementSystem.GetRetakeExam(id);
        ViewBag.ReTakeExamTypeList = new List<string>();
        foreach (var term in Enum.GetNames(typeof(RetakeExamType)))
        {
            ViewBag.ReTakeExamTypeList.Add(term);
        }

        ViewBag.Exams = await _learningManagementSystem.ExamList(null);
        var model = new RetakeExamRequest(response.Exam.Id, response.Deadline, response.RetakeExamType, response.Price);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] Guid id, RetakeExamRequest request)
    {
        try
        {
            var response = await _learningManagementSystem.UpdateRetakeExam(request);
            return RedirectToAction("Edit",new{id=id});

            return RedirectToAction("Index");
        }
        catch (ValidationApiException e)
        {
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().Value.FirstOrDefault());
            return RedirectToAction("Edit",new{id=id});
        }
        catch (ApiException e)
        {
            var errorContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Content);
            if (errorContent != null && errorContent.ContainsKey("detail"))
            {
                var errorMessage = errorContent["detail"];
                _toastNotification.AddErrorToastMessage(errorMessage);
            }
            return RedirectToAction("Edit",new{id=id});

        }
        
        catch (Exception e)
        {
            _toastNotification.AddAlertToastMessage(e.Message);
            return RedirectToAction("Edit",new{id=id});
        }
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.ReTakeExamTypeList = new List<string>();
        foreach (var term in Enum.GetNames(typeof(RetakeExamType)))
        {
            ViewBag.ReTakeExamTypeList.Add(term);
        }

        ViewBag.Exams = await _learningManagementSystem.ExamList(new(){AllUsers = true});
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RetakeExamRequest request)
    {
        try
        {
            
            var response = await _learningManagementSystem.CreateRetakeExam(request);
            _toastNotification.AddSuccessToastMessage("RetakeExam Created Successfully");
            return RedirectToAction("Index");
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
            return RedirectToAction("Create");
        }
        
        catch (Exception e)
        {
            return RedirectToAction("Create");

        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _learningManagementSystem.RemoveRetakeExam(id);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> Details(Guid id)
    {
        var response = await _learningManagementSystem.GetRetakeExam(id);
       string retakeExamType = response.RetakeExamType.ToString();
       string examType = response.Exam.ExamType.ToString();
        return Json(
            new
            {
                retakeExam=response,
                retakeExamType=retakeExamType,
                examType=examType
            });
    }
}