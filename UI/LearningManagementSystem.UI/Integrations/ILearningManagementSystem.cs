using LearningManagementSystem.Application.Abstractions.Services.Attendance;
using LearningManagementSystem.Application.Abstractions.Services.Auth;
using LearningManagementSystem.Application.Abstractions.Services.Chat;
using LearningManagementSystem.Application.Abstractions.Services.Dean;
using LearningManagementSystem.Application.Abstractions.Services.Document;
using LearningManagementSystem.Application.Abstractions.Services.Exam;
using LearningManagementSystem.Application.Abstractions.Services.Faculty;
using LearningManagementSystem.Application.Abstractions.Services.Group;
using LearningManagementSystem.Application.Abstractions.Services.GroupSchedule;
using LearningManagementSystem.Application.Abstractions.Services.Lesson;
using LearningManagementSystem.Application.Abstractions.Services.Major;
using LearningManagementSystem.Application.Abstractions.Services.OCR;
using LearningManagementSystem.Application.Abstractions.Services.Question;
using LearningManagementSystem.Application.Abstractions.Services.RetakeExam;
using LearningManagementSystem.Application.Abstractions.Services.Role;
using LearningManagementSystem.Application.Abstractions.Services.Stats;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.StudentExam;
using LearningManagementSystem.Application.Abstractions.Services.StudentGroup;
using LearningManagementSystem.Application.Abstractions.Services.StudentRetakeExam;
using LearningManagementSystem.Application.Abstractions.Services.Subject;
using LearningManagementSystem.Application.Abstractions.Services.Survey;
using LearningManagementSystem.Application.Abstractions.Services.Teacher;
using LearningManagementSystem.Application.Abstractions.Services.Term;
using LearningManagementSystem.Application.Abstractions.Services.Theme;
using LearningManagementSystem.Application.Abstractions.Services.Token;
using LearningManagementSystem.Application.Abstractions.Services.Transcript;
using LearningManagementSystem.Application.Abstractions.Services.User;
using LearningManagementSystem.Application.Abstractions.Services.Vote;
using LearningManagementSystem.Domain.Enums;
using LearningManagementSystem.Persistence.Filters;
using Microsoft.AspNetCore.Mvc;
using Refit;

namespace LearningManagementSystem.UI.Integrations;

public interface ILearningManagementSystem
{
    //Faculty
    [Get("/api/Faculties")]
    Task<IList<FacultyResponse>> FacultyList(RequestFilter? filter);

    [Post("/api/Faculties")]
    Task<FacultyResponse> CreateFaculty(FacultyRequest dto);

    [Put("/api/Faculties")]
    Task<FacultyResponse> UpdateFaculty(Guid id, FacultyRequest dto);

    [Delete("/api/Faculties")]
    Task<FacultyResponse> RemoveFaculty(Guid id);

    [Get("/api/Faculties/{id}")]
    Task<FacultyResponse> GetFaculty(Guid id);

//Major
    [Get("/api/Majors")]
    Task<IList<MajorResponse>> MajorList(RequestFilter? filter);

    [Post("/api/Majors")]
    Task<MajorResponse> CreateMajor(MajorRequest dto);

    [Put("/api/Majors")]
    Task<MajorResponse> UpdateMajor(Guid id, MajorRequest dto);

    [Delete("/api/Majors")]
    Task<MajorResponse> RemoveMajor(Guid id);

    [Get("/api/Majors/{id}")]
    Task<MajorResponse> GetMajor(Guid id);

    //User
    [Get("/api/Users")]
    Task<IList<UserResponse>> UserList(RequestFilter? filter);

    [Post("/api/Users")]
    Task<UserResponse> CreateUser(UserRequest dto);

    [Put("/api/Users")]
    Task<UserResponse> UpdateUser(string id, UserRequest dto);

    [Delete("/api/Users")]
    Task<UserResponse> RemoveUser(string id);

    [Get("/api/Users/{id}")]
    Task<UserResponse> GetUser(string id);

    [Post("/api/Users/assign-role")]
    Task<UserResponse> AssignRole(UserRoleDto request);

    [Post("/api/Users/infoby-token")]
    Task<UserClaim> GetUserInfosByToken(string token);

    //Role
    [Get("/api/Roles")]
    Task<IList<RoleResponse>> RoleList(RequestFilter? filter);

    [Post("/api/Roles")]
    Task<RoleResponse> CreateRole(RoleRequest dto);

    [Put("/api/Roles")]
    Task<RoleResponse> UpdateRole(string id, RoleRequest dto);

