using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.Document;

public record DocumentRequest(
    Guid Id,
    DocumentType DocumentType,
    string? Path,
    string Key,
    string FileName,
    string OriginName,
    Guid OwnerId,
    List<IFormFile> Files=null
);