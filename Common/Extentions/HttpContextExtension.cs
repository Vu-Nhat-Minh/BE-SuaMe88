using Domain.Models.Authentications;
using Microsoft.AspNetCore.Mvc;

namespace Common.Extensions
{
    public static class HttpContextExtension
    {

        public static AuthModel GetAuthenticatedUser(this ControllerBase controller)
        {
            try
            {
                if (controller.HttpContext.Items.TryGetValue("USER", out var userObject))
                {
                    return (AuthModel)userObject!;
                }

                return null!;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
