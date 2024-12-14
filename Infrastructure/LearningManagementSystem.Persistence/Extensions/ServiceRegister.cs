using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Abstractions.UnitOfWork;
using LearningManagementSystem.Persistence.Context;
using LearningManagementSystem.Persistence.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LearningManagementSystem.Persistence.Extensions;

public static class ServiceRegister
{
 public static void AddPersistenceServices(this IServiceCollection services,IConfiguration configuration)
 {
  services.AddDbContext<AppDbContext>(opts =>
   opts.UseSqlServer(configuration.GetConnectionString("SqlConnectionString"))
  );
   
  services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
  services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
 }   
}