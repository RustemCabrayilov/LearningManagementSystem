using FluentValidation;
using LearningManagementSystem.API.ExceptionHandlers;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.API.Extensions;

public static class ServiceRegister
{
    public static void AddApiServices(this IServiceCollection services,IConfiguration configuration)
    { 
        services.AddHttpContextAccessor();
        services.AddDbContext<AppDbContext>(opts
            => opts.UseSqlServer(configuration.GetConnectionString("SqlConnectionString")));
        	services.AddIdentity<AppUser, AppRole>(options =>
    {
        options.Password.RequiredLength = 3;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireUppercase = false;
    }).AddEntityFrameworkStores<AppDbContext>();
        /*services.AddScoped(typeof(IdFilter<>));
        services.AddScoped(typeof(ValidationFilter<>));*/
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        /*ValidatorOptions.Global.LanguageManager.Culture = new System.Globalization.CultureInfo("az");*/
    }
}