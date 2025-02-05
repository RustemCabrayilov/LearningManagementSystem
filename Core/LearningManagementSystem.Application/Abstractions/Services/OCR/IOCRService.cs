using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.OCR;

public interface IOCRService
{
    Task<List<OCRModel>> GetTextFromFileAsync(IFormFile file,Guid documentId);
}