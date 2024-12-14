namespace LearningManagementSystem.Application.Abstractions.Services.Vote;

public record VoteRequest(
    string Description,
    int Point,
    Domain.Entities.Survey Survey,
    Guid SurveyId,
    Domain.Entities.Teacher Teacher,
    Guid TeacherId,
    Domain.Entities.Student Student,
    Guid StudentId
);