using Microsoft.Extensions.Options;
using Partify.Web.Common.Settings;

namespace Partify.Web;

internal static class DependencyInjection
{
    #region Configuration

    internal static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services.AddValidatedSettings<SpotifySettings>();

        return services;
    }

    private static IServiceCollection AddValidatedSettings<TOptions>(
        this IServiceCollection services
    )
        where TOptions : class, ISettings =>
        services.AddValidatedSettings<TOptions>(TOptions.SectionName);

    private static IServiceCollection AddValidatedSettings<TOptions>(
        this IServiceCollection services,
        string sectionName
    )
        where TOptions : class
    {
        services.AddOptions<TOptions>().BindConfiguration(sectionName);
        // TODO add FluentValidation
        // .FluentValidateOptions()
        // .ValidateOnStart();

        return services;
    }

    #endregion

    internal static IServiceCollection AddWeb(this IServiceCollection services)
    {
        // add support for ProblemDetails to handle failed requests
        // it's effectively a default implementation of the IProblemDetailsService and can be
        // overridden if desired
        // it adds a default problem details response for all client and server error responses (400-599)
        // that don't have body content yet
        services.AddProblemDetails();

        using (var serviceProvider = services.BuildServiceProvider())
        {
            var spotifySettings = serviceProvider
                .GetRequiredService<IOptions<SpotifySettings>>()
                .Value;
            services.AddAuthentication();
            services.AddAuthorization();
        }

        services.AddControllers();

        return services;
    }
}
