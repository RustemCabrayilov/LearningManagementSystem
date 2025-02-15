using LearningManagementSystem.Application.Abstractions.Services.Question;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.Survey;

namespace LearningManagementSystem.Application.Abstractions.Services.Vote;

public record VoteResponse(
    Guid Id,
    int Point,
    QuestionResponse Question=null,
    StudentResponse Student=null,
    SurveyResponse SurveyResponse=null
);