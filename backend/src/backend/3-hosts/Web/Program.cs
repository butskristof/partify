using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;
using OpenIddict.Client.WebIntegration;
using Partify.ServiceDefaults;
using Partify.Web;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.Services.AddConfiguration().AddWeb();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app
// the default exception handler will catch unhandled exceptions and return
// them as ProblemDetails with status code 500 Internal Server Error
.UseExceptionHandler()
    // the status code pages will map additional failed requests (outside of
    // those throwing exceptions) to responses with ProblemDetails body content
    // this includes 404, method not allowed, ... (all status codes between 400 and 599)
    // keep in mind that this middleware will only activate if the body is empty when
    // it reaches it
    .UseStatusCodePages();

app.MapDefaultEndpoints();

app.MapGet(
    "challenge",
    (string? returnUrl) =>
    {
        var properties = new AuthenticationProperties
        {
            // Only allow local return URLs to prevent open redirect attacks.
            RedirectUri = RedirectHttpResult.IsLocalUrl(returnUrl) ? returnUrl : "/",
        };

        return Results.Challenge(
            properties,
            authenticationSchemes: [OpenIddictClientWebIntegrationConstants.Providers.Spotify]
        );
    }
);
app.MapMethods(
    "auth/callback/login/spotify",
    [HttpMethods.Get, HttpMethods.Post],
    async (HttpContext context) =>
    {
        var result = await context.AuthenticateAsync(
            OpenIddictClientWebIntegrationConstants.Providers.Spotify
        );

        var identity = new ClaimsIdentity(
            authenticationType: OpenIddictClientWebIntegrationConstants.Providers.Spotify
        );
        if (result.Principal is not null)
        {
            identity
                .SetClaim(ClaimTypes.Name, result.Principal.GetClaim(ClaimTypes.Name))
                .SetClaim(
                    ClaimTypes.NameIdentifier,
                    result.Principal.GetClaim(ClaimTypes.NameIdentifier)
                );
        }

        // Build the authentication properties based on the properties that were added when the challenge was triggered.
        var properties = new AuthenticationProperties(
            result.Properties?.Items ?? new Dictionary<string, string?>()
        )
        {
            RedirectUri = result.Properties?.RedirectUri ?? "/whoami",
        };

        return Results.SignIn(new ClaimsPrincipal(identity), properties);
    }
);

app.MapGet(
    "whoami",
    async (HttpContext context) =>
    {
        var result = await context.AuthenticateAsync();
        if (result is not { Succeeded: true })
        {
            return Results.Text("You're not logged in.");
        }

        // result.Principal.Claims
        var token = await context.GetTokenAsync(
            OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken
        );
        var claims = result.Principal.Claims.ToDictionary(c => c.Type, c => c.Value);
        return Results.Json(new { claims, token });
        // return Results.Text($"You are {result.Principal.FindFirst("display_name")!.Value}.");
    }
);

app.MapControllers();

app.Run();
