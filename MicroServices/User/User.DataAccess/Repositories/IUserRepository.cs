using Domain.Interfaces;
using User.DataAccess.Models;

namespace DataAccess.Repositories;

public interface IUserRepository : IRepository<UserModel, Guid>
{

}
