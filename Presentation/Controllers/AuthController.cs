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
        public async Task<IActionResult> Authenticate([FromBody] CertificateModel certificate)
        {
            try
            {
                return await _authService.Authenticate(certificate);
            }
            catch (Exception e)
            {
                return e.Message.InternalServerError();
            }
        }

        [HttpPost]
        [Route("sign-in-with-token")]
        [Authorize]
        public async Task<IActionResult> GetInformation()
        {
            try
            {
                var auth = this.GetAuthenticatedUser();
                return await _authService.GetInformation(auth.Id);
            }
            catch (Exception e)
            {
                return e.Message.InternalServerError();
            }
        }
    }
}
