using System.Net.Http.Headers;
using System.Net.Http.Json;
using LearningManagementSystem.Application.Abstractions.Services.GeminiAI;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LearningManagementSystem.Infrastructure.Services.GeminiAIService;

public class GeminiAIService(IConfiguration _configuration,
    HttpClient _httpClient):IGeminiAIService
{
    public async ValueTask<string> GenerateTextAsync(string prompt)
    {
      
        var url = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_configuration["GeminiAI:ApiKey"]}";

        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = prompt } } }
            }
        };

        var request = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = JsonContent.Create(requestBody, MediaTypeHeaderValue.Parse("application/json"))
        };

        var response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode) throw new Exception($"Error: {response.StatusCode} - {response.ReasonPhrase}");

        var responseBody = await response.Content.ReadAsStringAsync();

        // Parse the response JSON to extract the text
        var json = JObject.Parse(responseBody);
        var text = json["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString();

        // Return the extracted text or throw an error if null
        if (string.IsNullOrEmpty(text))
            throw new Exception("Generated text is null or empty in the response.");
        
        return text;
    }
}

public class GeminiResponse
{
}
