using Amazon.S3;
using Amazon.S3.Model;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace FilmLib.Infrastructure.CloudStorage;
public class CloudStorageService(IAmazonS3 s3Client) : ICloudStorageService
{
    private readonly string _bucketName = "film-lib-pet";

    public async Task<Result<string>> UploadFileAsync(IFormFile file, string keyName)
    {
        if (file == null)
            return Result.Success(string.Empty);
        
        if (s3Client == null)
        {
            return Result.Failure<string>("s3Client is not initialized.");
        }
        var fileExists = await IsFileExistsAsync(keyName);
        if (fileExists.IsSuccess && fileExists.Value)
        {
            return Result.Failure<string>("File with this name already exists.");
        }

        var rawKey = $"{keyName}.{file.ContentType.Split('/')[1]}";
        var key = rawKey
            .Replace(" ", "%20");
        var filepath = file.ContentType.StartsWith("video/") ? "films" : "images";
        using var stream = new MemoryStream();
        await file.CopyToAsync(stream);

        var putRequest = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = $"{filepath}/{rawKey}",
            InputStream = stream,
            ContentType = file.ContentType,
            CannedACL = S3CannedACL.PublicRead
        };
        
        if (putRequest == null)
        {
            return Result.Failure<string>("Failed to create put request.");
        }

        var response = await s3Client.PutObjectAsync(putRequest);

        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            return Result.Failure<string>($"Failed to upload file.\n{response.ResponseMetadata.Metadata}");
        }
        
        
        
        return Result.Success($"https://{_bucketName}.fra1.digitaloceanspaces.com/{filepath}/{key}");
    }

    public async Task<Result<string>> GetFileUrlAsync(string keyName)
    {
        var getRequest = new GetObjectRequest
        {
            BucketName = _bucketName,
            Key = keyName
        };
    
        var response = await s3Client.GetObjectAsync(getRequest);
    
        if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
        {
            return Result.Failure<string>($"Failed to get file URL.\n{response.ResponseMetadata.Metadata}");
        }
    
        return Result.Success("ok");
    }
    
    public async Task<Result<bool>> IsFileExistsAsync(string keyName)
    {
        try
        {
            var metadataRequest = new GetObjectMetadataRequest
            {
                BucketName = _bucketName,
                Key = keyName
            };
            var response = await s3Client.GetObjectMetadataAsync(metadataRequest);
        }
        catch (AmazonS3Exception e)
        {
            if (e.ErrorCode == "NoSuchKey" || e.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }
            throw;
        }
        return true;
    }
    
    public async Task<Result<bool>> DeleteFileAsync(string keyName)
    {
        try
        {
            var deleteRequest = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = keyName
            };

            var response = await s3Client.DeleteObjectAsync(deleteRequest);

            return response.HttpStatusCode == System.Net.HttpStatusCode.NoContent;
        }
        catch (AmazonS3Exception e)
        {
            return false;
        }
    } 
}
