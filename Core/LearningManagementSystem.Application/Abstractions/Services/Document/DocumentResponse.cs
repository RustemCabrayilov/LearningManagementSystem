using LearningManagementSystem.Domain.Enums;

namespace LearningManagementSystem.Application.Abstractions.Services.Document;

public record DocumentResponse(
    Guid Id,
    DocumentType DocumentType,
    string? Path,
    string Key ,
    string FileName,
    string OriginName,
    Guid OwnerId
    );