using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Partify.Application.Common.Services;

namespace Partify.Web.Controllers;

[Controller]
[Route("[controller]")]
[Authorize]
public sealed class SpotifyController : Controller
{
    private readonly ISpotifyClientFactory _spotifyClientFactory;

    public SpotifyController(ISpotifyClientFactory spotifyClientFactory)
    {
        _spotifyClientFactory = spotifyClientFactory;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile()
    {
        var client = await _spotifyClientFactory.BuildClient();
        var profile = await client.UserProfile.Current();
        return Ok(profile);
    }
}
