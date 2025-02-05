using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Persistence.Filters;
using LearningManagementSystem.UI.Extensions;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using NToastNotify;
using Refit;

namespace LearningManagementSystem.UI.Areas.Admin.Controllers;

[Area("Admin")]
public class StudentsController(
    ILearningManagementSystem _learningManagementSystem,
    IToastNotification _toastNotification) : Controller
{
    public async Task<IActionResult> Index([FromQuery] RequestFilter? filter)
    {
        var responses = await _learningManagementSystem.StudentList(filter);
        var responseTasks = responses.Select(async response =>
        {
            var qrCodeStream =await _learningManagementSystem.GenerateQRCode(response.Id);

            using var memoryStream = new MemoryStream();
          await  qrCodeStream.CopyToAsync(memoryStream);
            var qrCodeBytes = memoryStream.ToArray();
            var base64String = Convert.ToBase64String(qrCodeBytes);
            string qrCodeUrl = $"data:image/png;base64,{base64String}";

            return response with { QrCodeUrl = qrCodeUrl };
        }).ToList();
        responses = await Task.WhenAll(responseTasks);
        int totalStudents = _learningManagementSystem.StudentList(new RequestFilter(){AllUsers = true}).Result.Count;
        ViewBag.TotalPages = (int)Math.Ceiling(totalStudents / (double)filter.Count);
        ViewBag.CurrentPage = filter.Page;
        return View(responses);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        try
        {
            var response = await _learningManagementSystem.GetStudent(id);
            var model = new StudentRequest(response.AppUser.Id, response.Name, response.Surname, response.StudentNo,
                null);
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
    public async Task<IActionResult> Edit([FromRoute] Guid id, StudentRequest request)
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
                var response = await _learningManagementSystem.UpdateStudent(id,
                    request.AppUserId,
                    request.Name,
                    request.Surname,
                    request.StudentNo,
                    streamPart);
                Console.WriteLine("UploadFileInModel_WithRefitAsync response :" + response);
                var document = await _learningManagementSystem.GetByOwner(response.Id);


                /*var ocrResponse = await _learningManagementSystem.GetTextFromFile(fileInfoPart, document.Id);
                foreach (var ocrModel in ocrResponse)
                {
                    var elasticResponse = await _learningManagementSystem.CreateElastic(ocrModel);
                }*/
                /*System.IO.File.Delete(tempFilePath);*/
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

        ViewBag.Users = await _learningManagementSystem.UserList(null);
        return View(request);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Users = await _learningManagementSystem
            .UserList(null);
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(StudentRequest request)
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
                var response = await _learningManagementSystem.CreateStudent(
                    request.AppUserId,
                    request.Name,
                    request.Surname,
                    request.StudentNo,
                    streamPart);
                Console.WriteLine("UploadFileInModel_WithRefitAsync response :" + response);
                var document = await _learningManagementSystem.GetByOwner(response.Id);


                /*var ocrResponse = await _learningManagementSystem.GetTextFromFile(fileInfoPart, document.Id);
                foreach (var ocrModel in ocrResponse)
                {
                    var elasticResponse = await _learningManagementSystem.CreateElastic(ocrModel);
                }*/
                /*System.IO.File.Delete(tempFilePath);*/
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

    [HttpGet]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _learningManagementSystem.RemoveStudent(id);
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var response = await _learningManagementSystem.GetStudent(id);
        var document = await _learningManagementSystem.GetByOwner(id);
        return Json(new
            {
                student = response,
                fileUrl = document.Path,
                fileName=document.FileName
            }
        );
    }
    


    public async Task<IActionResult> AssignMajor(Guid id)
    {
        var model = new StudentMajorDto(id,Guid.Empty);
        return View(model);
    }
    
    public async Task<IActionResult> AssignMajor(StudentMajorDto dto)
    {
        /*var response=await _learningManagementSystem.a;*/
        return RedirectToAction("Index");
    }
}