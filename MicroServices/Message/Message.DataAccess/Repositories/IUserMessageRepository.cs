using Domain.Interfaces;
using Message.DataAccess.Models;

namespace Message.DataAccess.Repositories;

public interface IUserMessageRepository : IRepository<UserMessage, Guid>
{
}

