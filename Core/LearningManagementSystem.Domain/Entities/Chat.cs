using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class Chat:BaseEntity
{
    public string UserId { get; set; }
    public string ToUserId { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime Date {  get; set; }
}