using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Student;
using LearningManagementSystem.Application.Abstractions.Services.StudentExam;

namespace LearningManagementSystem.BLL.Services.StudentExam;

public class StudentExamValidator : AbstractValidator<StudentExamRequest[]>
{
    public StudentExamValidator()
    {
        RuleForEach(x => x)
            .SetValidator(new SingleStudentExamValidator());
    }
}
public class SingleStudentExamValidator : AbstractValidator<StudentExamRequest>
{
    public SingleStudentExamValidator()
    {
        RuleFor(x=>x.StudentId).NotNull();
        RuleFor(x=>x.ExamId).NotNull();
        RuleFor(x=>x.Point).GreaterThanOrEqualTo(0);
    }
}