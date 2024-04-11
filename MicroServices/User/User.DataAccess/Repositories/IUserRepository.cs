using DataAccess.Models;
using Domain.Interfaces;

namespace DataAccess.Repositories;

public interface IUserRepository : IRepository<User, Guid>
{

}
