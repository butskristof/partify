namespace Partify.WebServer;

internal static class DependencyInjection
{
    internal static IServiceCollection AddWebServer(this IServiceCollection services)
    {
        services.AddProblemDetails();

        const string schemeName = "AppCookie";
        services
            .AddAuthentication(schemeName)
            .AddCookie(
                schemeName,
                options =>
                {
                    options.Cookie.Name = "__Host_app";
                    options.Cookie.HttpOnly = true;
                    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                    options.Cookie.SameSite = SameSiteMode.Lax;
                    options.SlidingExpiration = true;
                    options.ExpireTimeSpan = TimeSpan.FromDays(14);
                    options.LoginPath = "/auth/login";
                    options.AccessDeniedPath = "/auth/forbidden";
                }
            );
        services.AddAuthorization();

        return services;
    }
}
