namespace Partify.Web.Common.Settings;

public sealed record SpotifySettings : ISettings
{
    public static string SectionName => "Spotify";

    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
};
