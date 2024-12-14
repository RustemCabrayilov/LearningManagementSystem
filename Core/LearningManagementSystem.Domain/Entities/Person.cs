using LearningManagementSystem.Domain.Entities.Common;

namespace LearningManagementSystem.Domain.Entities;

public class Person:BaseEntity
{
    public string Name { get; set; }   
    public string Surname { get; set; }   
}