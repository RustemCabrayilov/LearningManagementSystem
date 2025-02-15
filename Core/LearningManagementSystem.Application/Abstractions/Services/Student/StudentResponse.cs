using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.StudentExam;
using LearningManagementSystem.Application.Abstractions.Services.StudentRetakeExam;
using LearningManagementSystem.Application.Abstractions.Services.Transcript;
using LearningManagementSystem.Application.Abstractions.Services.User;

namespace LearningManagementSystem.Application.Abstractions.Services.Student;

public record StudentResponse(
    Guid Id,
    string Name,
    string Surname,
    string StudentNo,
    UserResponse AppUser,
    string QrCodeUrl="",
    string FileUrl=null,
    IList<GroupResponse> Groups=null,
    IList<StudentExamResponse> StudentExams=null,
    IList<StudentRetakeExamResponse> StudentRetakeExams=null ,
    IList<TranscriptResponse> Transcripts=null); 