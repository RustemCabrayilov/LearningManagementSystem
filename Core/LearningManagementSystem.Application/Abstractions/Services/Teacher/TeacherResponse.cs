using LearningManagementSystem.Application.Abstractions.Services.Survey;
using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.Teacher;

public record TeacherResponse(
    Guid Id,
    string Name,
    string Surname,
    string Occupation,
    decimal Salary,
    float Rate,
    UserResponse AppUser,
    IList<SurveyResponse> Surveys,
    Domain.Entities.Faculty Faculty,
    string FileUrl=null
    );