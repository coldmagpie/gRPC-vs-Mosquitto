using DataAccess.Context;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class MessageRepository(ApplicationContext context) : IMessageRepository
{
    private readonly ApplicationContext _context = context;
    public async Task AddAsync(MessageModel entity)
    {
        await _context.AddAsync(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var message = await _context.Messages.FirstOrDefaultAsync(x => x.Id.Equals(id));
        if (message != null)
        {
            _context.Messages.Remove(message);
        }
    }

    public async Task<IEnumerable<MessageModel>> GetAllAsync()
    {
        return await _context.Messages.ToListAsync();
    }

    public async Task<MessageModel> GetByIdAsync(Guid id)
    {
        return await _context.Messages.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task UpdateAsync(MessageModel entity)
    {
        _context.Messages.Update(entity);
        return Task.CompletedTask;
    }
}
