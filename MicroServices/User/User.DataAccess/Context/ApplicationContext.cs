using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace DataAccess.Context;
public class ApplicationContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<UserMessage> UserMessages { get; set; } = null!;
    ILogger<ApplicationContext> _logger;
    public ApplicationContext(DbContextOptions<ApplicationContext> options,ILogger<ApplicationContext> logger) : base(options)
    {
        _logger = logger;
        try
        {
            if (Database.GetService<IDatabaseCreator>() is not RelationalDatabaseCreator databaseCreator) return;
            if (!databaseCreator.CanConnect()) databaseCreator.Create();
            if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"DB could not be created: {e.Message}");
        }
    }
}
