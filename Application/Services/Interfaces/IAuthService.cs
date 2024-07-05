using Domain.Models.Authentications;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IActionResult> Authenticate(CertificateModel certificate);
        Task<IActionResult> GetInformation(Guid id);
        Task<AuthModel> GetUser(Guid id);
    }
}
