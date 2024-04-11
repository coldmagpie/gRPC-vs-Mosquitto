using DataAccess.Context;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;

public class MessageRepository(MessageContext context) : IMessageRepository
{
    private readonly MessageContext _context = context;
    public async Task AddAsync(Message entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var message = await GetByIdAsync(id);
        if (message != null)
        {
            _context.Messages.Remove(message);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<Message>> GetAllAsync()
    {
        return await _context.Messages.ToListAsync();
    }

    public async Task<Message?> GetByIdAsync(Guid id)
    {
        return await _context.Messages.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task UpdateAsync(Message entity)
    {
        _context.Messages.Update(entity);
        _context.SaveChanges();
        return Task.CompletedTask;
    }
}
