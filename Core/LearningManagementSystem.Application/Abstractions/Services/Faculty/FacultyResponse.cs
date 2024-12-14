using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;

namespace LearningManagementSystem.Application.Abstractions.Services.Faculty;

public record FacultyResponse(
    Guid Id,
    string Name,
    List<MajorResponse> Majors,
    List<TeacherResponse> Teachers
);