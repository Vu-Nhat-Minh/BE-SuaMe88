using Microsoft.AspNetCore.Http;

namespace Application.Services.Interfaces
{
    public interface ICloudStorageService
    {
        Task<string> Upload(Guid id, IFormFile file);
    }
}
