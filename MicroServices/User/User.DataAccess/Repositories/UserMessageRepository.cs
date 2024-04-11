using DataAccess.Context;
using DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace DataAccess.Repositories;
public class UserMessageRepository(ApplicationContext context) : IUserMessageRepository
{
    private readonly ApplicationContext _context = context;
    public async Task AddAsync(UserMessage entity)
    {
        await _context.AddAsync(entity);
    }

    public async Task DeleteAsync(Guid id)
    {
        var userMessage = await GetByIdAsync(id);
        if (userMessage != null)
        {
            _context.UserMessages.Remove(userMessage);
        }
    }

    public async Task<IEnumerable<UserMessage>> GetAllAsync()
    {
        return await _context.UserMessages.ToListAsync();
    }

    public async Task<UserMessage?> GetByIdAsync(Guid id)
    {
        return await _context.UserMessages.FirstOrDefaultAsync(x => x.Id.Equals(id));
    }

    public Task UpdateAsync(UserMessage entity)
    {
        _context.UserMessages.Update(entity);
        return Task.CompletedTask;
    }
}
