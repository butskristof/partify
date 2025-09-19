using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Partify.Domain.ValueTypes;

namespace Partify.Persistence.ValueConverters;

/// <summary>
/// Entity Framework value converter for SpotifyId value objects.
/// Converts SpotifyId to/from string for database storage.
/// </summary>
internal sealed class SpotifyIdValueConverter : ValueConverter<SpotifyId, string>
{
    public SpotifyIdValueConverter()
        : base(
            // To database: extract string value
            id => id.Value,
            // From database: create SpotifyId with validation
            value => new SpotifyId(value)
        ) { }
}
