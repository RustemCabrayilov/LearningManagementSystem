using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities.Common;

public class Person:BaseEntity
{
    public string Name { get; set; }   
    public string Surname { get; set; }   
}