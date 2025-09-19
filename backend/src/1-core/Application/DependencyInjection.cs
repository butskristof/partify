using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Partify.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // FluentValidation
        // scan the current assembly for validators and register them with the DI container
        // make sure to include internal types, since almost all of them should be defined as internal sealed class
        services.AddValidatorsFromAssembly(
            Assembly.GetExecutingAssembly(),
            includeInternalTypes: true
        );

        return services;
    }
}
