using FluentValidation;
using FluentValidation.AspNetCore;
using LearningManagementSystem.Application.Abstractions.Services.Attendance;
using LearningManagementSystem.Application.Abstractions.Services.Auth;
using LearningManagementSystem.Application.Abstractions.Services.Chat;
using LearningManagementSystem.Application.Abstractions.Services.Dean;
using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Application.Abstractions.Services.Email;
using LearningManagementSystem.Application.Abstractions.Services.Exam;
using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.GroupSchedule;
using LearningManagementSystem.Application.Abstractions.Services.Lesson;
using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Application.Abstractions.Services.Question;
using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;
using LearningManagementSystem.Application.Abstractions.Services.Role;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.StudentExam;
using LearningManagementSystem.Application.Abstractions.Services.StudentGroup;
using LearningManagementSystem.Application.Abstractions.Services.StudentRetakeExam;
using LearningManagementSystem.Application.Abstractions.Services.Subject;
using LearningManagementSystem.Application.Abstractions.Services.Survey;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Application.Abstractions.Services.Term;
using LearningManagementSystem.Application.Abstractions.Services.Theme;
using LearningManagementSystem.Application.Abstractions.Services.Transcript;
using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Application.Abstractions.Services.Vote;
using LearningManagementSystem.BLL.Services.Attendance;
using LearningManagementSystem.BLL.Services.Auth;
using LearningManagementSystem.BLL.Services.Chat;
using LearningManagementSystem.BLL.Services.Dean;
using LearningManagementSystem.BLL.Services.Document;
using LearningManagementSystem.BLL.Services.Email;
using LearningManagementSystem.BLL.Services.Exam;
using LearningManagementSystem.BLL.Services.Faculty;
using LearningManagementSystem.BLL.Services.Group;
using LearningManagementSystem.BLL.Services.GroupSchedule;
using LearningManagementSystem.BLL.Services.Lesson;
using LearningManagementSystem.BLL.Services.Major;
using LearningManagementSystem.BLL.Services.Question;
using LearningManagementSystem.BLL.Services.RetakeExam;
using LearningManagementSystem.BLL.Services.Role;
using LearningManagementSystem.BLL.Services.Student;
using LearningManagementSystem.BLL.Services.StudentExam;
using LearningManagementSystem.BLL.Services.StudentGroup;
using LearningManagementSystem.BLL.Services.StudentRetakeExam;
using LearningManagementSystem.BLL.Services.Subject;
using LearningManagementSystem.BLL.Services.Survey;
using LearningManagementSystem.BLL.Services.Teacher;
using LearningManagementSystem.BLL.Services.Term;
using LearningManagementSystem.BLL.Services.Theme;
using LearningManagementSystem.BLL.Services.Transcript;
using LearningManagementSystem.BLL.Services.User;
using LearningManagementSystem.BLL.Services.Vote;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace LearningManagementSystem.BLL.Extensions;
public static class ServiceRegister
{
    public static void AddBusinessLogicServices(this IServiceCollection services)
    {
        services.AddScoped<IStudentService, StudentService>();
        services.AddScoped<IAttendanceService, AttendanceService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<IMajorService, MajorService>();
        services.AddScoped<IFacultyService, FacultyService>();
        services.AddScoped<IDocumentService, DocumentService>();
        services.AddScoped<IDeanService, DeanService>();
        services.AddScoped<ISurveyService, SurveyService>();
        services.AddScoped<ITermService, TermService>();
        services.AddScoped<ITeacherService, TeacherService>();
        services.AddScoped<IRetakeExamService, RetakeExamService>();
        services.AddScoped<IVoteService, VoteService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ILessonService, LessonService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<ISubjectService, SubjectService>();
        services.AddScoped<IGroupScheduleService, GroupScheduleService>();
        services.AddScoped<IStudentExamService, StudentExamService>();
        services.AddScoped<IExamService, ExamService>();
        services.AddScoped<ITranscriptService, TranscriptService>();
        services.AddScoped<IStudentRetakeExamService, StudentRetakeExamService>();
        services.AddScoped<IStudentGroupService, StudentGroupService>();
        services.AddScoped<IThemeService,ThemeService>();
        services.AddScoped<IQuestionService,QuestionService>();
        services.AddScoped<IChatService,ChatService>();
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(SignInValidator)));  
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(SignUpValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(DeanValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(DocumentValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(FacultyValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(GroupValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(LessonValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(MajorValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(QuestionValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(StudentValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(SubjectValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(SurveyValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(TeacherValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(TermValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(UserValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(VoteValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(AttendanceValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(SingleAttendanceValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(GroupScheduleValidator)));
        services.AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining(typeof(ExamValidator)));
        ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("az");
    }
    public static void AddCustomSerilog(this ILoggingBuilder logBuilder)
    {
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(new ConfigurationBuilder()
                .AddJsonFile("serilog-config.json")
                .Build())
            .Enrich.FromLogContext()
            .CreateLogger();
        logBuilder.ClearProviders();
        logBuilder.AddSerilog(logger);
    }
}