using LearningManagementSystem.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.Document;

public record DocumentByOwner(
    List<IFormFile> Files,
    Guid OwnerId,
    DocumentType DocumentType
    );