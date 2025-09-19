using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using OpenIddict.Client.AspNetCore;
using SpotifyAPI.Web;

namespace Web.Services;

public class SpotifyClientService : ISpotifyClientService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IConfiguration _configuration;

    public SpotifyClientService(IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
    {
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
    }

    public async Task<SpotifyClient?> GetClientAsync()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext == null) return null;

        // Get current tokens from the authentication cookie
        var accessToken = await httpContext.GetTokenAsync(
            OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken);
        var refreshToken = await httpContext.GetTokenAsync(
            OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken);
        var expirationDateStr = await httpContext.GetTokenAsync(
            OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessTokenExpirationDate);

        if (string.IsNullOrWhiteSpace(accessToken) || string.IsNullOrWhiteSpace(refreshToken))
            return null;

        // Check if token is expired (with 5-minute buffer)
        if (DateTime.TryParse(expirationDateStr, out var expirationDate))
        {
            if (expirationDate <= DateTime.UtcNow.AddMinutes(5))
            {
                // Token is expired or about to expire, refresh it
                var newTokens = await RefreshTokenAsync(refreshToken);
                if (newTokens == null) return null;

                // Update the authentication session with new tokens
                await UpdateAuthenticationAsync(newTokens);

                accessToken = newTokens.AccessToken;
            }
        }

        return new SpotifyClient(accessToken);
    }

    private async Task<AuthorizationCodeRefreshResponse?> RefreshTokenAsync(string refreshToken)
    {
        var clientId = _configuration["SpotifySettings:ClientId"];
        var clientSecret = _configuration["SpotifySettings:ClientSecret"];

        // Use SpotifyAPI-NET's OAuth client to refresh
        var oauthClient = new OAuthClient();

        var request = new AuthorizationCodeRefreshRequest(
            clientId!,
            clientSecret!,
            refreshToken);

        try
        {
            var response = await oauthClient.RequestToken(request);
            return response;
        }
        catch
        {
            // Refresh failed - refresh token might be revoked
            return null;
        }
    }

    private async Task UpdateAuthenticationAsync(AuthorizationCodeRefreshResponse newTokens)
    {
        var httpContext = _httpContextAccessor.HttpContext!;

        // Get the current authentication result to preserve existing claims
        var currentAuth = await httpContext.AuthenticateAsync(
            CookieAuthenticationDefaults.AuthenticationScheme);

        if (!currentAuth.Succeeded || currentAuth.Principal == null)
            return;

        // Clone the current principal to preserve all existing claims
        var claimsIdentity = currentAuth.Principal.Identity as ClaimsIdentity;
        var newIdentity = new ClaimsIdentity(claimsIdentity);

        // Create new authentication properties with updated tokens
        var authProperties = new AuthenticationProperties(currentAuth.Properties?.Items ?? new Dictionary<string, string?>())
        {
            IsPersistent = currentAuth.Properties?.IsPersistent ?? false,
            ExpiresUtc = currentAuth.Properties?.ExpiresUtc
        };

        // Update the stored tokens
        var tokens = new List<AuthenticationToken>
        {
            new AuthenticationToken
            {
                Name = OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken,
                Value = newTokens.AccessToken
            },
            new AuthenticationToken
            {
                Name = OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessTokenExpirationDate,
                Value = DateTime.UtcNow.AddSeconds(newTokens.ExpiresIn).ToString("o")
            }
        };

        // Include refresh token if a new one was provided (Spotify sometimes rotates them)
        if (!string.IsNullOrWhiteSpace(newTokens.RefreshToken))
        {
            tokens.Add(new AuthenticationToken
            {
                Name = OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken,
                Value = newTokens.RefreshToken
            });
        }
        else
        {
            // Keep the existing refresh token if no new one provided
            var existingRefreshToken = await httpContext.GetTokenAsync(
                OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken);
            if (!string.IsNullOrWhiteSpace(existingRefreshToken))
            {
                tokens.Add(new AuthenticationToken
                {
                    Name = OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken,
                    Value = existingRefreshToken
                });
            }
        }

        authProperties.StoreTokens(tokens);

        // Sign the user in again with the updated tokens
        // This creates a new authentication cookie with the refreshed tokens
        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(newIdentity),
            authProperties);
    }
}