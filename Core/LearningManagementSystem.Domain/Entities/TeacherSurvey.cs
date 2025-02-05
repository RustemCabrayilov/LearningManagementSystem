using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class TeacherSurvey:BaseEntity
{
    public Teacher Teacher { get; set; }
    public Guid TeacherId { get; set; }
    public Survey Survey { get; set; }
    public Guid SurveyId { get; set; }
}