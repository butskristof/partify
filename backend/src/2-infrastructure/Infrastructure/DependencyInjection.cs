using Microsoft.Extensions.DependencyInjection;
using Partify.Application.Common.Services;
using Partify.Infrastructure.Spotify;

namespace Partify.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        // the default implementation of TimeProvider can be registered as a singleton
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<ISpotifyTokensService, SpotifyTokensService>();
        services.AddScoped<ISpotifyClientFactory, SpotifyClientFactory>();

        return services;
    }
}
