using LearningManagementSystem.Application.Abstractions.Services.Teacher;
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
public class TeachersController(
    ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
    public async Task<IActionResult> Index([FromQuery] RequestFilter? filter)
    {
        var response = await _learningManagementSystem.TeacherList(filter);
        int totalTeachers = _learningManagementSystem.TeacherList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalTeachers / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(response);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var response = await _learningManagementSystem.GetTeacher(id);
            var model = new TeacherRequest(response.Name, response.Surname, response.Occupation, response.Salary,
                response.AppUser.Id, response.Faculty.Id.ToString(), null);
            ViewBag.Faculties = await _learningManagementSystem
                .FacultyList(null);
            ViewBag.Users = await _learningManagementSystem
                .UserList(null);
            return View(model);
        }
        catch (ApiException e)
        {
            Console.WriteLine(e.Content);
            throw;
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit([FromRoute] Guid id, TeacherRequest request)
    {
        if (ModelState.IsValid)
        {
            try
            {
                /*var tempFilePath = Path.GetTempFileName();
                await using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await request.File.CopyToAsync(stream);
                }

                string filePath = tempFilePath;
                var fileInfo = new FileInfo(filePath);
                FileInfoPart fileInfoPart = new FileInfoPart(fileInfo, fileInfo.Name);*/
                // Convert the stream to a MemoryStream (to handle issues with ReadTimeout, WriteTimeout)
                  

                // Create StreamPart with file stream and MIME type
                var fileName = Path.GetFileName(request.File.FileName);
                // Use ASP.NET Core to get MIME type
                var provider = new FileExtensionContentTypeProvider();
                provider.TryGetContentType(fileName, out var mimeType);

                if (mimeType == null)
                {
                    // If MIME type could not be determined, set a default type
                    mimeType = "application/octet-stream";
                }
                var fileStream = request.File.OpenReadStream();
                var streamPart = new StreamPart(fileStream, fileName, mimeType, "file");

                var response = await _learningManagementSystem.UpdateTeacher(id,
                    request.Name,
                    request.Surname,
                    request.Occupation,
                    request.Salary,
                    request.AppUserId,
                    request.FacultyId,
                    streamPart);
                Console.WriteLine("UploadFileInModel_WithRefitAsync response :" + response);
                var document = await _learningManagementSystem.GetByOwner(response.Id);


                /*var ocrResponse = await _learningManagementSystem.GetTextFromFile(fileInfoPart, document.Id);
                foreach (var ocrModel in ocrResponse)
                {
                    var elasticResponse = await _learningManagementSystem.CreateElastic(ocrModel);
                }*/
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
        return View(request);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Faculties = await _learningManagementSystem
            .FacultyList(null);
        ViewBag.Users = await _learningManagementSystem
            .UserList(null);
        return View();
    }

    [HttpPost]
  public async Task<IActionResult> Create(TeacherRequest request)
{
    if (ModelState.IsValid)
    {
        try
        {
            var fileName = Path.GetFileName(request.File.FileName);
            var provider = new FileExtensionContentTypeProvider();
            provider.TryGetContentType(fileName, out var mimeType);

            // Default mime type if the above fails
            if (mimeType == null)
            {
                mimeType = "application/octet-stream";
            }

            // Opening the stream to ensure it's valid throughout the request lifecycle
            using (var fileStream = request.File.OpenReadStream())
            {
                var streamPart = new StreamPart(fileStream, fileName, mimeType, "file");

                var response = await _learningManagementSystem.CreateTeacher(
                    request.Name,
                    request.Surname,
                    request.Occupation,
                    request.Salary,
                    request.AppUserId,
                    request.FacultyId.ToString(),
                    streamPart);

                var streamParts = new List<StreamPart> { streamPart };
                Console.WriteLine("UploadFileInModel_WithRefitAsync response :" + response);
                var document = await _learningManagementSystem.GetByOwner(response.Id);
            }

            return RedirectToAction("Index");
        }
        catch (ValidationApiException ex)
        {
            ModelState.AddValidationError(ex);
        }
        catch (ApiException e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
        }
        catch (Exception e)
        {
            _toastNotification.AddErrorToastMessage(e.Message);
        }
    }

    ViewBag.Faculties = await _learningManagementSystem.FacultyList(null);
    ViewBag.Users = await _learningManagementSystem.UserList(null);
    return View(request);
}


    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _learningManagementSystem.RemoveTeacher(id);
        return RedirectToAction("Index");
    }

}