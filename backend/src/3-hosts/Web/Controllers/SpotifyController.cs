using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Partify.Application.Common.Services;

namespace Partify.Web.Controllers;

[Route("api/[controller]")]
[Authorize]
[ApiController]
public sealed class SpotifyController : ControllerBase
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
