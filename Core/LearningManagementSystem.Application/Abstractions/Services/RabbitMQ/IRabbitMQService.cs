namespace LearningManagementSystem.Application.Abstractions.Services.RabbitMQ;

public interface IRabbitMQService
{
    Task PublishMessageAsync<T>(T message,string queueName);
    Task GetMessage(string queueName);
}