    [Delete("/api/Roles")]
    Task<RoleResponse> RemoveRole(string id);

    [Get("/api/Roles/{id}")]
    Task<RoleResponse> GetRole(string id);

    //Term
    [Get("/api/Terms")]
    Task<IList<TermResponse>> TermList(RequestFilter? filter);

    [Post("/api/Terms")]
    Task<TermResponse> CreateTerm(TermRequest dto);

    [Put("/api/Terms")]
    Task<TermResponse> UpdateTerm(Guid id, TermRequest dto);

    [Delete("/api/Terms")]
    Task<TermResponse> RemoveTerm(Guid id);

    [Get("/api/Terms/{id}")]
    Task<TermResponse> GetTerm(Guid id);
    
    [Post("/api/Terms/activate")]
    Task<TermResponse> ActivateTerm(Guid id);

    //Group
    [Get("/api/Groups")]
    Task<IList<GroupResponse>> GroupList(RequestFilter? filter);

    [Post("/api/Groups")]
    Task<GroupResponse> CreateGroup(GroupRequest dto);

    [Put("/api/Groups")]
    Task<GroupResponse> UpdateGroup(Guid id, GroupRequest dto);

    [Delete("/api/Groups")]
    Task<GroupResponse> RemoveGroup(Guid id);

    [Get("/api/Groups/{id}")]
    Task<GroupResponse> GetGroup(Guid id);

    [Post("/api/Groups/activate")]
    Task<GroupResponse> ActivateGroup(Guid id);

    //Lesson
    [Get("/api/Lessons")]
    Task<IList<LessonResponse>> LessonList(RequestFilter? filter);

    [Post("/api/Lessons")]
    Task<LessonResponse> CreateLesson(LessonRequest dto);

    [Put("/api/Lessons")]
    Task<LessonResponse> UpdateLesson(Guid id, LessonRequest dto);

    [Delete("/api/Lessons")]
    Task<LessonResponse> RemoveLesson(Guid id);

    [Get("/api/Lessons/{id}")]
    Task<LessonResponse> GetLesson(Guid id);

    //Survey
    [Get("/api/Surveys")]
    Task<IList<SurveyResponse>> SurveyList(RequestFilter? filter);

    [Post("/api/Surveys")]
    Task<SurveyResponse> CreateSurvey(SurveyRequest dto);

    [Put("/api/Surveys")]
    Task<SurveyResponse> UpdateSurvey(Guid id, SurveyRequest dto);

    [Delete("/api/Surveys")]
    Task<SurveyResponse> RemoveSurvey(Guid id);

    [Get("/api/Surveys/{id}")]
    Task<SurveyResponse> GetSurvey(Guid id);
    [Post("/api/Surveys/activate")]
    Task<GroupResponse> ActivateSurvey(Guid id);

    //RetakeExam
    [Get("/api/RetakeExams")]
    Task<IList<RetakeExamResponse>> RetakeExamList(RequestFilter? filter);

    [Post("/api/RetakeExams")]
    Task<RetakeExamResponse> CreateRetakeExam(RetakeExamRequest request);

    [Put("/api/RetakeExams")]
    Task<RetakeExamResponse> UpdateRetakeExam(RetakeExamRequest request);

    [Delete("/api/RetakeExams")]
    Task<RetakeExamResponse> RemoveRetakeExam(Guid id);

    [Get("/api/RetakeExams/{id}")]
    Task<RetakeExamResponse> GetRetakeExam(Guid id);

    //Attendance
    [Get("/api/Attendances")]
    Task<IList<AttendanceResponse>> AttendanceList(RequestFilter? filter);

    [Post("/api/Attendances")]
    Task<AttendanceResponse> CreateAttendance(AttendanceRequest dto);

    [Put("/api/Attendances")]
    Task<AttendanceResponse> UpdateAttendance(Guid id, AttendanceRequest dto);

    [Put("/api/Attendances/attendance-list")]
    Task<AttendanceResponse[]> UpdateAttendanceList(AttendanceRequest[] dtos);

    [Delete("/api/Attendances")]
    Task<AttendanceResponse> RemoveAttendance(Guid id);

    [Get("/api/Attendances/{id}")]
    Task<AttendanceResponse> GetAttendance(Guid id);
    
    [Post("/api/Attendances/student-attendance-list")]
    Task<List<AttendanceResponse>> GetStudentAttendance(StudentAttendanceDto request);

    //Question
    [Get("/api/Questions")]
    Task<IList<QuestionResponse>> QuestionList(RequestFilter? filter);

