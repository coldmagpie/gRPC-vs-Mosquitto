using Domain.Interfaces;

namespace DataAccess.Models;
public class Message : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid Sender { get; set; }
    public string Content { get; set; }
    public Guid Recipient { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}
