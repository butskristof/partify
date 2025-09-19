using Microsoft.AspNetCore.Mvc;

namespace Partify.Web.Controllers;

[Controller]
[Route("[controller]")]
internal sealed class TestController : Controller
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
}
