namespace LearningManagementSystem.Application.Abstractions.Services.Vote;

public record VoteRequest(
    int Point,
    Guid QuestionId,
    Guid StudentId
);