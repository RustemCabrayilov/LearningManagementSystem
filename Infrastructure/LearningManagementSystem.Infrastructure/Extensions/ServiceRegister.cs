using Hangfire;
using LearningManagementSystem.Application.Abstractions.Services.BackgroundJob;
using LearningManagementSystem.Application.Abstractions.Services.ElasticService;
using LearningManagementSystem.Application.Abstractions.Services.GeminiAI;
using LearningManagementSystem.Application.Abstractions.Services.GoogleMeet;
using LearningManagementSystem.Application.Abstractions.Services.OCR;
using LearningManagementSystem.Application.Abstractions.Services.QRCode;
using LearningManagementSystem.Application.Abstractions.Services.Redis;
using LearningManagementSystem.Application.Abstractions.Services.Storage;
using LearningManagementSystem.Application.Abstractions.Services.Storage.Aws;
using LearningManagementSystem.Application.Abstractions.Services.Token;
using LearningManagementSystem.Infrastructure.Services.BackgroundJob;
using LearningManagementSystem.Infrastructure.Services.ElasticService;
using LearningManagementSystem.Infrastructure.Services.GeminiAIService;
using LearningManagementSystem.Infrastructure.Services.GoogleMeet;
using LearningManagementSystem.Infrastructure.Services.OCR;
using LearningManagementSystem.Infrastructure.Services.QrCode;
using LearningManagementSystem.Infrastructure.Services.Redis;
using LearningManagementSystem.Infrastructure.Services.Storage.Aws;
using LearningManagementSystem.Infrastructure.Services.Token;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearningManagementSystem.Infrastructure.Extensions;

public static class ServiceRegister
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient("GeminiAPI", client =>
        {
            client.BaseAddress = new Uri(configuration["GeminiAI:BaseUrl"]);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {configuration["GeminiAI:ApiKey"]}");
        });
        services.AddHangfire(config =>
        {
            config.UseSqlServerStorage(configuration.GetConnectionString("SqlConnectionString"));
        });
        services.AddHangfireServer();
        services.AddScoped<ITokenHandler, TokenHandler>();
        services.AddSingleton<IElasticService, ElasticService>();
        services.AddScoped<IOCRService, OCRService>();
        services.AddScoped<IGeminiAIService, GeminiAIService>();
        services.AddScoped<IQRCodeService, QRCodeService>();
        services.AddScoped<IBackgroundJobService, BackgroundJobService>();
        services.AddScoped<IGoogleMeetService, GoogleMeetService>();
        services.AddScoped<IRedisCachingService,RedisCachingService >();
        services.AddScoped<IAwsStorage, AwsStorage>();
    }
    public static void AddStorage<T>(this IServiceCollection serviceCollection) where T : class, IStorage
    {
        serviceCollection.AddScoped<IStorage, T>();
    }
}