using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Enums;

namespace LearningManagementSystem.Domain.Entities;

public class Term:BaseEntity
{
    public DateOnly StartDate { get; set; }
    public DateOnly EndDate { get; set; }
    public TermSeason TermSeason { get; set; }
    public List<Group> Groups { get; set; }
}