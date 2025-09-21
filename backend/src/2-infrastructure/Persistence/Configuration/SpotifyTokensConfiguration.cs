using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Partify.Domain.Entities;
using Partify.Persistence.Common;

namespace Partify.Persistence.Configuration;

internal sealed class SpotifyTokensConfiguration : IEntityTypeConfiguration<SpotifyTokens>
{
    public void Configure(EntityTypeBuilder<SpotifyTokens> builder)
    {
        builder.ToTable("SpotifyTokens");

        // primary key makes it unique as well
        builder.HasKey(t => t.UserId);

        // configure token columns for unlimited OAuth token storage
        builder.Property(t => t.AccessToken).HasNoMaxLength();
        builder.Property(t => t.RefreshToken).HasNoMaxLength();
    }
}
