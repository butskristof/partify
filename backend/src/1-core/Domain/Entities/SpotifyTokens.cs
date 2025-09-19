using Partify.Domain.ValueTypes;

namespace Partify.Domain.Entities;

public sealed class SpotifyTokens
{
    public required SpotifyId UserId { get; set; }

    public string? AccessToken { get; set; }
    public DateTimeOffset? AccessTokenExpiresOn { get; set; }
    public string? RefreshToken { get; set; }
}
