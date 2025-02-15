
using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(TeacherConfiguration))]

public class Teacher:Employee
{
    public string Occupation { get; set; }
    public float Rate { get; set; }   
    public AppUser AppUser { get; set; }
    public string AppUserId { get; set; }
    public Faculty Faculty { get; set; }
    public Guid FacultyId { get; set; }
    public List<Group> Groups { get; set; }
    public List<Survey> Surveys { get; set; }
    
}