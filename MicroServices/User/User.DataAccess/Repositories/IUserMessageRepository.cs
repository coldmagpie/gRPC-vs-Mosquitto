using DataAccess.Models;
using Domain.Interfaces;

namespace DataAccess.Repositories;
public interface IUserMessageRepository : IRepository<UserMessage, Guid>
{
}
