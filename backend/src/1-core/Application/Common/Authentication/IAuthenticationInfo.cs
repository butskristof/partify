using Partify.Domain.ValueTypes;

namespace Partify.Application.Common.Authentication;

public interface IAuthenticationInfo
{
    SpotifyId UserId { get; }
    string Name { get; }
}
