using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Partify.Web.Common.Settings;
using Partify.Web.Data;

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

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseInMemoryDatabase("db");
            options.UseOpenIddict();
        });

        using (var serviceProvider = services.BuildServiceProvider())
        {
            var spotifySettings = serviceProvider
                .GetRequiredService<IOptions<SpotifySettings>>()
                .Value;

            services
                .AddOpenIddict()
                .AddClient(options =>
                {
                    options.AllowAuthorizationCodeFlow();

                    options
                        .AddDevelopmentEncryptionCertificate()
                        .AddDevelopmentSigningCertificate();

                    options.UseAspNetCore().EnableRedirectionEndpointPassthrough();
                    options.UseSystemNetHttp();

                    options
                        .UseWebProviders()
                        .AddSpotify(spotifyOptions =>
                        {
                            spotifyOptions
                                .SetClientId(spotifySettings.ClientId)
                                .SetClientSecret(spotifySettings.ClientSecret)
                                .SetRedirectUri("/auth/callback/login/spotify");
                        });
                })
                .AddCore(options =>
                {
                    options.UseEntityFrameworkCore().UseDbContext<AppDbContext>();
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
                    options.ExpireTimeSpan = TimeSpan.FromDays(1);
                    options.LoginPath = "/challenge";
                    options.AccessDeniedPath = "/auth/forbidden";
                });
            services.AddAuthorization();
        }

        services.AddControllers();

        return services;
    }
}
