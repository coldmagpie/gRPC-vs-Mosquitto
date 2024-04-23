using DataAccess.Repositories;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.server;

namespace gRPC.server.Services;

public class MessageService
{
    private readonly IMessageRepository _repository;

}
