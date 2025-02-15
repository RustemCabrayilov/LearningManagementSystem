using Hangfire;
using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Application.Abstractions.Services.BackgroundJob;
using LearningManagementSystem.Application.Extensions;
using LearningManagementSystem.BLL.Extensions;
using LearningManagementSystem.Infrastructure.Extensions;
using LearningManagementSystem.Infrastructure.Services.Storage.Aws;
using LearningManagementSystem.Persistence.Extensions;
using LearningManagementSystem.SignalR.Extensions;
using Microsoft.Extensions.FileProviders;

namespace LearningManagementSystem.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
           
            builder.Services.AddApplicationServices();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddBusinessLogicServices();
            builder.Services.AddPersistenceServices(builder.Configuration);
            builder.Logging.AddCustomSerilog();
            builder.Services.AddApiServices(builder.Configuration);
            builder.Services.AddSignalRServices();
            builder.Services.AddStorage<AwsStorage>();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHangfireDashboard();
            RecurringJob.AddOrUpdate<IBackgroundJobService>(
                "recommend-teacher-job",
                service => service.Recommendteacher(),
                Cron.Minutely());
            RecurringJob.AddOrUpdate<IBackgroundJobService>(
                "average-of-student-job",
                service => service.AveragOfStudent(),
                Cron.Minutely());
            RecurringJob.AddOrUpdate<IBackgroundJobService>(
                "fail-notification-job",
                service => service.FailNotification(),
                "*/2 * * * *" // Runs every hour
            );
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Documents")),
                RequestPath = "/Documents"  // This sets the URL path for the documents
            }); 
            app.UseStaticFiles();
            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors("AllowFrontend");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseExceptionHandler(_ => { });

            app.MapControllers();
            
            app.MapHubs();
            app.Run();
        }
    }
}