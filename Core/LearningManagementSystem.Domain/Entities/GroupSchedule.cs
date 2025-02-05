using LearningManagementSystem.Domain.Configurations;
using LearningManagementSystem.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Entities;
[EntityTypeConfiguration(typeof(GroupScheduleConfiguration))]
public class GroupSchedule:BaseEntity
{
    public Group Group { get; set; }
    public Guid GroupId { get; set; }
    public string ClassTime { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
}