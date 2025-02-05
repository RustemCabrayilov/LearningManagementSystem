using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(GroupConfiguration))]
public class Group:BaseEntity
{
    public char Code { get; set; }
    public string Name { get; set; }
    public int Credit { get; set; }
    public bool CanApply { get; set; }

    public Term Term { get; set; }   
    public Guid TermId { get; set; }
    public Teacher Teacher { get; set; }
    public Guid TeacherId { get; set; }
    public Subject Subject { get; set; }
    public Guid SubjectId { get; set; }
    public Major Major { get; set; }
    public Guid MajorId { get; set; }
    public List<Lesson> Lessons { get; set; }
    public List<Exam> Exams { get; set; }
    public List<GroupSchedule> GroupSchedules { get; set; }
}