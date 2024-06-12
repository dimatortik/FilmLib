using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace FilmLib.Infrastructure.CloudStorage;

public interface ICloudStorageService
{
    public Task<Result<string>> UploadFileAsync(IFormFile file, string keyName);
    public Task<Result<string>> GetFileUrlAsync(string keyName);
        
    public Task<Result<bool>> IsFileExistsAsync(string keyName);

    public Task<Result<bool>> DeleteFileAsync(string keyName);
}