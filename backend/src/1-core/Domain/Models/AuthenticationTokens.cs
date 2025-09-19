namespace Partify.Domain.Models;

public sealed class AuthenticationTokens
{
    public required string SpotifyUserId { get; set; }

    public string? AccessToken { get; set; }
    public DateTimeOffset? AccessTokenExpiresOn { get; set; }
    public string? RefreshToken { get; set; }
}