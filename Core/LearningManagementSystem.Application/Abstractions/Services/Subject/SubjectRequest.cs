namespace LearningManagementSystem.Application.Abstractions.Services.Subject;

public record SubjectRequest(
    string Name,
    string SubjectCode,
    int AttendanceLimit
);