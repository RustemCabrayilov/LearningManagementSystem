using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Student;

namespace LearningManagementSystem.BLL.Services.StudentGroup;

public class StudentGroupValidator:AbstractValidator<StudentGroupDto>
{
    public StudentGroupValidator()
    {
        RuleFor(x => x.StudentId).NotNull();
        RuleFor(x => x.GroupId).NotNull();
    }
}