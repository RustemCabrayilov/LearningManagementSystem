using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.UI.Integrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Refit;

namespace LearningManagementSystem.UI.Controllers;

public class DocumentsController(ILearningManagementSystem _learningManagementSystem) : Controller
{
    // GET
    public async Task<IActionResult> Details(Guid id)
    {
        var response = await _learningManagementSystem.GetFile(id);
        return View(response);
    }
    

    public async Task<IActionResult> CreateExamDocument(List<IFormFile> files, Guid ownerId)
    {
        try
        {
            var streamParts = new List<StreamPart>();
            var request = await _learningManagementSystem.GetStudentExam(ownerId);
            foreach (var file in files)
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

            var responses = await _learningManagementSystem.CreateDocumentByOwner(
                streamParts,
                ownerId,
                DocumentType.StudentExam);
        }
        catch (ValidationApiException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (ApiException e)
        {
            Console.WriteLine(e);
            throw;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return RedirectToAction("Index","Home");
    }
}