using Aspire.Hosting;

var builder = DistributedApplication.CreateBuilder(args);

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
    .WithReference(cache);

builder.Build().Run();
