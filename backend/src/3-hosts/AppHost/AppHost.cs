using Partify.ServiceDefaults.Constants;
using Projects;

var builder = DistributedApplication.CreateBuilder(args);

#region Database

var postgres = builder
    .AddPostgres(Resources.Postgres)
    .WithPgAdmin()
    .WithLifetime(ContainerLifetime.Session)
    .WithDataVolume();
var appDb = postgres.AddDatabase(Resources.AppDb);

var databaseMigrations = builder
    .AddProject<DatabaseMigrations>(Resources.DatabaseMigrations)
    .WithReference(appDb)
    .WaitFor(appDb);

#endregion

#region Backend

var backend = builder
    .AddProject<Web>(Resources.Backend)
    .WithReference(appDb)
    .WaitForCompletion(databaseMigrations)
    .WithHttpHealthCheck("/health/live")
    .WithHttpHealthCheck("/health/ready");

#endregion

builder.Build().Run();
