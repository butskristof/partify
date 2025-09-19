using Partify.Persistence;
using Partify.ServiceDefaults;
using Partify.ServiceDefaults.Constants;

var builder = Host.CreateApplicationBuilder(args);
builder.AddServiceDefaults();
builder.AddPersistence(Resources.AppDb);

var host = builder.Build();

using var scope = host.Services.CreateScope();
var dbInitializer = scope.ServiceProvider.GetRequiredService<IAppDbContextInitializer>();
try
{
    await dbInitializer.InitializeAsync();
}
catch (Exception ex)
{
    var logger = host.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during database migration");
    throw;
}
