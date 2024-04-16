using DataAccess.Repositories;
using Message.DataAccess.Repositories;

namespace Message.DataAccess.UnitOfWork;

public interface IUnitOfWork
{
    public IMessageRepository MessageRepository { get; }
    public IUserMessageRepository UserMessageRepository { get; }
    public Task SaveAsync();
    public void Dispose();
}