using Microsoft.AspNetCore.Http;

namespace LearningManagementSystem.Application.Abstractions.Services.Aws;

public record FileRequest
(
    IFormFile File,
    string FileName,
    string Prefix
    
);