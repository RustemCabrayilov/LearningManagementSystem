using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(DeanConfiguration))]

public class Dean:Person
{
    public string Name { get; set; }
    public string Surname { get; set; }
    public string Salary { get; set; }
    public PositionType PositionType { get; set; }
    public Faculty Faculty { get; set; }
    public Guid FacultyId { get; set; }
    public AppUser AppUser { get; set; }
    public string AppUserId { get; set; }
}