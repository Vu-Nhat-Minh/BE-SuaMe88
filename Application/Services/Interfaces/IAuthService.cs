using Domain.Models.Authentications;
using Microsoft.AspNetCore.Mvc;

namespace Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<IActionResult> CustomerAuthenticate(CertificateModel certificate);
        Task<IActionResult> GetCustomerInformation(Guid id);
        Task<AuthModel> GetUser(Guid id);
    }
}
