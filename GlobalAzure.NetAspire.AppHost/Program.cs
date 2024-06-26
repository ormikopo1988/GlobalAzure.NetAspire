using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Automatically provision an Application Insights resource
var insights = builder.ExecutionContext.IsPublishMode
    ? builder.AddAzureApplicationInsights("aspiredemoapplicationinsights")
    : builder.AddConnectionString("aspiredemoapplicationinsights", "APPLICATIONINSIGHTS_CONNECTION_STRING");

// Provisions an Azure SQL Database when published
var customerDb = builder
    .AddSqlServer("aspiredemosqlserver")
    .PublishAsAzureSqlDatabase()
    .AddDatabase("aspiredemodb");

// Provisions an Azure Redis Cache when published
var cache = builder
    .AddRedis("cache")
    .PublishAsAzureRedis();

var aspireDemoApi = builder
    .AddProject<Projects.GlobalAzure_NetAspire_Api>("aspiredemoapi")
    .WithReference(insights);

var aspireDemoApp = builder
    .AddProject<Projects.GlobalAzure_NetAspire_Server>("aspiredemoapp")
    .WithReference(customerDb)
    .WithReference(cache)
    .WithReference(insights)
    .WithReference(aspireDemoApi)
    .WithExternalHttpEndpoints();

// Angular: npm run start
if (builder.ExecutionContext.IsRunMode)
{
    builder.AddNpmApp("aspiredemoclient", "../globalazure.netaspire.client")
        .WithReference(aspireDemoApp)
        .WithHttpEndpoint(targetPort: 3000, env: "PORT");
}

builder.Build().Run();
