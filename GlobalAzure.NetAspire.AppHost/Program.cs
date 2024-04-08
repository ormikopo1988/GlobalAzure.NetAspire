using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

// Automatically provision an Application Insights resource
//var insights = builder.ExecutionContext.IsPublishMode
//    ? builder.AddAzureApplicationInsights("aspiredemoapplicationinsights")
//    : builder.AddConnectionString("aspiredemoapplicationinsights", "APPLICATIONINSIGHTS_CONNECTION_STRING");
var insights = builder.AddAzureApplicationInsights("aspiredemoapplicationinsights");

// Provisions an Azure SQL Database when published
var customerDb = builder
    .AddSqlServer("aspiredemosqlserver")
    .PublishAsAzureSqlDatabase()
    .AddDatabase("aspiredemodb");

// Provisions an Azure Redis Cache when published
var cache = builder
    .AddRedis("cache")
    .PublishAsAzureRedis();

builder
    .AddProject<Projects.GlobalAzure_NetAspire_Server>("aspiredemoapp")
    .WithReference(customerDb)
    .WithReference(cache)
    .WithReference(insights);

builder.Build().Run();
