using SpotifyAPI.Web;

namespace Web.Models;

public class SpotifyProfileViewModel
{
    public required PrivateUser Profile { get; set; }
    public required string? AccessTokenExpirationDate { get; set; }
    public required string? AccessToken { get; set; }
    public required string? RefreshToken { get; set; }
}
