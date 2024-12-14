namespace LearningManagementSystem.Application.Abstractions.Services.Vote;

public class VoteResponse(
    Guid Id,
    string Description,
    int Point,
    Domain.Entities.Survey Survey,
    Domain.Entities.Teacher Teacher,
    Domain.Entities.Student Student
);