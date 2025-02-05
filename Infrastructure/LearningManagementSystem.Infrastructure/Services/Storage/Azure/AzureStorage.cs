using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using LearningManagementSystem.Application.Abstractions.Services.Aws;
using LearningManagementSystem.Application.Abstractions.Services.Storage.Azure;
using LearningManagementSystem.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace LearningManagementSystem.BLL.Services.Storage.Azure;

public class AzureStorage : IAzureStorage
{
    readonly BlobServiceClient _blobServiceClient;
    BlobContainerClient _blobContainerClient;

    public AzureStorage(IConfiguration configuration)
    {
        _blobServiceClient = new(configuration["Storage:Azure"]);
    }

    public async ValueTask<bool> DeleteFileAsync(string containerName, string fileName)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(containerName);
        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
        var response = await blobClient.DeleteAsync();
        return response.IsError;
    }

    public async ValueTask<string> GetFileUrlAsync(string fileName, string containerName)
    {
        BlobClient blobClient = _blobContainerClient.GetBlobClient(fileName);
        if (!blobClient.Exists())
        {
            throw new NotFoundException("File not Found");
        }

        return blobClient.Uri.AbsoluteUri;
    }

    public async ValueTask<string> UploadFileAsync(FileRequest request)
    {
        _blobContainerClient = _blobServiceClient.GetBlobContainerClient(request.Prefix);
        await _blobContainerClient.CreateIfNotExistsAsync();
        await _blobContainerClient.SetAccessPolicyAsync(PublicAccessType.BlobContainer);
        BlobClient blobClient = _blobContainerClient.GetBlobClient(request.FileName);
        await blobClient.UploadAsync(request.File.OpenReadStream());
        if (!blobClient.Exists())
        {
            throw new NotFoundException("File not Found");
        }

        return blobClient.Uri.AbsoluteUri;
    }

    public async ValueTask<string> UpdateFileAsync(FileRequest request, string fileName)
    {
        throw new NotImplementedException();
    }
}