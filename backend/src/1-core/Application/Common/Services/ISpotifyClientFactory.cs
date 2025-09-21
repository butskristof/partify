using SpotifyAPI.Web;

namespace Partify.Application.Common.Services;

public interface ISpotifyClientFactory
{
    Task<SpotifyClient> BuildClient(CancellationToken cancellationToken = default);
}
