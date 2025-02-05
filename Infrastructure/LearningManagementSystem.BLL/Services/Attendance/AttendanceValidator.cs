using FluentValidation;
using LearningManagementSystem.Application.Abstractions.Services.Attendance;

namespace LearningManagementSystem.BLL.Services.Attendance
{
    public class AttendanceValidator : AbstractValidator<List<AttendanceRequest>>
    {
        public AttendanceValidator()
        {
            RuleForEach(x => x)
                .SetValidator(new SingleAttendanceValidator());
        }
    }

    public class SingleAttendanceValidator : AbstractValidator<AttendanceRequest>
    {
        public SingleAttendanceValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(x => x.LessonId)
                .NotEmpty()
                .NotEqual(Guid.Empty);

            RuleFor(x => x.Absence);
        }
    }
}