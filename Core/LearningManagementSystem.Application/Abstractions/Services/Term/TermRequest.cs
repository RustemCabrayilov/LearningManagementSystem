using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Domain.Enums;

namespace LearningManagementSystem.Application.Abstractions.Services.Term;

public record TermRequest(
    string Name,
    DateOnly StartDate,
    DateOnly EndDate,
    bool IsActive,
    TermSeason TermSeason);