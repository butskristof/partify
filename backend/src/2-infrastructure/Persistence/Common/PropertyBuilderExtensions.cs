using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Partify.Persistence.Common;

internal static class PropertyBuilderExtensions
{
    // /// <summary>
    // /// Configures the string property to use PostgreSQL text type with no maximum length constraint.
    // /// This overrides any convention-applied max length and provides unlimited text storage.
    // /// </summary>
    // public static PropertyBuilder<string?> HasUnlimitedText(this PropertyBuilder<string?> builder)
    // {
    //     return builder.HasColumnType("text").HasMaxLength(-1);
    // }

    internal static PropertyBuilder HasNoMaxLength(this PropertyBuilder builder) =>
        builder.HasMaxLength(-1);
}
