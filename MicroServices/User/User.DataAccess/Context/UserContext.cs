using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using User.DataAccess.Models;

namespace DataAccess.Context;
public class UserContext : DbContext
{
    public DbSet<UserModel> Users { get; set; } = null!;
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
        try
        {
            if (Database.GetService<IDatabaseCreator>() is not RelationalDatabaseCreator databaseCreator) return;
            if (!databaseCreator.CanConnect()) databaseCreator.Create();
            if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e}, DB could not be created: {e.Message}");
        }
    }
}
