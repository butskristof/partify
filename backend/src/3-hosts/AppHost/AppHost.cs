using Partify.ServiceDefaults.Constants;

var builder = DistributedApplication.CreateBuilder(args);

#region Web

var web = builder
    .AddProject<Projects.Web>(Resources.Web)
    .WithHttpHealthCheck("/health/live")
    .WithHttpHealthCheck("/health/ready");

#endregion

builder.Build().Run();
