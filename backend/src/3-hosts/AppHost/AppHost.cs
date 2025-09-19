using Partify.ServiceDefaults.Constants;

var builder = DistributedApplication.CreateBuilder(args);

#region Database

var postgres = builder
    .AddPostgres(Resources.Postgres)
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();
var appDb = postgres.AddDatabase(Resources.AppDb);

var databaseMigrations = builder
    .AddProject<Projects.DatabaseMigrations>(Resources.DatabaseMigrations)
    .WithReference(appDb)
    .WaitFor(appDb);

#endregion

#region Backend

var backend = builder
    .AddProject<Projects.Web>(Resources.Backend)
    .WithReference(appDb)
    .WaitForCompletion(databaseMigrations)
    .WithHttpHealthCheck("/health/live")
    .WithHttpHealthCheck("/health/ready");

#endregion

builder.Build().Run();
