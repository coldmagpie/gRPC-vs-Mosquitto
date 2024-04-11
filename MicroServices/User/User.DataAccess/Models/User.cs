using Domain.Interfaces;

namespace DataAccess.Models;

public class User : IEntity<Guid>
{
    public Guid Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string NickName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
