using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(MajorConfiguration))]
public class Major:BaseEntity
{
    public string Title { get; set; }
    public float Point { get; set; }
    public string EducationLanguage { get; set; }
    public decimal TuitionFee { get; set; }
    public bool StateFunded { get; set; }
    public Faculty Faculty { get; set; }
    public Guid FacultyId { get; set; }
}