using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace Partify.WebServer.Modules;

internal static class AuthModule
{
    internal static IEndpointRouteBuilder MapAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup($"/auth");
        group.MapGet("/login", Login);
        group.MapGet("/callback", Callback);
        return endpoints;
    }

    private static async ValueTask<IResult> Login(HttpContext httpContext)
    {
        // const string url = "https://accounts.spotify.com/authorize";
        // const string state = "some state";
        // const string clientId = "91a5f76fec884055837d6ad02905eca2";
        // const string redirectUri = "https://127.0.0.1:7292/auth/callback";
        // string[] scopes = ["user-read-playback-state", "user-modify-playback-state"];
        //
        // var destination = BuildUrlWithQueryStringUsingUriBuilder(
        //     "https://accounts.spotify.com/authorize",
        //     [
        //         ("response_type", "code"),
        //         ("client_id", clientId),
        //         ("state", state),
        //         ("redirect_uri", redirectUri),
        //         ("scope", string.Join(" ", scopes)),
        //     ]
        // );
        // return Task.FromResult(Results.Ok(new { Destination = destination }));

        var claims = new List<Claim>() { new(ClaimTypes.NameIdentifier, "test") };
        var identity = new ClaimsIdentity(claims, "AppCookie");
        await httpContext.SignInAsync(new ClaimsPrincipal(identity));
        return Results.Ok();
        // return Results.Redirect(
        //     httpContext.Request.Query.TryGetValue("returnUrl", out var returnUrl) ? returnUrl! : "/"
        // );
    }

    private static Task<IResult> Callback([FromQuery] string code, [FromQuery] string state)
    {
        // return Task.FromResult(Results.Ok(new { code, state }));
        throw new NotImplementedException();
    }

    private static string BuildUrlWithQueryStringUsingUriBuilder(
        string basePath,
        (string key, string value)[] queryParams
    )
    {
        var uriBuilder = new UriBuilder(basePath)
        {
            Query = string.Join("&", queryParams.Select(kvp => $"{kvp.key}={kvp.value}")),
        };
        return uriBuilder.Uri.AbsoluteUri;
    }
}
