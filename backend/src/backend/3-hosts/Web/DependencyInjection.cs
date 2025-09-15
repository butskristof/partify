namespace Partify.Web;

internal static class DependencyInjection
{
    internal static IServiceCollection AddWeb(this IServiceCollection services)
    {
        // add support for ProblemDetails to handle failed requests
        // it's effectively a default implementation of the IProblemDetailsService and can be
        // overridden if desired
        // it adds a default problem details response for all client and server error responses (400-599)
        // that don't have body content yet
        services.AddProblemDetails();

        services.AddAuthentication();
        services.AddAuthorization();

        return services;
    }
}
