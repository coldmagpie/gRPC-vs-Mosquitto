using gRPC.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace gRPC.DataAccess.Context;
public partial class MessageContext : DbContext
{
    public virtual DbSet<MessageModel> Messages { get; set; } = null!;
    public MessageContext(DbContextOptions<MessageContext> options) : base(options)
    {
        try
        {
            if (Database.GetService<IDatabaseCreator>() is RelationalDatabaseCreator databaseCreator)
            {
                if (!databaseCreator.CanConnect()) databaseCreator.Create();
                if (!databaseCreator.HasTables()) databaseCreator.CreateTables();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

}
