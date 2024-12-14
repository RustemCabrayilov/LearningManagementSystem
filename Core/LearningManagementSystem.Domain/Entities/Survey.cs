using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Enums;

namespace LearningManagementSystem.Domain.Entities;

public class Survey : BaseEntity
{
    public string Name { get; set; }
    public Term Term { get; set; }
    public Guid TermId { get; set; }
    public List<Vote> Votes { get; set; }
}