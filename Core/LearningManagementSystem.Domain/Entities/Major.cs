using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class Major:BaseEntity
{
    public string Name { get; set; }
    public float Point { get; set; }
    public string EducationLanguage { get; set; }
    public decimal TuitionFee { get; set; }
    public decimal StateFunded { get; set; }
    public Faculty Faculty { get; set; }
    public Guid FacultyId { get; set; }
}