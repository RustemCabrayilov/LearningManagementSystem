namespace LearningManagementSystem.Application.Abstractions.Services.OCR;

public record OCRModel(
    Guid Id,
    Guid DocumentId,
    string Headline,
    string Content
);