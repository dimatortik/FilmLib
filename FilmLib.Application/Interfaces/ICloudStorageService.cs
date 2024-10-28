using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Http;

namespace FilmLib.Application.Interfaces;

public interface ICloudStorageService
{
    public Task<Result<string>> UploadFileAsync(IFormFile file, string keyName);
    public Task<Result<string>> GetFileUrlAsync(string keyName);
        
    public Task<Result<bool>> IsFileExistsAsync(string keyName);
    
}