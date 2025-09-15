using Partify.ServiceDefaults;
using Partify.WebServer;
using Partify.WebServer.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();
builder.Services.AddWebServer();

var app = builder.Build();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.MapAuthEndpoints().MapTestEndpoints();

app.Run();
