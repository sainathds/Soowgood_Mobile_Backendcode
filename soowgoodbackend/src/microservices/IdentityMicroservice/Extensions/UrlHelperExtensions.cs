using IdentityMicroservice.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace IdentityMicroservice.Extensions
{
    public static class UrlHelperExtensions
    {
        public static string EmailConfirmationLink(this IUrlHelper urlHelper, string userId, string code, string scheme)
        {
            return urlHelper.Action(
                action: nameof(UsersController.ConfirmEmail),
                controller: "Users",
                values: new { userId, code },
                protocol: scheme);
        }
    }
}
