using FluentValidation;
using Partify.Application.Common.Validation;

namespace Partify.Application.Common.Configuration;

public sealed class SpotifySettings : ISettings
{
    public static string SectionName => "Spotify";

    public required string ClientId { get; init; }
    public required string ClientSecret { get; init; }
}

internal sealed class SpotifySettingsValidator : BaseValidator<SpotifySettings>
{
    public SpotifySettingsValidator()
    {
        RuleFor(s => s.ClientId).Cascade(CascadeMode.Stop).NotEmptyWithErrorCode();

        RuleFor(s => s.ClientSecret).Cascade(CascadeMode.Stop).NotEmptyWithErrorCode();
    }
}
