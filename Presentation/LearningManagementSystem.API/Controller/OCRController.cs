using LearningManagementSystem.Application.Abstractions.Services.OCR;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Controller;
[ApiController]
[Route("api/[controller]")]
public class OCRController(IOCRService _ocrService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(IFormFile file,Guid documentId)
    {
        var response = await _ocrService.GetTextFromFileAsync(file,documentId);
        return Ok(response);
    }
}