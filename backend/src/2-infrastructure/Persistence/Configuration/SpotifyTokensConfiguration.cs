using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Partify.Domain.Entities;

namespace Partify.Persistence.Configuration;

internal sealed class SpotifyTokensConfiguration : IEntityTypeConfiguration<SpotifyTokens>
{
    public void Configure(EntityTypeBuilder<SpotifyTokens> builder)
    {
        builder.ToTable("SpotifyTokens");

        // primary key makes it unique as well
        builder.HasKey(t => t.UserId);
    }
}