    [Post("/api/Questions")]
    Task<QuestionResponse> CreateQuestion(QuestionRequest dto);

    [Put("/api/Questions")]
    Task<QuestionResponse> UpdateQuestion(Guid id, QuestionRequest dto);

    [Delete("/api/Questions")]
    Task<QuestionResponse> RemoveQuestion(Guid id);

    [Get("/api/Questions/{id}")]
    Task<QuestionResponse> GetQuestion(Guid id);

    //Exam
    [Get("/api/Exams")]
    Task<IList<ExamResponse>> ExamList(RequestFilter? filter);

    [Post("/api/Exams")]
    Task<ExamResponse> CreateExam(ExamRequest request);

    [Put("/api/Exams")]
    Task<ExamResponse> UpdateExam(
        Guid id,
        ExamRequest request);

    [Delete("/api/Exams")]
    Task<ExamResponse> RemoveExam(Guid id);

    [Get("/api/Exams/{id}")]
    Task<ExamResponse> GetExam(Guid id);

    //Subject
    [Get("/api/Subjects")]
    Task<IList<SubjectResponse>> SubjectList(RequestFilter? filter);

    [Post("/api/Subjects")]
    Task<SubjectResponse> CreateSubject(SubjectRequest dto);

    [Put("/api/Subjects")]
    Task<SubjectResponse> UpdateSubject(Guid id, SubjectRequest dto);

    [Delete("/api/Subjects")]
    Task<SubjectResponse> RemoveSubject(Guid id);

    [Get("/api/Subjects/{id}")]
    Task<SubjectResponse> GetSubject(Guid id);

    //Student
    [Get("/api/Students")]
    Task<IList<StudentResponse>> StudentList(RequestFilter? filter);

    [Multipart]
    [Post("/api/Students")]
    Task<StudentResponse> CreateStudent(
        [AliasAs(("AppUserId"))] string AppUserId,
        [AliasAs("Name")] string Name,
        [AliasAs("Surname")] string Surname,
        [AliasAs("StudentNo")] string StudentNo,
        [AliasAs("File")] StreamPart file);

    [Multipart]
    [Put("/api/Students/{id}")]
    Task<StudentResponse> UpdateStudent(
        Guid id,
        [AliasAs(("AppUserId"))] string AppUserId,
        [AliasAs("Name")] string Name,
        [AliasAs("Surname")] string Surname,
        [AliasAs("StudentNo")] string StudentNo,
        [AliasAs("File")] StreamPart file
    );

    [Delete("/api/Students")]
    Task<StudentResponse> RemoveStudent(Guid id);

    [Get("/api/Students/{id}")]
    Task<StudentResponse> GetStudent(Guid id);

    [Post("/api/Students/assign-group")]
    Task<IActionResult> AssignGroup(StudentGroupDto request);
    
    [Post("/api/Students/assign-retakeExam")]
    Task<StudentRetakeExamResponse> AssignRetakeExam(StudentRetakeExamDto request);

    [Post("/api/Students/assign-group-list")]
    Task<StudentGroupDto[]> AssignGroupList(StudentGroupDto[] request);

    //Teacher
    [Get("/api/Teachers")]
    Task<IList<TeacherResponse>> TeacherList(RequestFilter? filter);

    [Multipart]
    [Post("/api/Teachers")]
    Task<TeacherResponse> CreateTeacher(
        [AliasAs("Name")] string name,
        [AliasAs("Surname")] string surname,
        [AliasAs("Occupation")] string occupation,
        [AliasAs("Salary")] decimal salary,
        [AliasAs("AppUserId")] string appUserId,
        [AliasAs("FacultyId")] string facultyId,
        [AliasAs("File")] StreamPart file
    );

    [Multipart]
    [Put("/api/Teachers/{id}")]
    Task<TeacherResponse> UpdateTeacher(
        Guid id,
        [AliasAs("Name")] string name,
        [AliasAs("Surname")] string surname,
        [AliasAs("Occupation")] string occupation,
        [AliasAs("Salary")] decimal salary,
        [AliasAs("AppUserId")] string appUserId,
        [AliasAs("FacultyId")] string facultyId,
        [AliasAs("File")] StreamPart file);

    [Delete("/api/Teachers")]
    Task<TeacherResponse> RemoveTeacher(Guid id);

    [Get("/api/Teachers/{id}")]
    Task<TeacherResponse> GetTeacher(Guid id);

    //GroupSchedule
    [Get("/api/GroupSchedules")]
    Task<IList<GroupScheduleResponse>> GroupScheduleList(RequestFilter? filter);

