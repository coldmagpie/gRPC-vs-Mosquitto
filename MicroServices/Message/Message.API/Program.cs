using DataAccess.Context;
using DataAccess.Repositories;
using Message.API.Logger;
using Message.API.Services;
using Message.DataAccess.Repositories;
using Message.DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using MQTTnet.Adapter;
using MQTTnet.Client;
using MQTTnet.Diagnostics;
using MQTTnet.Implementations;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "127.0.0.1";
var databaseName = Environment.GetEnvironmentVariable("DB_DATABASE") ?? "MessageDb";
var username = Environment.GetEnvironmentVariable("DB_USER") ?? "sa";
var password = Environment.GetEnvironmentVariable("DB_MSSQL_SA_PASSWORD") ?? "Pwd123!!3";

var connectionString =
    $"Data Source={host};Initial Catalog={databaseName};User ID={username};Password={password};Trusted_connection=False;TrustServerCertificate=True;";

builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlServer(connectionString, sqlServerOptionsAction: sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 5, // Adjust as needed
            maxRetryDelay: TimeSpan.FromSeconds(30), // Adjust as needed
            errorNumbersToAdd: null);
    }));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();
builder.Services.AddScoped<IUserMessageRepository, UserMessageRepository>();
// Register MQTT client adapter factory
builder.Services.AddSingleton<IMqttClientAdapterFactory, MqttClientAdapterFactory>();

// Register MQTT client
builder.Services.AddScoped<IMqttClient, MqttClient>();
builder.Services.AddSingleton<IMqttNetLogger, MqttNetLogger>();

builder.Services.AddScoped<IPublishService, PublishService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
