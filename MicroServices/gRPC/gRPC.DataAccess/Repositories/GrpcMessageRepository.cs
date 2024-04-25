using gRPC.DataAccess.Context;
using gRPC.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace gRPC.DataAccess.Repositories;
public class GrpcMessageRepository(MessageContext context) : IGrpcMessageRepository
{
    private readonly MessageContext _context = context;

    public async Task<MessageModel?> GetByIdAsync(Guid id)
    {
        return await _context.Messages.FirstOrDefaultAsync(m => m.Id.Equals(id));
    }

    public async Task<IEnumerable<MessageModel>> GetAllAsync()
    {
        return await _context.Messages.ToListAsync();
    }

    public async Task AddAsync(MessageModel entity)
    {
        await _context.Messages.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(MessageModel entity)
    {
        var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id.Equals(entity.Id));
        message.Content = entity.Content;
        message.TimeStamp = entity.TimeStamp;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id.Equals(id));
        _context.Messages.Remove(message);
        await _context.SaveChangesAsync();
    }
}
