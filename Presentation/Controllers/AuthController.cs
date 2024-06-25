using Application.Services.Interfaces;
using Common.Extensions;
using Domain.Constants;
using Domain.Models.Authentications;
using Infrastructure.Configurations;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        [Route("customers")]
        public async Task<IActionResult> CustomerAuthenticate([FromBody] CertificateModel certificate)
        {
            try
            {
                return await _authService.CustomerAuthenticate(certificate);
            }
            catch (Exception e)
            {
                return e.Message.InternalServerError();
            }
        }

        [HttpPost]
        [Route("customers/sign-in-with-token")]
        [Authorize(UserRoles.CUSTOMER)]
        public async Task<IActionResult> GetAdminInformation()
        {
            try
            {
                var auth = this.GetAuthenticatedUser();
                return await _authService.GetCustomerInformation(auth.Id);
            }
            catch (Exception e)
            {
                return e.Message.InternalServerError();
            }
        }
    }
}
