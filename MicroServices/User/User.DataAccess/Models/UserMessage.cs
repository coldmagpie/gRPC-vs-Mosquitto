using Domain.Interfaces;

namespace DataAccess.Models;
public class UserMessage : IEntity<Guid>
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid MessageId { get; set; }
}
