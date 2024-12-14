using LearningManagementSystem.Domain.Enums;

namespace LearningManagementSystem.Application.Abstractions.Services.Document;

public record DocumentResponse(
    DocumentType DocumentType,
    string Path,
    string FileName,
    string OriginName,
    Guid OwnerId
    );