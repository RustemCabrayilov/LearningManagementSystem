using LearningManagementSystem.Application.Abstractions.Services.Dean;
using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.Survey;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Application.Abstractions.Services.Term;
using LearningManagementSystem.Application.Abstractions.Services.Vote;
using LearningManagementSystem.BLL.Services.Dean;
using LearningManagementSystem.BLL.Services.Document;
using LearningManagementSystem.BLL.Services.Faculty;
using LearningManagementSystem.BLL.Services.Major;
using LearningManagementSystem.BLL.Services.RetakeExam;
using LearningManagementSystem.BLL.Services.Student;
using LearningManagementSystem.BLL.Services.Survey;
using LearningManagementSystem.BLL.Services.Teacher;
using LearningManagementSystem.BLL.Services.Term;
using LearningManagementSystem.BLL.Services.Vote;
using Microsoft.Extensions.DependencyInjection;

namespace LearningManagementSystem.BLL.Extensions;

public static class RegisterServices
{
    public static void AddBusinessLogicServices(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IMajorService, MajorService>();
        services.AddScoped<IFacultyService, FacultyService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IDeanService, DeanService>();
        services.AddScoped<ISurveyService, SurveyService>();
        services.AddScoped<ITermService, TermService>();
        services.AddScoped<ITeacherService, TeacherService>();
        services.AddScoped<IRetakeExamService, RetakeExamService>();
        services.AddScoped<IVoteService, VoteService>();
    }
}