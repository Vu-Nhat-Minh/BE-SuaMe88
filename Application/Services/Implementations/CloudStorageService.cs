using Application.Services.Interfaces;
using Application.Settings;
using Common.Helpers;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Application.Services.Implementations;

public class CloudStorageService : ICloudStorageService
{
    private static readonly StorageClient Storage;
    private readonly AppSettings _settings;

    static CloudStorageService()
    {
        Storage = CloudStorageHelper.GetStorage();
    }

    public CloudStorageService(IOptions<AppSettings> settings)
    {
        _settings = settings.Value;
    }

    public async Task<string> Upload(Guid id, IFormFile file)
    {
        try
        {
            var fileName = id + Path.GetExtension(file.FileName);

            await Storage.UploadObjectAsync(
                _settings.Bucket,
                $"{_settings.DefaultFolder}/{fileName}",
                Path.GetExtension(file.FileName),
                file.OpenReadStream(),
                null,
                CancellationToken.None);
            var domain = "https://firebasestorage.googleapis.com/";
            var bucket = "v0/b/happy-milk-1b780.appspot.com";
            var filePath = $"/o/{_settings.DefaultFolder}%2F{ fileName}?alt=media";
            return domain + bucket + filePath;
        }
        catch
        {
            throw;
        }
    }
}