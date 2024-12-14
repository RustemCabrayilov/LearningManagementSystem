using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace LearningManagementSystem.Application.Extensions;

public static class ServiceRegister
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
    }
}