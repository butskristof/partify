using Partify.Domain.Constants;

namespace Partify.Domain.ValueTypes;

// https://developer.spotify.com/documentation/web-api/concepts/spotify-uris-ids
// https://stackoverflow.com/a/37986045

/// <summary>
/// Represents a Spotify ID - a base-62 encoded identifier used throughout the Spotify Web API.
/// Spotify IDs are generated as UUID4s converted to a base-62 representation.
/// </summary>
/// <remarks>
/// Base-62 uses characters [0-9a-zA-Z] to create shorter, URL-safe identifiers.
/// Used for tracks, artists, albums, playlists, users, and other Spotify resources.
/// Example: "6rqhFgbbKwnb9MLmUQDhG6"
/// </remarks>
public readonly record struct SpotifyId
{
    public string Value { get; }

    /// <summary>
    /// Creates a new SpotifyId with validation.
    /// </summary>
    /// <param name="value">The Spotify ID string to validate and store.</param>
    /// <exception cref="ArgumentException">Thrown when the value is null, empty, or not a valid Spotify ID format.</exception>
    public SpotifyId(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Spotify ID cannot be null or empty", nameof(value));

        if (!IsValidSpotifyId(value))
            throw new ArgumentException("Invalid Spotify ID format", nameof(value));

        Value = value;
    }

    /// <summary>
    /// Validates that a string conforms to Spotify's base-62 ID format.
    /// </summary>
    /// <param name="value">The string to validate.</param>
    /// <returns>True if the string is at most 50 characters and contains only base-62 characters [0-9a-zA-Z].</returns>
    private static bool IsValidSpotifyId(string value) =>
        value.Length <= ApplicationConstants.SpotifyIdMaxLength
        // check base 62: only alphanumeric characters
        && value.All(c =>
            c is >= '0' and <= '9'
            || // Digits 0-9
            c is >= 'a' and <= 'z'
            || // Lowercase letters a-z
            c is >= 'A' and <= 'Z'
        ); // Uppercase letters A-Z

    public static implicit operator string(SpotifyId id) => id.Value;

    public static explicit operator SpotifyId(string value) => new(value);

    public override string ToString() => Value;
}
