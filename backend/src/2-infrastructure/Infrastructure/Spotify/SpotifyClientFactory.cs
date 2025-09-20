using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Partify.Application.Common.Authentication;
using Partify.Application.Common.Configuration;
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

    public async Task BuildClient(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Building Spotify client for user {UserId}",
            _authenticationInfo.UserId
        );

        var accessToken = await _spotifyTokensService.GetAccessToken(
            _authenticationInfo.UserId,
            cancellationToken
        );
        // TODO handle null
        return new SpotifyClient(accessToken);
    }
}
