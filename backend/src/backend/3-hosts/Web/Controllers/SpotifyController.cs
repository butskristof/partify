using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

public class SpotifyController : Controller
{
    [HttpGet]
    public IActionResult Profile()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Playlists()
    {
        return View();
    }
}
