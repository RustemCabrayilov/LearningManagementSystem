using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(StudentConfiguration))]

public class Student:Person
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string StudentNo { get; set; }
    public AppUser AppUser { get; set; }
    public string AppUserId { get; set; }
    public List<StudentExam> Results { get; set; }
    public List<StudentLesson> StudentLessons { get; set; }
    public List<Vote> Votes { get; set; }
}