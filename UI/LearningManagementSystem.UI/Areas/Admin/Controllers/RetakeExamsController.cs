using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Extensions;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
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
        var model = new RetakeExamRequest(response.Exam.Id, response.Deadline, response.RetakeExamType, response.Price,
            null);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] Guid id, RetakeExamRequest request)
    {
        try
        {
            var streamParts = new List<StreamPart>();
            foreach (var file in request.Files)
            {
                var fileName = Path.GetFileName(file.FileName);
                // Use ASP.NET Core to get MIME type
                var provider = new FileExtensionContentTypeProvider();
                provider.TryGetContentType(fileName, out var mimeType);

                if (mimeType == null)
                {
                    // If MIME type could not be determined, set a default type
                    mimeType = "application/octet-stream";
                }

                var fileStream = file.OpenReadStream();
                streamParts.Add(new StreamPart(fileStream, fileName, mimeType, "file"));
            }

            var response = await _learningManagementSystem.UpdateRetakeExam(
                id,
                request.ExamId,
                request.Deadline, 
                request.RetakeExamType,
                request.Price,
                streamParts);
            _toastNotification.AddSuccessToastMessage("RetakeExam Updated Successfully");
            return RedirectToAction("Index");
        }
        catch (ValidationApiException e)
        {
            ModelState.AddValidationError(e);
            ViewBag.ReTakeExamTypeList = new List<string>();
            foreach (var term in Enum.GetNames(typeof(RetakeExamType)))
            {
                ViewBag.ReTakeExamTypeList.Add(term);
            }

            ViewBag.Exams = await _learningManagementSystem.ExamList(null);

            return View();
        }
        catch (Exception e)
        {
            _toastNotification.AddAlertToastMessage(e.Message);
            ViewBag.ReTakeExamTypeList = new List<string>();
            foreach (var term in Enum.GetNames(typeof(RetakeExamType)))
            {
                ViewBag.ReTakeExamTypeList.Add(term);
            }

            ViewBag.Exams = await _learningManagementSystem.ExamList(null);

            return View();
        }
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.ReTakeExamTypeList = new List<string>();
        foreach (var term in Enum.GetNames(typeof(RetakeExamType)))
        {
            ViewBag.ReTakeExamTypeList.Add(term);
        }

        ViewBag.Exams = await _learningManagementSystem.ExamList(null);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(RetakeExamRequest request)
    {
        try
        {
            var streamParts = new List<StreamPart>();
            foreach (var file in request.Files)
            {
                var fileName = Path.GetFileName(file.FileName);
                // Use ASP.NET Core to get MIME type
                var provider = new FileExtensionContentTypeProvider();
                provider.TryGetContentType(fileName, out var mimeType);

                if (mimeType == null)
                {
                    // If MIME type could not be determined, set a default type
                    mimeType = "application/octet-stream";
                }

                var fileStream = file.OpenReadStream();
                streamParts.Add(new StreamPart(fileStream, fileName, mimeType, "file"));
            }

            var response = await _learningManagementSystem.CreateRetakeExam(
                request.ExamId,
                request.Deadline, 
                request.RetakeExamType,
                request.Price,
                streamParts);
            _toastNotification.AddSuccessToastMessage("RetakeExam Created Successfully");
            return RedirectToAction("Index");
        }
        catch (ValidationApiException e)
        {
            ModelState.AddValidationError(e);
            ViewBag.ReTakeExamTypeList = new List<string>();
            foreach (var term in Enum.GetNames(typeof(RetakeExamType)))
            {
                ViewBag.ReTakeExamTypeList.Add(term);
            }

            ViewBag.Exams = await _learningManagementSystem.ExamList(null);
            return View();
        }
        catch (Exception e)
        {
            _toastNotification.AddAlertToastMessage(e.Message);
            ViewBag.ReTakeExamTypeList = new List<string>();
            foreach (var term in Enum.GetNames(typeof(RetakeExamType)))
            {
                ViewBag.ReTakeExamTypeList.Add(term);
            }

            ViewBag.Exams = await _learningManagementSystem.ExamList(null);
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _learningManagementSystem.RemoveRetakeExam(id);
        return RedirectToAction("Index");
    }
}