using Domain.Interfaces;

namespace gRPC.DataAccess.Models;

public class MessageModel : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string Sender { get; set; }
    public string Recipient { get; set; }
    public string Content { get; set; }
    public DateTime TimeStamp { get; set; }
}
