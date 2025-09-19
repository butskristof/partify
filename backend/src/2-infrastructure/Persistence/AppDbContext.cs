using Microsoft.EntityFrameworkCore;
using Partify.Application.Common.Persistence;
using Partify.Domain.Constants;
using Partify.Domain.Entities;
using Partify.Domain.ValueTypes;
using Partify.Persistence.Common;
using Partify.Persistence.ValueConverters;

namespace Partify.Persistence;

public sealed class AppDbContext : DbContext, IAppDbContext
{
    #region construction

    public AppDbContext(DbContextOptions options)
        : base(options) { }

    #endregion

    #region entities

    public DbSet<SpotifyTokens> SpotifyTokens { get; set; }

    #endregion

    #region configuration

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // the base method is empty, but retain the call to minimise impact if
        // it should be used in a future version
        base.ConfigureConventions(configurationBuilder);

        // set text fields to have a reduced maximum length by default
        // this cuts down on a lot of varchar(max) columns, and can still be set to a higher
        // maximum length on a per-column basis
        configurationBuilder
            .Properties<string>()
            .HaveMaxLength(ApplicationConstants.DefaultMaxStringLength);

        configurationBuilder.Properties<decimal>().HavePrecision(18, 6);

        configurationBuilder
            .Properties<DateTimeOffset>()
            .HaveConversion<DateTimeOffsetValueConverter>();

        configurationBuilder.Properties<SpotifyId>().HaveConversion<SpotifyIdValueConverter>();
        configurationBuilder
            .Properties<SpotifyId>()
            .HaveMaxLength(ApplicationConstants.SpotifyIdLength)
            .AreFixedLength();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // the base method is empty, but retain the call to minimise impact if
        // it should be used in a future version
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasCollation(
            PersistenceConstants.CaseInsensitiveCollation,
            locale: "en-u-ks-primary",
            provider: "icu",
            deterministic: false
        );
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    #endregion
}
