using DataAccess.Models;
using Domain.Interfaces;

namespace DataAccess.Repositories;

public interface IMessageRepository : IRepository<MessageModel, Guid>
{
}
