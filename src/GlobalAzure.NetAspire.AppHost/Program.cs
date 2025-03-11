using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Automatically provision an Application Insights resource
var insights = builder.ExecutionContext.IsPublishMode
    ? builder.AddAzureApplicationInsights("aspiredemoapplicationinsights")
    : builder.AddConnectionString("aspiredemoapplicationinsights", "APPLICATIONINSIGHTS_CONNECTION_STRING");

// Provisions an Azure SQL Database when published
var customerDb = builder
    .AddAzureSqlServer("aspiredemosqlserver")
    .RunAsContainer()
    .AddDatabase("aspiredemodb");

// Provisions an Azure Redis Cache when published
var cache = builder
    .AddAzureRedis("cache")
    .RunAsContainer();

var aspireDemoApi = builder
    .AddProject<Projects.GlobalAzure_NetAspire_Api>("aspiredemoapi")
    .WithReference(insights);

var aspireDemoApp = builder
    .AddProject<Projects.GlobalAzure_NetAspire_Server>("aspiredemoapp")
    .WithReference(customerDb)
    .WaitFor(customerDb)
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(insights)
    .WithReference(aspireDemoApi)
    .WaitFor(aspireDemoApi)
    .WithExternalHttpEndpoints();

// Angular: npm run start
if (builder.ExecutionContext.IsRunMode)
{
    builder.AddNpmApp("aspiredemoclient", "../globalazure.netaspire.client")
        .WithReference(aspireDemoApp)
        .WaitFor(aspireDemoApp)
        .WithHttpEndpoint(env: "PORT");
}

await builder.Build().RunAsync();
