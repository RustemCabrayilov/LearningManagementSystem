using LearningManagementSystem.Application.Abstractions.Services.Term;
using LearningManagementSystem.Application.Abstractions.Services.Vote;

namespace LearningManagementSystem.Application.Abstractions.Services.Survey;

public record SurveyRequest(
    string Name,
    Guid TermId
);