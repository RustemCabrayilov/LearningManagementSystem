using LearningManagementSystem.Application.Abstractions.Services.Survey;

namespace LearningManagementSystem.Application.Abstractions.Services.Question;

public record QuestionResponse(
    Guid Id,
    string Description,
    int MaxPoint,
    SurveyResponse Survey);