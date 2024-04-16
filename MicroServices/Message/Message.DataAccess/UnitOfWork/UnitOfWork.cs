using DataAccess.Context;
using DataAccess.Repositories;
using Message.DataAccess.Repositories;

namespace Message.DataAccess.UnitOfWork;

public class UnitOfWork(ApplicationContext context, IMessageRepository messageRepository, IUserMessageRepository userMessageRepository) : IUnitOfWork, IDisposable
{
    private readonly ApplicationContext _context = context;
    public IMessageRepository _userRepository = messageRepository;
    public IUserMessageRepository _userMessageRepository = userMessageRepository;

    public IMessageRepository MessageRepository => _userRepository;
    public IUserMessageRepository UserMessageRepository => _userMessageRepository;


    private bool _disposed;

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
