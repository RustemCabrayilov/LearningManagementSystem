using System.Text.Json;
using LearningManagementSystem.API.Extensions;
using LearningManagementSystem.Application.Abstractions.Repository;
using LearningManagementSystem.Application.Exceptions;
using LearningManagementSystem.Domain.Entities.Common;
using LearningManagementSystem.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearningManagementSystem.API.ActionFilters;

public class UserExistFilter(
    UserManager<AppUser> _userManager,
    ILogger<UserExistFilter> _logger) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var id = String.Empty;
        if (!context.ActionArguments.ContainsKey("id")) throw new BadHttpRequestException("Id not provided");
        id = (string)(context.ActionArguments["id"]);
        var entity = await _userManager.FindByIdAsync(id);
        if (entity is null) throw new NotFoundException($"This {id} entity not found.");
        context.HttpContext.Items["entity"] = entity;
        _logger.LogInformation("Before action execution with Url: " + context.HttpContext.Request.Path);

        var executedContext = await next();

        var response=  executedContext.Result?.ExtractObject();
        _logger.LogInformation("Action executed with Response: {Response}", response);

        _logger.LogInformation("After action execution with statuscode: " + context.HttpContext.Response.StatusCode);
    }
}