using System.Text.Json;
using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearningManagementSystem.API.ActionFilters;

public class EntityExistFilter<T> : Attribute, IAsyncActionFilter where T : BaseEntity, new()
{
    private readonly IGenericRepository<T> _repository;
    private readonly ILogger<EntityExistFilter<T>> _logger;

    public EntityExistFilter(
        IGenericRepository<T> repository,
        ILogger<EntityExistFilter<T>> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var id = Guid.Empty;
        if (!context.ActionArguments.ContainsKey("id")) throw new BadHttpRequestException("Id not provided");
        id = (Guid)(context.ActionArguments["id"]);
        var entity = await _repository.GetAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) throw new NotFoundException($"This {id} entity not found.");
        context.HttpContext.Items["entity"] = entity;
        _logger.LogInformation("Before action execution with Url: " + context.HttpContext.Request.Path);

        var executedContext = await next();
   
        var response=  executedContext.Result?.ExtractObject();
        
        _logger.LogInformation("Action executed with Response: {Response}", response);

        _logger.LogInformation("After action execution with statuscode: " + context.HttpContext.Response.StatusCode);
    }
}