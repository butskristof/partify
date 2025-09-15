using Partify.ServiceDefaults.Constants;

var builder = DistributedApplication.CreateBuilder(args);

#region web server

var api = builder
    .AddProject<Projects.WebServer>(Resources.WebServer)
    .WithHttpHealthCheck("/health/live")
    .WithHttpHealthCheck("/health/ready");

#endregion

builder.Build().Run();
