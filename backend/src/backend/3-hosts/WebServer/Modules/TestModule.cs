namespace Partify.WebServer.Modules;

internal static class TestModule
{
    internal static IEndpointConventionBuilder MapTestEndpoints(
        this IEndpointRouteBuilder endpoints
    )
    {
        var group = endpoints.MapGroup($"/test").RequireAuthorization();
        group.MapGet("/", Get);

        return group;
    }

    private static IResult Get()
    {
        return Results.Ok();
    }
}
