using SpotifyAPI.Web;

namespace Web.Models;

public class SpotifyPlaylistsViewModel
{
    public required bool HasNext { get; set; }
    public required bool HasPrevious { get; set; }
    public required Paging<FullPlaylist> Playlists { get; set; }
}
