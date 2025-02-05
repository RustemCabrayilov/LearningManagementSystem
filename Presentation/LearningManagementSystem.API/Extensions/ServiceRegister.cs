using System.Security.Claims;
using System.Text;
using Amazon.S3;
using Elastic.Clients.Elasticsearch;
using LearningManagementSystem.API.ActionFilters;
using LearningManagementSystem.API.ExceptionHandlers;
using LearningManagementSystem.Application.Abstractions.Services.Token;
using LearningManagementSystem.BLL.Services.Auth;
using LearningManagementSystem.BLL.Services.Email;
using LearningManagementSystem.Domain.Entities.Identity;
using LearningManagementSystem.Infrastructure.Services.ElasticService;
using LearningManagementSystem.Infrastructure.Services.Storage.Aws;
using LearningManagementSystem.Persistence.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace LearningManagementSystem.API.Extensions;

public static class ServiceRegister
{
    public static void AddApiServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpContextAccessor();
        services.AddDbContext<AppDbContext>(opts
            => opts.UseSqlServer(configuration.GetConnectionString("SqlConnectionString")));
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = configuration["Redis:RedisConnectionString"];
            options.InstanceName = configuration["Redis:RedisInstanceName"];
        });
        services.AddIdentity<AppUser, AppRole>(options =>
        {
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
        }).AddEntityFrameworkStores<AppDbContext>();
        services.Configure<TokenSettings>(configuration.GetSection("Token"));
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<ElasticSettings>(configuration.GetSection("ElasticSettings"));
        services.AddSingleton(provider =>
        {
            var settings = provider.GetRequiredService<IOptions<ElasticSettings>>().Value;
            var elasticsearchSettings = new ElasticsearchClientSettings(new Uri(settings.Url))
                .DefaultIndex(settings.DefaultIndex);
            return new ElasticsearchClient(elasticsearchSettings);
        });
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Token:Issuer"],
                    ValidAudience = configuration["Token:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:SecurityKey"])),
                    RoleClaimType = ClaimTypes.Role 
                };
            });
        services.AddScoped(typeof(EntityExistFilter<>));
        services.AddScoped(typeof(UserExistFilter));
        services.AddScoped(typeof(RoleExistFilter));
        services.AddScoped(typeof(ValidationFilter<>));
        services.AddExceptionHandler<BadRequestExceptionHandler>();
        services.AddExceptionHandler<NotFoundExceptionHandler>();
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.Configure<S3Settings>(configuration.GetSection("S3Settings"));
        services.Configure<AdminSettings>(configuration.GetSection("AdminSettings"));
        /*services.AddSingleton<IAmazonS3>(sp =>
        {
            var s3Settings = sp.GetRequiredService<IOptions<S3Settings>>().Value;
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.GetBySystemName(s3Settings.Region)
            };

            return new AmazonS3Client(config);
        });*/
        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend",
                builder =>
                {
                    builder
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithOrigins("http://localhost:5052","https://localhost:5052");
                });
        });
        services.AddDefaultAWSOptions(configuration.GetAWSOptions());
        services.AddAWSService<IAmazonS3>();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

            // Add the security definition for Bearer authentication
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
             
            // Add a security requirement to include the Bearer token globally
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });
        services.AddHttpContextAccessor();
    }
}