    [Post("/api/GroupSchedules")]
    Task<GroupScheduleResponse> CreateGroupSchedule(GroupScheduleRequest dto);

    [Put("/api/GroupSchedules")]
    Task<GroupScheduleResponse> UpdateGroupSchedule(Guid id, GroupScheduleRequest dto);

    [Delete("/api/GroupSchedules")]
    Task<GroupScheduleResponse> RemoveGroupSchedule(Guid id);

    [Get("/api/GroupSchedules/{id}")]
    Task<GroupScheduleResponse> GetGroupSchedule(Guid id);

    //Dean
    [Get("/api/Deans")]
    Task<IList<DeanResponse>> DeanList(RequestFilter? filter);

    [Multipart]
    [Post("/api/Deans")]
    Task<DeanResponse> CreateDean(
        [AliasAs("Name")] string name,
        [AliasAs("Surname")] string surname,
        [AliasAs("Salary")] decimal salary,
        [AliasAs("PositionType")] int positionType,
        [AliasAs("FacultyId")] string facultyId,
        [AliasAs("AppUserId")] string appUserId,
        [AliasAs("File")] StreamPart file);

    [Multipart]
    [Put("/api/Deans/{id}")]
    Task<DeanResponse> UpdateDean(
        Guid id,
        [AliasAs("Name")] string name,
        [AliasAs("Surname")] string surname,
        [AliasAs("Salary")] decimal salary,
        [AliasAs("PositionType")] int positionType,
        [AliasAs("FacultyId")] string facultyId,
        [AliasAs("AppUserId")] string appUserId,
        [AliasAs("File")] StreamPart file);

    [Delete("/api/Deans")]
    Task<DeanResponse> RemoveDean(Guid id);

    [Get("/api/Deans/{id}")]
    Task<DeanResponse> GetDean(Guid id);

    //OCR
    [Post("/api/OCR")]
    [Multipart]
    Task<List<OCRModel>> GetTextFromFile([AliasAs("file")] FileInfoPart file, [AliasAs("documentId")] Guid documentId);

    //QRCode
    [Post("/api/QRCode")]
    Task<Stream> GenerateQRCode(Guid documentId);

    //Elastic
    [Post("/api/Elastic")]
    ValueTask<bool> CreateElastic(UserResponse dto);

    //Elastic
    [Get("/api/Elastic")]
    Task<List<UserResponse>> ElasticList(string name, string value);

    //Document
    [Post("/api/Documents")]
    Task<DocumentResponse> CreateDocument(DocumentRequest request);


    [Multipart]
    [Post("/api/Documents/create-byowner")]
    Task<List<DocumentResponse>> CreateDocumentByOwner(
        [AliasAs("Files")] StreamPart[] files,
        [AliasAs("OwnerId")] string ownerId,
        [AliasAs("DocumentType")] string documentType);

    [Get("/api/Documents/get-byowner/{ownerId}")]
    Task<DocumentResponse> GetByOwner(Guid ownerId);

    [Get("/api/Documents/{id}")]
    Task<DocumentResponse> GetDocument(Guid id);

    [Get("/api/Documents/getfile/{id}")]
    Task<DocumentResponse> GetFile(Guid id);

//Account
    [Post("/api/Auth/sign-in")]
    Task<Token> SignIn(SignInRequest dto);

    [Post("/api/Auth/sign-up")]
    Task<Token> SignUp(SignUpRequest dto);

    [Get("/api/Auth/confirm-email")]
    Task ConfirmEmail(string email, string token);
    [Put("/api/StudentExams/studentExam-list")]
    Task<StudentExamResponse[]> UpdateStudentExam(StudentExamRequest[] requests);

    [Get("/api/StudentExams/{id}")]
    Task<StudentExamResponse[]> GetStudentExam(Guid id);

    [Get("/api/StudentExams")]
    Task<StudentExamResponse[]> StudentExamList([FromQuery] RequestFilter? filter);

    [Post("/api/GeminiAI")]
    Task<string> AskGeminiAI(string prompt);
    
    //Transcript
    [Get("/api/Transcripts")]
    Task<IList<TranscriptResponse>> TranscriptList(RequestFilter? filter);

    [Post("/api/Transcripts")]
    Task<TranscriptResponse> CreateTranscript(TranscriptRequest dto);

    [Put("/api/Transcripts")]
    Task<TranscriptResponse> UpdateTranscript(Guid id, TranscriptRequest dto);

