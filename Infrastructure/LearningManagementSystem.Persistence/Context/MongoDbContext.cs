using System.Reflection.Metadata;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Document = LearningManagementSystem.Domain.Entities.Document;

namespace LearningManagementSystem.Persistence.Context;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var client = new MongoClient(configuration.GetConnectionString("MongoDb"));
        _database = client.GetDatabase("YourDatabaseName"); // Set your database name
    }

    public IMongoCollection<Document> FileDocuments => _database.GetCollection<Document>("Documents");   
}