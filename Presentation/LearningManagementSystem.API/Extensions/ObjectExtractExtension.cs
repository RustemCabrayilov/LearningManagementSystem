using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace LearningManagementSystem.API.Extensions;

public static class ObjectExtractExtension
{
    public static string ExtractObject(this IActionResult result)
    {
        if (result is ObjectResult objectResult)
        {
            var extractedObject = objectResult.Value;
            var serializedResponse = JsonSerializer.Serialize(extractedObject, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            return serializedResponse;
        }

        return "{}";
    }
}