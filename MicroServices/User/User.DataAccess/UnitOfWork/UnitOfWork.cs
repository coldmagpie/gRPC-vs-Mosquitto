
using DataAccess.Context;
using DataAccess.Repositories;

namespace DataAccess.UnitOfWork;

public class UnitOfWork(ApplicationContext context, IUserRepository userRepository, IUserMessageRepository userMessageRepository) : IUnitOfWork, IDisposable
{
    private readonly ApplicationContext _context = context;
    public IUserRepository _userRepository = userRepository;
    public IUserMessageRepository _userMessageRepository = userMessageRepository;

    public IUserRepository UserRepository => _userRepository;
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
