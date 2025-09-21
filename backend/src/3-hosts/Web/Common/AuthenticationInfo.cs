using System.Security.Authentication;
using System.Security.Claims;
using Partify.Application.Common.Authentication;
using Partify.Domain.ValueTypes;

namespace Partify.Web.Common;

internal sealed class AuthenticationInfo : IAuthenticationInfo
{
    #region construction

    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticationInfo(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    #endregion

    private ClaimsPrincipal? User => _httpContextAccessor.HttpContext?.User;

    public SpotifyId UserId =>
        new(User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new AuthenticationException());

    public string Name =>
        User?.FindFirstValue(ClaimTypes.Name) ?? throw new AuthenticationException();
}
