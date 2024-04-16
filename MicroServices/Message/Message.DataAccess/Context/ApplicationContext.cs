using DataAccess.Models;
using Message.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace DataAccess.Context;
public class ApplicationContext : DbContext
{
    public DbSet<MessageModel> Messages { get; set; } = null!;
    public DbSet<UserMessage> UserMessages { get; set; } = null!;
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        try
        {
            if (Database.GetService<IDatabaseCreator>() is not RelationalDatabaseCreator databaseCreator) return;
            if (!databaseCreator.CanConnect()) databaseCreator.Create();
            if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
