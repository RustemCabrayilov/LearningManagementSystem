using System.Text.Json;
using LearningManagementSystem.Application.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using LearningManagementSystem.API.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LearningManagementSystem.API.ActionFilters;

public class ValidationFilter<T> : Attribute, IAsyncActionFilter
{
    private readonly ILogger<ValidationFilter<T>> _logger;
    private readonly IValidator<T> _validator;

    public ValidationFilter(
        ILogger<ValidationFilter<T>> logger,
        IValidator<T> validator)
    {
        _logger = logger;
        _validator = validator;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        _logger.LogInformation("Before action execution with Url: " + context.HttpContext.Request.Path);
        if (context.ActionArguments.TryGetValue("dto", out var model) && model is T typedModel)
        {
            ValidationResult validationResult = await _validator.ValidateAsync(typedModel);
            if (typedModel is ObjectResult objectResult)
            {
                var requestObject = objectResult.Value;
                var serializedResponse = JsonSerializer.Serialize(requestObject, new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                _logger.LogInformation("Action executing with Request: {Request}", serializedResponse);
            }
            if (!validationResult.IsValid)
            {
                foreach (var error in validationResult.Errors)
                {
                    _logger.LogError("Validation error: {Error}", error.ErrorMessage);
                }

                throw new BadRequestException(validationResult.Errors[0].ErrorMessage);
            }
        }

        var executedContext = await next();
       var response=  executedContext.Result?.ExtractObject();
       _logger.LogInformation("Action executed with Response: {Response}", response);
       

        _logger.LogInformation("After action execution with statuscode: " + context.HttpContext.Response.StatusCode);
    }
}