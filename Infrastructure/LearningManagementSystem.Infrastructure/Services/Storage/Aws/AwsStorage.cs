using Amazon.S3;
using Amazon.S3.Model;
using LearningManagementSystem.Application.Abstractions.Services.Aws;
using LearningManagementSystem.Application.Abstractions.Services.Storage.Aws;
using LearningManagementSystem.Application.Exceptions;
using Microsoft.Extensions.Options;

namespace LearningManagementSystem.Infrastructure.Services.Storage.Aws;

public class AwsStorage:IAwsStorage
{
    private readonly S3Settings _s3Settings;
    private readonly IAmazonS3 _s3Client;
    public AwsStorage(IOptions<S3Settings> settings)
    {
        _s3Settings = settings.Value;
        var config = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_s3Settings.Region) // Set the correct region
        };
        _s3Client =  new AmazonS3Client("AKIARYEUCU47FKZ2WDJO", "xPTffM8W5JuTRsTyGXbfMzibMnGqgwZBOkNNgon1",config);;
        
    }
    
    public async ValueTask<string> GetFileUrlAsync(string key,string prefix)
    {
        /*var request = new GetPreSignedUrlRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = $"images/{key}",
            Verb = HttpVerb.GET,
            Expires = DateTime.Now.AddMinutes(15),
        };

        string preSignedUrl = _s3Client.GetPreSignedURL(request);
        return preSignedUrl;*/
        var decodedKey = Uri.UnescapeDataString(key);
        var config = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_s3Settings.Region) // Set the correct region
        };
        var s3client = new AmazonS3Client("AKIARYEUCU47FKZ2WDJO", "xPTffM8W5JuTRsTyGXbfMzibMnGqgwZBOkNNgon1",config);
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3client, _s3Settings.BucketName);
        if (!bucketExists) throw new NotFoundException($"Bucket {_s3Settings.BucketName} does not exist.");
        var request = new ListObjectsV2Request()
        {
            BucketName = _s3Settings.BucketName,
            Prefix = prefix
        };
        var result = await s3client.ListObjectsV2Async(request);
        var s3Objects = result.S3Objects.Select(s =>
        {
            var urlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = _s3Settings.BucketName,
                Key = s.Key,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };
            return new
            {
                Name = s.Key.ToString(),
                PresignedUrl = s3client.GetPreSignedURL(urlRequest),
            };
        });
        return s3Objects?.FirstOrDefault(x=>x.Name==decodedKey).PresignedUrl;
    }

    public async ValueTask<string> UploadFileAsync(FileRequest dto)
    {

        try
        {
            {
                var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(_s3Client,_s3Settings.BucketName);
                if (!bucketExists) throw new NotFoundException($"Bucket {_s3Settings.BucketName} does not exist.");
                var request = new PutObjectRequest()
                {
                    BucketName =_s3Settings.BucketName,
                    Key = string.IsNullOrEmpty(dto.Prefix) ? dto.FileName : $"{dto.Prefix?.TrimEnd('/')}/{dto.FileName}",
                    InputStream = dto.File.OpenReadStream()
                };
                request.Metadata.Add("Content-Type", dto.File.ContentType);
                await _s3Client.PutObjectAsync(request);
                return request.Key;
            }
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
    

    public async ValueTask<string> UpdateFileAsync(FileRequest dto, string key)
    {
        var decodedKey = Uri.UnescapeDataString(key);
    
        // Create S3 client with config
        var config = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_s3Settings.Region) // Set the correct region
        };
        var s3client = new AmazonS3Client("AKIARYEUCU47FKZ2WDJO", "xPTffM8W5JuTRsTyGXbfMzibMnGqgwZBOkNNgon1", config);

        // Check if bucket exists
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3client, _s3Settings.BucketName);
        if (!bucketExists) throw new NotFoundException($"Bucket {_s3Settings.BucketName} does not exist.");

        // Check if the file exists in S3
        var fileExists = await GetFileUrlAsync(decodedKey,dto.Prefix);
        if (string.IsNullOrEmpty(fileExists))
        {
            throw new NotFoundException("File Not Found to Update");
        }
    
        // Upload the new file (same logic as in UploadFileUrl method)
        var uploadResult = await UploadFileAsync(dto);
    
        return uploadResult;
    }
    
    public async ValueTask<bool> DeleteFileAsync(string key)
    {
        var config = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_s3Settings.Region) // Set the correct region
        };
        var s3Client = new AmazonS3Client("AKIARYEUCU47FKZ2WDJO", "xPTffM8W5JuTRsTyGXbfMzibMnGqgwZBOkNNgon1", config);
        var deleteRequest = new DeleteObjectRequest
        {
            BucketName = _s3Settings.BucketName,
            Key = key,
        };
     var result=await s3Client.DeleteObjectAsync(deleteRequest);
        return result.HttpStatusCode == System.Net.HttpStatusCode.OK;
    }

    public async Task<IEnumerable<object>> GetAllFilesAsync(string bucketName, string? prefix)
    {
        var config = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_s3Settings.Region) // Set the correct region
        };
        var s3client = new AmazonS3Client("AKIARYEUCU47FKZ2WDJO", "xPTffM8W5JuTRsTyGXbfMzibMnGqgwZBOkNNgon1", config);
        var bucketExists = await Amazon.S3.Util.AmazonS3Util.DoesS3BucketExistV2Async(s3client, bucketName);
        if (!bucketExists) throw new NotFoundException($"Bucket {bucketName} does not exist.");
        var request = new ListObjectsV2Request()
        {
            BucketName = bucketName,
            Prefix = prefix
        };
        var result = await s3client.ListObjectsV2Async(request);
        var s3Objects = result.S3Objects.Select(s =>
        {
            var urlRequest = new GetPreSignedUrlRequest()
            {
                BucketName = bucketName,
                Key = s.Key,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };
            return new
            {
                Name = s.Key.ToString(),
                PresignedUrl = s3client.GetPreSignedURL(urlRequest),
            };
        });
        return s3Objects;
    }

    public async Task<List<string>> GetAllBucketsAsync()
    {
        var config = new AmazonS3Config
        {
            RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(_s3Settings.Region) // Set the correct region
        };
        var s3client = new AmazonS3Client("AKIARYEUCU47FKZ2WDJO", "xPTffM8W5JuTRsTyGXbfMzibMnGqgwZBOkNNgon1", config);
        var data = await s3client.ListBucketsAsync();
        var buckets = data.Buckets.Select(x => x.BucketName).ToList();
        return buckets;
    }
}