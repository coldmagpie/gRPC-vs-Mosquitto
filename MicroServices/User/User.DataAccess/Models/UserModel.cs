 using Domain.Interfaces;

namespace User.DataAccess.Models;

public class UserModel : IEntity<Guid>
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string NickName { get; set; } = null!;

    public string Password { get; set; } = null!;
}
