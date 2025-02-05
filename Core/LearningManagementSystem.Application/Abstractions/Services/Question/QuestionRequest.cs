using LearningManagementSystem.Application.Abstractions.Services.Survey;

namespace LearningManagementSystem.Application.Abstractions.Services.Question;

public record QuestionRequest(
    string Description,
    int MaxPoint,
    Guid SurveyId
);