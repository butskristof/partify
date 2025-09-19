using Partify.Persistence;
using Partify.ServiceDefaults;
using Partify.ServiceDefaults.Constants;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();
builder.AddPersistence(Resources.AppDb, [HealthCheckConstants.Tags.Ready]);

var app = builder.Build();

app.Run();
