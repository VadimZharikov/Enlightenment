var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.EnlightenmentApp_API>("enlightenmentapp.api");

builder.Build().Run();
