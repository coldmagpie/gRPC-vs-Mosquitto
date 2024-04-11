using DataAccess.Repositories;

namespace DataAccess.UnitOfWork;

public interface IUnitOfWork
{
    public IUserRepository UserRepository { get; }
    public IUserMessageRepository UserMessageRepository { get; }
    public Task SaveAsync();
    public void Dispose();
}
