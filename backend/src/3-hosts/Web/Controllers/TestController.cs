using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Partify.Application.Common.Services;

namespace Partify.Web.Controllers;

[Controller]
[Route("[controller]")]
public sealed class TestController : Controller
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(
            new
            {
                IsAuthenticated = User.Identity?.IsAuthenticated ?? false,
                Name = User.Identity?.Name,
                Claims = User.Claims.Select(c => new { c.Type, c.Value }),
            }
        );
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile(
        [FromServices] ISpotifyClientFactory spotifyClientFactory
    )
    {
        var client = await spotifyClientFactory.BuildClient();
        var profile = await client.UserProfile.Current();
        return Ok(profile);
    }
}
