using gRPC.DataAccess.Context;
using gRPC.DataAccess.Repositories;
using Grpc.server.Servers;
using Grpc.server.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var databaseName = Environment.GetEnvironmentVariable("DB_NAME") ?? "GrpcMessageDb";
var username = Environment.GetEnvironmentVariable("DB_USERSA") ?? "sa";
var password = Environment.GetEnvironmentVariable("DB_MSSQL_SA_PASSWORD") ?? "Pwd123!!1";

var connectionString =
    $"Data Source={host};Initial Catalog={databaseName};User ID={username};Password={password};Trusted_connection=False;TrustServerCertificate=True;";

builder.Services.AddLogging();
builder.Services.AddDbContext<MessageContext>(options =>
    options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5, // Adjust as needed
            maxRetryDelay: TimeSpan.FromSeconds(30), // Adjust as needed
            errorNumbersToAdd: null);
    }));

builder.Services.AddScoped<IGrpcMessageRepository, GrpcMessageRepository>();
builder.Services.AddScoped<CommunicationService>();
builder.Services.AddScoped<CommunicationServer>();

// Add services to the container.
builder.Services.AddGrpc();

var app = builder.Build();


// Resolve the service

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var chatServer = services.GetRequiredService<CommunicationServer>();
chatServer.Start();

app.MapGrpcService<CommunicationService>();
// Configure the HTTP request pipeline.

app.Run();
scope.Dispose();
