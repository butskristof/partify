using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Partify.Application.Common.Configuration;
using Partify.Application.Common.Exceptions;
using Partify.Application.Common.Persistence;
using Partify.Domain.Entities;
using Partify.Domain.ValueTypes;
using SpotifyAPI.Web;

namespace Partify.Infrastructure.Spotify;

public interface ISpotifyTokensService
{
    Task<SpotifyTokens> UpdateSpotifyTokens(
        SpotifyId userId,
        string accessToken,
        DateTimeOffset accessTokenExpiresOn,
        string? refreshToken,
        CancellationToken cancellationToken = default
    );

    Task<string> GetAccessToken(SpotifyId userId, CancellationToken cancellationToken = default);
}

internal sealed class SpotifyTokensService : ISpotifyTokensService
{
    private readonly ILogger<SpotifyTokensService> _logger;
    private readonly IAppDbContext _dbContext;
    private readonly SpotifySettings _spotifySettings;
    private readonly TimeProvider _timeProvider;

    public SpotifyTokensService(
        ILogger<SpotifyTokensService> logger,
        IAppDbContext dbContext,
        IOptions<SpotifySettings> spotifySettings,
        TimeProvider timeProvider
    )
    {
        _logger = logger;
        _dbContext = dbContext;
        _timeProvider = timeProvider;
        _spotifySettings = spotifySettings.Value;
    }

    public async Task<SpotifyTokens> UpdateSpotifyTokens(
        SpotifyId userId,
        string accessToken,
        DateTimeOffset accessTokenExpiresOn,
        string? refreshToken,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInformation("Updating Spotify tokens for user {UserId}", userId);

        var spotifyTokens = await _dbContext.SpotifyTokens.SingleOrDefaultAsync(
            st => st.UserId == userId,
            cancellationToken
        );
        if (spotifyTokens is null)
        {
            _logger.LogInformation("Creating new Spotify tokens entity");
            spotifyTokens = new SpotifyTokens { UserId = userId };
            _dbContext.SpotifyTokens.Add(spotifyTokens);
        }
        else
        {
            _logger.LogInformation("Updating existing Spotify tokens entity");
        }

        spotifyTokens.AccessToken = accessToken;
        spotifyTokens.AccessTokenExpiresOn = accessTokenExpiresOn;

        // only update if new value is passed in, sometimes Spotify doesn't return a new refresh token and we
        // should keep using the existing one
        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            _logger.LogInformation("Updating refresh token");
            spotifyTokens.RefreshToken = refreshToken;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Successfully persisted changes to database");

        return spotifyTokens;
    }

    public async Task<string> GetAccessToken(
        SpotifyId userId,
        CancellationToken cancellationToken = default
    )
    {
        _logger.LogInformation("Getting Spotify access token for user {UserId}", userId);

        var spotifyTokens = await _dbContext.SpotifyTokens.SingleOrDefaultAsync(
            st => st.UserId == userId,
            cancellationToken
        );

        if (spotifyTokens is null)
        {
            _logger.LogWarning("No Spotify tokens found for user {UserId}", userId);
            throw new SpotifyAuthenticationException("Authentication required");
        }

        if (spotifyTokens.IsAccessTokenExpired(_timeProvider.GetLocalNow()))
        {
            _logger.LogInformation("Access token expired, refreshing");
            if (!spotifyTokens.HasRefreshToken)
            {
                _logger.LogWarning("Access token expired but no refresh token available");
                throw new SpotifyAuthenticationException("Authentication required");
            }

            spotifyTokens = await RefreshTokens(
                userId,
                spotifyTokens.RefreshToken!,
                cancellationToken
            );
        }

        if (!spotifyTokens.HasAccessToken)
        {
            _logger.LogWarning("Access token is null or empty after token refresh");
            throw new SpotifyAuthenticationException("Authentication required");
        }

        return spotifyTokens.AccessToken!;
    }

    private async Task<SpotifyTokens> RefreshTokens(
        SpotifyId userId,
        string refreshToken,
        CancellationToken cancellationToken
    )
    {
        try
        {
            _logger.LogInformation("Refreshing Spotify tokens for user {UserId}", userId);

            var response = await new OAuthClient().RequestToken(
                new AuthorizationCodeRefreshRequest(
                    _spotifySettings.ClientId,
                    _spotifySettings.ClientSecret,
                    refreshToken
                ),
                cancellationToken
            );

            return await UpdateSpotifyTokens(
                userId,
                response.AccessToken,
                response.CreatedAt.AddSeconds(response.ExpiresIn),
                !string.IsNullOrWhiteSpace(response.RefreshToken) ? response.RefreshToken : null,
                cancellationToken
            );
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to refresh Spotify tokens for user {UserId}. Error: {ErrorMessage}",
                userId,
                ex.Message
            );
            throw new SpotifyAuthenticationException("Authentication required", ex);
        }
    }
}
