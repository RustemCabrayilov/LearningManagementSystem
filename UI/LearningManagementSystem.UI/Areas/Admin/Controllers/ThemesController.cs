using LearningManagementSystem.Application.Abstractions.Services.Term;
using LearningManagementSystem.Application.Abstractions.Services.Theme;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;
[Area("Admin")]
public class ThemesController(ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
  public async Task<IActionResult> Index([FromQuery]RequestFilter? filter)
    {
        var response = await _learningManagementSystem.ThemeList(filter);
        int totalTerms = _learningManagementSystem.TermList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalTerms / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }
    public async Task<IActionResult> Edit(Guid id)
    {
        var response = await _learningManagementSystem.GetTheme(id);
        var model = new ThemeRequest(response.Title, null);
        return View(model);
    }
    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute]Guid id,ThemeRequest request)
    {
        try
        {
            var fileName = Path.GetFileName(request.File.FileName);
            var provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(fileName, out var mimeType);
            if (mimeType == null)
            {
                mimeType = "application/octet-stream";
            }
            var fileStream = request.File.OpenReadStream();
            var streamPart = new StreamPart(fileStream, fileName, mimeType, "file");
            var response = await _learningManagementSystem.UpdateTheme(id, request.Title,streamPart);
        }
        catch (ValidationApiException e)
        {
            Console.WriteLine(e.Content);
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().ToString());
            return RedirectToAction("Edit", new { id = id });
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage("Something went wrong");
        }
        return RedirectToAction("Index");
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Create(ThemeRequest request)
    {
        try
        {
            var fileName = Path.GetFileName(request.File.FileName);

            // Use ASP.NET Core to get MIME type
            var provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(fileName, out var mimeType);

            if (mimeType == null)
            {
                // If MIME type could not be determined, set a default type
                mimeType = "application/octet-stream";
            }

            // Open the file as a stream within a using block to ensure proper disposal
            var fileStream = request.File.OpenReadStream();
            // Convert the stream to a MemoryStream (to handle issues with ReadTimeout, WriteTimeout)
                  

            // Create StreamPart with file stream and MIME type
            var streamPart = new StreamPart(fileStream, fileName, mimeType, "file");
            await _learningManagementSystem.CreateTheme(request.Title, streamPart);
        }
        catch (ValidationApiException e)
        {
            Console.WriteLine(e.Content);
            _toastNotification.AddErrorToastMessage(e?.Content?.Errors.FirstOrDefault().ToString());
            return RedirectToAction("Create");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage("Something went wrong");
        }
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _learningManagementSystem.RemoveTheme(id);
        return RedirectToAction("Index");
    }
    public async Task<IActionResult> ActivateTheme(Guid id)
    {
        var response = await _learningManagementSystem.ActivateTheme(id);
        return RedirectToAction("Index");
    }
}