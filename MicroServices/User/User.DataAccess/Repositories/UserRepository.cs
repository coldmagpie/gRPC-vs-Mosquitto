using DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using User.DataAccess.Models;

namespace DataAccess.Repositories;

public class UserRepository(UserContext context) : IUserRepository
{
    private readonly UserContext _context = context;
    public async Task AddAsync(UserModel entity)
    {
        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var user = await GetByIdAsync(id);
        if (user is not null)
        {
            _context.Users.Remove(user);
        }
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<UserModel>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<UserModel?> GetByIdAsync(Guid id)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id.Equals(id));
    }

    public Task UpdateAsync(UserModel entity)
    {
        _context.Users.Update(entity);
        _context.SaveChangesAsync();
        return Task.CompletedTask;
    }
}
