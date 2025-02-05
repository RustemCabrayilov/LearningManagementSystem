using Amazon.S3;
using Amazon.S3.Model;
using LearningManagementSystem.Application.Abstractions.Services.Aws;
using LearningManagementSystem.Application.Abstractions.Services.Storage;
using LearningManagementSystem.Application.Abstractions.Services.Storage.Aws;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LearningManagementSystem.API.Controller;

[ApiController]
[Route("api/[controller]")]
public class AwsController : ControllerBase
{
    private readonly IStorage _storage;
    private readonly IAwsStorage _awsStorage;

    public AwsController(
        IStorage storage, IAwsStorage awsStorage)
    {
        _storage = storage;
        _awsStorage = awsStorage;
    }

    [HttpGet("{prefix}/{key}")]
    public async Task<IActionResult> GetFileUrl(string key, string prefix)
    {
        var response = await _awsStorage.GetFileUrlAsync(key, prefix);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> UploadFile(FileRequest request)
    {
        var response = await _awsStorage.UploadFileAsync(request);
        return Ok(response);
    }

    [HttpDelete]
    public async Task<IActionResult> GetAllBuckets()
    {
        var response=await _awsStorage.GetAllBucketsAsync();
        return Ok(response);
    }

    [HttpPost("files")]
    public async Task<IActionResult> GetAllFiles(string bucketName, string? prefix)
    {
        var response = await _awsStorage.GetAllFilesAsync(bucketName, prefix);
        return Ok(response);
    }
}