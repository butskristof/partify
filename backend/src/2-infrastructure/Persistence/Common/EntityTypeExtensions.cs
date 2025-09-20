using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Partify.Domain.Constants;

namespace Partify.Persistence.Common;

internal static class EntityTypeExtensions
{
    private const string OpenIddictNamespace = "OpenIddict";

    /// <summary>
    /// Determines if the entity type belongs to the OpenIddict namespace.
    /// Used to exclude third-party entities from custom conventions.
    /// </summary>
    internal static bool IsOpenIddictEntity(this IConventionEntityType entityType)
    {
        return entityType.ClrType.Namespace?.StartsWith(
                OpenIddictNamespace,
                StringComparison.Ordinal
            ) == true;
    }
}

/// <summary>
/// EF Core convention that applies a default maximum length to all string properties,
/// excluding OpenIddict entities to avoid interfering with their predefined configurations.
/// This helps reduce varchar(max) columns in the database schema.
/// </summary>
internal sealed class MaxStringLengthConvention : IModelFinalizingConvention
{
    public void ProcessModelFinalizing(
        IConventionModelBuilder modelBuilder,
        IConventionContext<IConventionModelBuilder> context
    )
    {
        var stringProperties = modelBuilder
            .Metadata.GetEntityTypes()
            .Where(entityType => !entityType.IsOpenIddictEntity())
            .SelectMany(entityType => entityType.GetDeclaredProperties())
            .Where(property => property.ClrType == typeof(string));

        foreach (var property in stringProperties)
            property.Builder.HasMaxLength(ApplicationConstants.DefaultMaxStringLength);
    }
}
