using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(SubjectConfiguration))]
public class Subject : BaseEntity
{
    public string Name { get; set; }
    public string SubjectCode { get; set; }
    public int AttendanceLimit { get; set; }
    public List<StudentSubject> StudentSubjects { get; set; }
    public List<Group> Groups { get; set; }
}