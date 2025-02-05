using LearningManagementSystem.Application.Abstractions.Services.Question;
using LearningManagementSystem.Application.Abstractions.Services.Term;
using LearningManagementSystem.Application.Abstractions.Services.Vote;

namespace LearningManagementSystem.Application.Abstractions.Services.Survey;

public record SurveyResponse(
    Guid Id,
    string Name,
    bool IsActive,
    TermResponse Term=null,
    List<QuestionResponse> Questions=null);