using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Faculty;

namespace LearningManagementSystem.BLL.Services.Faculty;

public class FacultyValidator:AbstractValidator<FacultyRequest>
{
    public FacultyValidator()
    {
        RuleFor(x=>x.Name).NotEmpty();
    }   
}