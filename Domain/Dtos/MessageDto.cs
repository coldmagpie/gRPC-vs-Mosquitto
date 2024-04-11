using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;
public class MessageDto
{
    [Required]
    public Guid Sender { get; set; }
    [Required]
    public string Content { get; set; }
    [Required]
    public Guid Recipient { get; set; }
    public DateTime TimeStamp { get; set; } = DateTime.UtcNow;
}