    [Delete("/api/Transcripts")]
    Task<TranscriptResponse> RemoveTranscript(Guid id);

    [Get("/api/Transcripts/{id}")]
    Task<TranscriptResponse> GetTranscript(Guid id);
    
    //StudentRetakeExam
    [Get("/api/StudentRetakeExams")]
    Task<IList<StudentRetakeExamResponse>> StudentRetakeExamList(RequestFilter? filter);
    
    [Get("/api/StudentRetakeExams/active-term-requests")]
    Task<IList<StudentRetakeExamResponse>> ActiveTermRequests(RequestFilter? filter);

    [Post("/api/StudentRetakeExams")]
    Task<StudentRetakeExamResponse> CreateStudentRetakeExam(StudentRetakeExamDto dto);

    [Put("/api/StudentRetakeExams")]
    Task<StudentRetakeExamResponse> UpdateStudentRetakeExam(Guid id, StudentRetakeExamDto dto);

    [Delete("/api/StudentRetakeExams")]
    Task<StudentRetakeExamResponse> RemoveStudentRetakeExam(Guid id);

    [Get("/api/StudentRetakeExams/{id}")]
    Task<StudentRetakeExamResponse> GetStudentRetakeExam(Guid id);
    
    //StudentGroup
    [Get("/api/StudentGroups")]
    Task<IList<StudentGroupResponse>> StudentGroupList(RequestFilter? filter);

    [Post("/api/StudentGroups")]
    Task<StudentGroupResponse> CreateStudentGroup(StudentRetakeExamDto dto);

    [Put("/api/StudentGroups")]
    Task<StudentGroupResponse> UpdateStudentGroup(Guid id, StudentRetakeExamDto dto);

    [Delete("/api/StudentGroups")]
    Task<StudentGroupResponse> RemoveStudentGroup(Guid id);

    [Get("/api/StudentGroups/{id}")]
    Task<StudentGroupResponse> GetStudentGroup(Guid id);
    
    //Theme
    [Get("/api/Themes")]
    Task<IList<ThemeResponse>> ThemeList(RequestFilter? filter);

    [Multipart]
    [Post("/api/Themes")]
    Task<ThemeResponse> CreateTheme(
        [AliasAs("Title")] string title,
        [AliasAs("File")] StreamPart file);
[Multipart]
    [Put("/api/Themes")]
    Task<ThemeResponse> UpdateTheme(
        [AliasAs("Id")] Guid id,
        [AliasAs("Title")] string title,
        [AliasAs("File")] StreamPart file);

    [Delete("/api/Themes")]
    Task<ThemeResponse> RemoveTheme(Guid id);

    [Get("/api/Themes/{id}")]
    Task<ThemeResponse> GetTheme(Guid id);
    [Post("/api/Themes/activate")]
    Task<GroupResponse> ActivateTheme(Guid id);
    
    //Vote
    [Get("/api/Votes")]
    Task<IList<VoteResponse>> VoteList(RequestFilter? filter);

    [Post("/api/Votes")]
    Task<VoteResponse> CreateVote(ThemeRequest dto);
    
    [Post("/api/Votes/create-vote-list")]
    Task<IList<VoteResponse>> CreateVoteList(VoteRequest[] dtos);

    [Put("/api/Votes")]
    Task<VoteResponse> UpdateVote(Guid id, ThemeRequest dto);

    [Delete("/api/Votes")]
    Task<VoteResponse> RemoveVote(Guid id);

    [Get("/api/Votes/{id}")]
    Task<VoteResponse> GetVote(Guid id);  
    //Chat
    [Get("/api/Chats/{id}")]
    Task<List<UserResponse>> GetChatUsers(string id);
    
    [Post("/api/Chats")]
    Task<ChatResponse> SendMessage(ChatRequest request);
    
    [Post("/api/Chats/get-chat-messages")]
    Task<List<ChatResponse>> GetChatMessages(ChatMessageDto request);
    
    //Stats
    [Get("/api/Stats/average-students")]
    Task<List<StatsResponse<StudentResponse>>> AvergaeOfStudents();
    
    [Get("/api/Stats/top-teacher-rate")]
    Task<List<StatsResponse<TeacherResponse>>> TopRatedTeachers();
    
    [Get("/api/Stats/most-failed-subjects")]
    Task<List<StatsResponse<SubjectResponse>>> MostFailedSubjects();
    
    [Get("/api/Stats/most-efficient-teachers")]
    Task<List<StatsResponse<TeacherResponse>>> MostEfficientTeachers();


}