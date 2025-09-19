using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using Partify.Application.Common.Authentication;
using Partify.Application.Common.Configuration;
using Partify.Application.Common.Validation;
using Partify.Persistence;
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

        services.AddAuth();
        services.AddHttpContextAccessor().AddSingleton<IAuthenticationInfo, AuthenticationInfo>();

        services.AddCors(options =>
        {
            using var serviceProvider = services.BuildServiceProvider();
            var corsSettings = serviceProvider.GetRequiredService<IOptions<CorsSettings>>().Value;
            if (!corsSettings.AllowCors)
                return;

            options.AddDefaultPolicy(policy =>
                policy.WithOrigins(corsSettings.AllowedOrigins).AllowAnyHeader().AllowAnyMethod()
            );
        });

        services.AddControllers();

        return services;
    }

    private static IServiceCollection AddAuth(this IServiceCollection services)
    {
        services
            .AddOpenIddict()
            .AddCore(options =>
            {
                options.UseEntityFrameworkCore().UseDbContext<AppDbContext>();
            })
            .AddClient(builder =>
            {
                builder.AllowAuthorizationCodeFlow();

                builder.AddDevelopmentEncryptionCertificate().AddDevelopmentSigningCertificate();

                builder.UseAspNetCore().EnableRedirectionEndpointPassthrough();

                builder.UseSystemNetHttp();

                using var serviceProvider = services.BuildServiceProvider();
                var spotifySettings = serviceProvider
                    .GetRequiredService<IOptions<SpotifySettings>>()
                    .Value;
                builder
                    .UseWebProviders()
                    .AddSpotify(spotifyOptions =>
                        spotifyOptions
                            .SetClientId(spotifySettings.ClientId)
                            .SetClientSecret(spotifySettings.ClientSecret)
                            .SetRedirectUri("auth/callback/spotify")
                    );
            });

        services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = "__Host_app";
                options.Cookie.HttpOnly = true;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.Cookie.SameSite = SameSiteMode.Lax;
                options.SlidingExpiration = true;
                options.ExpireTimeSpan = TimeSpan.FromDays(14);
                options.LoginPath = "/auth/login";
            });
        services.AddAuthorization();

        return services;
    }

    #endregion
}
