using Microsoft.Extensions.Logging;
using Partify.Application.Common.Authentication;
using Partify.Application.Common.Services;
using SpotifyAPI.Web;

namespace Partify.Infrastructure.Spotify;

internal sealed class SpotifyClientFactory : ISpotifyClientFactory
{
    private readonly ILogger<SpotifyClientFactory> _logger;
    private readonly IAuthenticationInfo _authenticationInfo;
    private readonly ISpotifyTokensService _spotifyTokensService;

    public SpotifyClientFactory(
        ILogger<SpotifyClientFactory> logger,
        IAuthenticationInfo authenticationInfo,
        ISpotifyTokensService spotifyTokensService
    )
    {
        _logger = logger;
        _authenticationInfo = authenticationInfo;
        _spotifyTokensService = spotifyTokensService;
    }

    public async Task<SpotifyClient> BuildClient(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Building Spotify client for user {UserId}",
            _authenticationInfo.UserId
        );

        var accessToken = await _spotifyTokensService.GetAccessToken(
            _authenticationInfo.UserId,
            cancellationToken
        );

        return new SpotifyClient(accessToken);
    }
}
