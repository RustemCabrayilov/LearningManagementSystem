using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.Student;

namespace LearningManagementSystem.Application.Abstractions.Services.StudentGroup;

public record StudentGroupResponse(
    Guid Id,
    StudentResponse Student,
    GroupResponse Group
    );