using Microsoft.Extensions.Options;
using Partify.Application.Common.Authentication;
using Partify.Application.Common.Configuration;
using Partify.Application.Common.Validation;
using Partify.Web.Common;

namespace Partify.Web;

internal static class DependencyInjection
{
    #region Configuration

    internal static IServiceCollection AddConfiguration(this IServiceCollection services)
    {
        services.AddValidatedSettings<CorsSettings>().AddValidatedSettings<SpotifySettings>();

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
        services
            .AddOptions<TOptions>()
            .BindConfiguration(sectionName)
            .FluentValidateOptions()
            .ValidateOnStart();

        return services;
    }

    #endregion

    #region Web

    internal static IServiceCollection AddWeb(this IServiceCollection services)
    {
        // add support for ProblemDetails to handle failed requests
        // it's effectively a default implementation of the IProblemDetailsService and can be
        // overridden if desired
        // it adds a default problem details response for all client and server error responses (400-599)
        // that don't have body content yet
        services.AddProblemDetails();

        services.AddHealthChecks();

        services.AddAuthentication();
        services.AddAuthorization();
        services.AddHttpContextAccessor().AddSingleton<IAuthenticationInfo, AuthenticationInfo>();

        services.AddCors(options =>
        {
            using var serviceProvider = services.BuildServiceProvider();
            var settings = serviceProvider.GetRequiredService<IOptions<CorsSettings>>().Value;
            if (!settings.AllowCors)
                return;

            options.AddDefaultPolicy(policy =>
                policy.WithOrigins(settings.AllowedOrigins).AllowAnyHeader().AllowAnyMethod()
            );
        });

        return services;
    }

    private static IServiceCollection AddSpotifyAuthentication(this IServiceCollection services)
    {
        return services;
    }

    #endregion
}
