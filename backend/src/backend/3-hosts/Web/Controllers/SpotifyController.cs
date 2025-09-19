using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Client.AspNetCore;
using SpotifyAPI.Web;
using Web.Models;
using Web.Services;

namespace Web.Controllers;

[Authorize]
public class SpotifyController : Controller
{
    private readonly ISpotifyClientService _spotifyClientService;

    public SpotifyController(ISpotifyClientService spotifyClientService)
    {
        _spotifyClientService = spotifyClientService;
    }
    [HttpGet]
    public async Task<IActionResult> Profile()
    {
        var client = await _spotifyClientService.GetClientAsync();
        if (client is null)
            return Unauthorized();

        var accessTokenExpirationDate = await HttpContext.GetTokenAsync(
            OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessTokenExpirationDate
        );
        var accessToken = await HttpContext.GetTokenAsync(
            OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken
        );
        var refreshToken = await HttpContext.GetTokenAsync(
            OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken
        );

        var profile = await client.UserProfile.Current();
        var model = new SpotifyProfileViewModel
        {
            Profile = profile,
            AccessTokenExpirationDate = accessTokenExpirationDate,
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        };
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Playlists([FromQuery] int offset = 0)
    {
        const int limit = 10;
        var client = await _spotifyClientService.GetClientAsync();
        if (client is null)
            return Unauthorized();

        var request = new PlaylistCurrentUsersRequest() { Limit = limit, Offset = offset };
        var playlists = await client.Playlists.CurrentUsers(request);
        return View(
            new SpotifyPlaylistsViewModel
            {
                HasNext = playlists.Next is not null,
                HasPrevious = playlists.Previous is not null,
                Playlists = playlists,
            }
        );
    }

}
