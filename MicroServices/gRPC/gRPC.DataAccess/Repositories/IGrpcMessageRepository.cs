using Domain.Interfaces;
using gRPC.DataAccess.Models;

namespace gRPC.DataAccess.Repositories;

public interface IGrpcMessageRepository : IRepository<MessageModel, Guid>
{

}
