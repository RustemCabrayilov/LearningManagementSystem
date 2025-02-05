using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(TermConfiguration))]
public class Term:BaseEntity
{
    public string Name { get; set; }
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public bool IsActive { get; set; }
    public TermSeason TermSeason { get; set; }
    public List<Group> Groups { get; set; }
    /*public List<StudentExam> StudentExams { get; set; }
    public List<StudentRetakeExam> StudentRetakeExams { get; set; }*/
}