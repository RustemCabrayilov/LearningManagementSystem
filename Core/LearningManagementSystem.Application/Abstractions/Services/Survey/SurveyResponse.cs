using LearningManagementSystem.Application.Abstractions.Services.Term;
using LearningManagementSystem.Application.Abstractions.Services.Vote;

namespace LearningManagementSystem.Application.Abstractions.Services.Survey;

public record SurveyResponse(
    Guid Id,
    string Name,
    TermResponse Term,
    Guid TermId,
    List<VoteResponse> Votes
    );