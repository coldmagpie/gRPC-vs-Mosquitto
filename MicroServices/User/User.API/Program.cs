using DataAccess.Context;
using DataAccess.Repositories;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

var host = Environment.GetEnvironmentVariable("DB_HOST")??"localhost";
var databaseName = Environment.GetEnvironmentVariable("DB_DATABASE") ??"UserMessageDb";
var username = Environment.GetEnvironmentVariable("DB_USER")??"sa";
var password = Environment.GetEnvironmentVariable("DB_MSSQL_SA_PASSWORD") ?? "Pwd123!!3";

var connectionString =
    $"Data Source={host};Initial Catalog={databaseName};User ID={username};Password={password};Trusted_connection=False;TrustServerCertificate=True;";

builder.Services.AddDbContext<ApplicationContext>(option => option.UseSqlServer(connectionString));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserMessageRepository, UserMessageRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddLogging();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();
var logger=app.Services.GetRequiredService<ILogger<Program>>();
logger.LogInformation($"DBConnectionString:{connectionString}");
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
