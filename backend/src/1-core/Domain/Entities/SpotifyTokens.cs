using Partify.Domain.Constants;
using Partify.Domain.ValueTypes;

namespace Partify.Domain.Entities;

public sealed class SpotifyTokens
{
    public required SpotifyId UserId { get; set; }

    public string? AccessToken { get; set; }
    public bool HasAccessToken => !string.IsNullOrWhiteSpace(AccessToken);
    public DateTimeOffset? AccessTokenExpiresOn { get; set; }

    public bool IsAccessTokenExpired(DateTimeOffset timestamp) =>
        !AccessTokenExpiresOn.HasValue
        || AccessTokenExpiresOn.Value.AddMinutes(
            -1 * ApplicationConstants.TokenExpirationMarginMinutes
        ) < timestamp;

    public string? RefreshToken { get; set; }
    public bool HasRefreshToken => !string.IsNullOrWhiteSpace(RefreshToken);
}
