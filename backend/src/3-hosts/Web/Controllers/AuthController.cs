using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Client.WebIntegration;
using Partify.Domain.ValueTypes;
using Partify.Infrastructure.Spotify;

namespace Partify.Web.Controllers;

[Route("[controller]")]
public sealed class AuthController : Controller
{
    private readonly ILogger<AuthController> _logger;

    public AuthController(ILogger<AuthController> logger)
    {
        _logger = logger;
    }

    [HttpGet("login")]
    public IActionResult Login([FromQuery] string? returnUrl)
    {
        _logger.LogInformation("User started login");
        return RedirectToAction(nameof(SpotifyLogin), new { returnUrl });
    }

    [HttpGet("login/spotify")]
    public IActionResult SpotifyLogin([FromQuery] string? returnUrl)
    {
        _logger.LogInformation("User started login with Spotify");

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
    public async Task<IActionResult> SpotifyCallback(
        [FromServices] ISpotifyTokensService spotifyTokensService,
        [FromServices] TimeProvider timeProvider
    )
    {
        _logger.LogInformation("Callback for Spotify authentication received");
        try
        {
            var result = await HttpContext.AuthenticateAsync(
                OpenIddictClientWebIntegrationConstants.Providers.Spotify
            );

            if (result.Principal is null)
            {
                _logger.LogWarning("Spotify authentication failed: No principal returned");
                return Redirect("/auth/error?type=authentication_failed");
            }

            if (!result.Succeeded)
            {
                _logger.LogWarning(
                    "Spotify authentication failed: {Error}",
                    result.Failure?.Message
                );
                return Redirect("/auth/error?type=authentication_failed");
            }

            var nameIdentifier = result.Principal.GetClaim(ClaimTypes.NameIdentifier);
            var name = result.Principal.GetClaim(ClaimTypes.Name);

            if (string.IsNullOrEmpty(nameIdentifier))
            {
                _logger.LogWarning(
                    "Spotify authentication failed: Missing required NameIdentifier claim"
                );
                return Redirect("/auth/error?type=missing_claims");
            }

            if (string.IsNullOrEmpty(name))
            {
                _logger.LogWarning("Spotify authentication failed: Missing required Name claim");
                return Redirect("/auth/error?type=missing_claims");
            }

            _logger.LogInformation(
                "Spotify authentication successful for user: {UserId}",
                nameIdentifier
            );

            try
            {
                var accessToken = result.Properties.GetTokenValue(
                    OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken
                );

                if (string.IsNullOrEmpty(accessToken))
                {
                    _logger.LogError(
                        "Failed to retrieve access token from Spotify for user {UserId}",
                        nameIdentifier
                    );
                    return Redirect("/auth/error?type=token_retrieval_failed");
                }

                var accessTokenExpirationDate = result.Properties.GetTokenValue(
                    OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessTokenExpirationDate
                );

                if (
                    !DateTimeOffset.TryParse(
                        accessTokenExpirationDate,
                        out var accessTokenExpiresOn
                    )
                )
                {
                    _logger.LogWarning(
                        "Failed to parse access token expiration date for user {UserId}, using default",
                        nameIdentifier
                    );
                    // we expect the token to be valid for an hour, let's subtract 1 minute to account for network
                    // latency between issuing the token and receiving it
                    accessTokenExpiresOn = timeProvider.GetUtcNow().AddMinutes(59);
                }

                var refreshToken = result.Properties.GetTokenValue(
                    OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken
                );

                await spotifyTokensService.UpdateSpotifyTokens(
                    new SpotifyId(nameIdentifier),
                    accessToken,
                    accessTokenExpiresOn,
                    refreshToken
                );

                _logger.LogInformation(
                    "Successfully persisted Spotify tokens for user {UserId}",
                    nameIdentifier
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to handle Spotify tokens for user {UserId}",
                    nameIdentifier
                );
                return Redirect("/auth/error?type=token_persistence_failed");
            }

            var identity = new ClaimsIdentity(
                authenticationType: CookieAuthenticationDefaults.AuthenticationScheme
            );

            identity
                .SetClaim(ClaimTypes.NameIdentifier, nameIdentifier)
                .SetClaim(ClaimTypes.Name, name);

            var properties = new AuthenticationProperties(
                result.Properties?.Items ?? new Dictionary<string, string?>()
            )
            {
                RedirectUri = result.Properties?.RedirectUri ?? "/",
            };

            return SignIn(
                new ClaimsPrincipal(identity),
                properties,
                CookieAuthenticationDefaults.AuthenticationScheme
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during Spotify authentication callback");
            return Redirect("/auth/error?type=unexpected_error");
        }
    }

    [HttpGet("error")]
    public IActionResult Error([FromQuery] string? type)
    {
        _logger.LogInformation("Authentication error page accessed with type: {ErrorType}", type);

        // For now, return a simple response - you'll want to create a proper error view
        var errorMessage = type switch
        {
            "authentication_failed" => "Authentication with Spotify failed. Please try again.",
            "missing_claims" => "Unable to retrieve required user information from Spotify.",
            "token_retrieval_failed" => "Failed to retrieve access tokens from Spotify.",
            "token_persistence_failed" => "Failed to save authentication tokens.",
            "unexpected_error" => "An unexpected error occurred during authentication.",
            _ => "An authentication error occurred.",
        };

        // TODO: Return a proper error view/page
        // For now, just return a simple response
        return Content(
            $"Authentication Error: {errorMessage}. <a href=\"/auth/login\">Try again</a>"
        );
    }

    [HttpGet("logout")]
    public IActionResult Logout()
    {
        _logger.LogInformation("User logging out");

        // Sign out from both the local cookie and the external provider (Spotify)
        var authProperties = new AuthenticationProperties { RedirectUri = "/" };

        // Sign out from both the local cookie and the external provider (Spotify)
        return SignOut(
            authProperties,
            CookieAuthenticationDefaults.AuthenticationScheme,
            OpenIddictClientWebIntegrationConstants.Providers.Spotify
        );
    }
}
