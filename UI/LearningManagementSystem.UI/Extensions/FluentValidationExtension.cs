using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Refit;

namespace LearningManagementSystem.UI.Extensions;

public static class FluentValidationExtension
{
    public static void AddValidationError(this ModelStateDictionary modelState, ValidationApiException exception)
    {
        foreach (var error in exception.Content.Errors)
        {
            modelState.AddModelError(error.Key, error.Value[0]);
        }
    }
}