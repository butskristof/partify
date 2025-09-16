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
app.MapControllers();

app.Run();
