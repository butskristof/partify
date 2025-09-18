using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Client.WebIntegration;

namespace Web.Controllers;

[Route("[controller]")]
public class AuthController : Controller
{
    [HttpGet("login/spotify")]
    public IActionResult SpotifyLogin(string? returnUrl)
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri =
                !string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
                    ? returnUrl
                    : "/",
        };

        return Challenge(properties, OpenIddictClientWebIntegrationConstants.Providers.Spotify);
    }

    [HttpGet("callback/spotify")]
    public async Task<IActionResult> SpotifyCallback()
    {
        // var result = await HttpContext.AuthenticateAsync(
        //     OpenIddictClientAspNetCoreDefaults.AuthenticationScheme
        // );
        var result = await HttpContext.AuthenticateAsync(
            OpenIddictClientWebIntegrationConstants.Providers.Spotify
        );

        var identity = new ClaimsIdentity(
            authenticationType: OpenIddictClientWebIntegrationConstants.Providers.Spotify
        );

        identity
            .SetClaim(
                ClaimTypes.NameIdentifier,
                result.Principal!.GetClaim(ClaimTypes.NameIdentifier)
            )
            .SetClaim(ClaimTypes.Name, result.Principal!.GetClaim(ClaimTypes.Name));

        var properties = new AuthenticationProperties(result.Properties!.Items)
        {
            RedirectUri = result.Properties.RedirectUri ?? "/",
        };

        return SignIn(new ClaimsPrincipal(identity), properties);
    }
}
