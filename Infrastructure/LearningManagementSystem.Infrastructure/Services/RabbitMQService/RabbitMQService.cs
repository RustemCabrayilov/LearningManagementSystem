using System.Text;
using LearningManagementSystem.Application.Abstractions.Services.RabbitMQ;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LearningManagementSystem.Infrastructure.Services.RabbitMQService;

public class RabbitMQService(IConfiguration _configuration) : IRabbitMQService
{
    public async Task PublishMessageAsync<T>(T message, string queueName)
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("");
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(queueName, false, false, false, null);

        var messageJson = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(messageJson);

        await channel.BasicPublishAsync(
            exchange: string.Empty, // Default exchange
            routingKey: queueName, // Queue name as the routing key
            mandatory: false, // Not mandatory
            body: body);
    }

    public async Task GetMessage(string queueName)
    {
        var factory = new ConnectionFactory();
        factory.Uri = new Uri("");
        await using var connection = await factory.CreateConnectionAsync();
        await using var channel = await connection.CreateChannelAsync();
        await channel.QueueDeclareAsync(queueName, false, false, false, null);

        var consumer = new AsyncEventingBasicConsumer(channel);
       await channel.BasicConsumeAsync(queueName,true,consumer);
       consumer.ReceivedAsync += Consumer_Received;
    }

    private static async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
    {
        Console.WriteLine($"Received {Encoding.UTF8.GetString(e.Body.ToArray())}");
    }
}