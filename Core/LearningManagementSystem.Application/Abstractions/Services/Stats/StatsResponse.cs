namespace LearningManagementSystem.Application.Abstractions.Services.Stats;

public class StatsResponse<T>
{
    public T Entity { get; set; }
    public float Count { get; set; }
}