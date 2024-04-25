using gRPC.server;
using Grpc.Core;
using Grpc.server.Services;

namespace Grpc.server.Servers;
public class CommunicationServer(CommunicationService communicationService, ILogger<CommunicationServer> logger)
{
    private const int ServerPort = 8082;

    private readonly Server _server = new()
    {
        Services = { Communication.BindService(communicationService) },
        Ports = { new ServerPort("0.0.0.0", ServerPort, ServerCredentials.Insecure) }
    };

    public void Start()
    {
        try
        {
            _server.Start();
            logger.LogInformation("GRPC server started");
        }
        catch (Exception e)
        {
            logger.LogError(e, "GRPC server could not start.");
        }
    }

    public async Task ShowDownAsync()
    {
        await _server.ShutdownAsync();
    }
}