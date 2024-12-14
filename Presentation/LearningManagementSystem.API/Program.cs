using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Application.Extensions;
using LearningManagementSystem.BLL.Extensions;
using LearningManagementSystem.Persistence.Extensions;

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
            builder.Services.AddSwaggerGen();
            builder.Services.AddApplicationServices();
            builder.Services.AddBusinessLogicServices();
            builder.Services.AddPersistenceServices(builder.Configuration);
            /*builder.Services.AddInfrastructureServices();*/
            /*builder.Logging.AddCustomSerilog();*/
            builder.Services.AddApiServices(builder.Configuration);
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            
            app.UseExceptionHandler(_ => { });
            
            app.MapControllers();

            app.Run();
        }
    }
}