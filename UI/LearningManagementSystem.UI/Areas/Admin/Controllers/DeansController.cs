using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Text.Json;
using LearningManagementSystem.Application.Abstractions.Services.Dean;
using LearningManagementSystem.Application.Exceptions;
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

[Area("Admin")]
public class DeansController(
    ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
    public async Task<IActionResult> Index([FromQuery] RequestFilter? filter)
    {
        try
        {
            var responses = await _learningManagementSystem.DeanList(filter);
            int totalDeans = _learningManagementSystem.DeanList(new RequestFilter() { AllUsers = true }).Result.Count;
            ViewBag.TotalPages = (int)Math.Ceiling(totalDeans / (double)filter.Count);
            ViewBag.CurrentPage = filter.Page;
            return View(responses);
        }
        catch (ApiException e)
        {
            var errorContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Content);
            if (errorContent != null && errorContent.ContainsKey("detail"))
            {
                var errorMessage = errorContent["detail"];
                _toastNotification.AddErrorToastMessage(errorMessage);
            }

            return RedirectToAction("Index","Home");
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
            return RedirectToAction("Index", "Home");
        }
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        ViewBag.Faculties = await _learningManagementSystem
            .FacultyList(null);
        ViewBag.Users = await _learningManagementSystem
            .UserList(null);
        ViewBag.PositionTypeList = Enum.GetValues(typeof(PositionType))
            .Cast<PositionType>()
            .Select(pt => new { Value = (int)pt, Text = pt.ToString() })
            .ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Guid id, DeanRequest request)
    {
        if (ModelState.IsValid)
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

                var response = await _learningManagementSystem.UpdateDean(id,
                    request.Name,
                    request.Surname,
                    request.Salary,
                    (int)request.PositionType,
                    request.FacultyId,
                    request.AppUserId,
                    streamPart);
                Console.WriteLine("UploadFileInModel_WithRefitAsync response :" + response);
                var document = await _learningManagementSystem.GetByOwner(response.Id);
            }
            catch (ValidationApiException ex)
            {
                ModelState.AddValidationError(ex);
            }
            catch (ApiException e)
            {
                var errorContent = JsonConvert.DeserializeObject<Dictionary<string, string>>(e.Content);
                if (errorContent != null && errorContent.ContainsKey("detail"))
                {
                    var errorMessage = errorContent["detail"];
                    _toastNotification.AddErrorToastMessage(errorMessage);
                }

                return RedirectToAction("Index","Home");
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage(e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        ViewBag.Faculties = await _learningManagementSystem.FacultyList(null);
        ViewBag.Users = await _learningManagementSystem.UserList(null);
        ViewBag.PositionTypeList = Enum.GetValues(typeof(PositionType))
            .Cast<PositionType>()
            .Select(pt => new { Value = (int)pt, Text = pt.ToString() })
            .ToList();

        return View(request);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _learningManagementSystem.RemoveDean(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Faculties = await _learningManagementSystem
            .FacultyList(null);
        ViewBag.Users = await _learningManagementSystem
            .UserList(null);
        ViewBag.PositionTypeList = Enum.GetValues(typeof(PositionType))
            .Cast<PositionType>()
            .Select(pt => new { Value = (int)pt, Text = pt.ToString() })
            .ToList();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(DeanRequest request)
    {
        if (ModelState.IsValid)
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

                        // Now send the request to the backend API
                        var response = await _learningManagementSystem.CreateDean(
                            request.Name,
                            request.Surname,
                            request.Salary,
                            (int)request.PositionType,
                            request.FacultyId,
                            request.AppUserId,
                            streamPart
                        );
                        // Handle response
                        Console.WriteLine("UploadFileInModel_WithRefitAsync response: " + response);
                        var document = await _learningManagementSystem.GetByOwner(response.Id);

                        return RedirectToAction("Index");
                
            
            }
            catch (ValidationApiException ex)
            {
                ModelState.AddValidationError(ex);
            }
            catch (Exception e)
            {
                _toastNotification.AddErrorToastMessage(e.Message);
            }
        }

        ViewBag.Faculties = await _learningManagementSystem.FacultyList(null);
        ViewBag.Users = await _learningManagementSystem.UserList(null);
        ViewBag.PositionTypeList = Enum.GetValues(typeof(PositionType))
            .Cast<PositionType>()
            .Select(pt => new { Value = (int)pt, Text = pt.ToString() })
            .ToList();

        return View(request);
    }
    public async Task<IActionResult> Details(Guid id)
    {
        var response = await _learningManagementSystem.GetTeacher(id);
        var document = await _learningManagementSystem.GetByOwner(id);
        return Json(new
            {
                student = response,
                fileUrl = document.Path,
                fileName=document.FileName
            }
        );
    }


}