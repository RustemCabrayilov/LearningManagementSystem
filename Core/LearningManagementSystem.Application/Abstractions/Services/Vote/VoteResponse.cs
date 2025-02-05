namespace LearningManagementSystem.Application.Abstractions.Services.Vote;

public record VoteResponse(
    Guid Id,
    int Point,
    Domain.Entities.Question Question,
    Domain.Entities.Student Student
);