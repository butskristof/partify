using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Partify.Application.Common.Persistence;
using Partify.Domain.Entities;
using Partify.Domain.ValueTypes;

namespace Partify.Infrastructure.Spotify;

public interface ISpotifyTokensService
{
    Task UpdateSpotifyTokens(
        SpotifyId userId,
        string accessToken,
        DateTimeOffset accessTokenExpiresOn,
        string? refreshToken,
        CancellationToken cancellationToken = default
    );
}

internal sealed class SpotifyTokensService : ISpotifyTokensService
{
    private readonly ILogger<SpotifyTokensService> _logger;
    private readonly IAppDbContext _dbContext;

    public SpotifyTokensService(ILogger<SpotifyTokensService> logger, IAppDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    public async Task UpdateSpotifyTokens(
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
            _logger.LogInformation("Creating new Spotify tokens entity for user {UserId}", userId);
            spotifyTokens = new SpotifyTokens { UserId = userId };
            _dbContext.SpotifyTokens.Add(spotifyTokens);
        }
        else
        {
            _logger.LogInformation(
                "Updating existing Spotify tokens entity for user {UserId}",
                userId
            );
        }

        spotifyTokens.AccessToken = accessToken;
        spotifyTokens.AccessTokenExpiresOn = accessTokenExpiresOn;

        // only update if new value is passed in, sometimes Spotify doesn't return a new refresh token and we
        // should keep using the existing one
        if (!string.IsNullOrWhiteSpace(refreshToken))
        {
            _logger.LogInformation("Updating refresh token for user {UserId}", userId);
            spotifyTokens.RefreshToken = refreshToken;
        }

        await _dbContext.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Successfully persisted changes to database");
    }
}
