using SpotifyAPI.Web;

namespace Web.Services;

public interface ISpotifyClientService
{
    Task<SpotifyClient?> GetClientAsync();
}