using gRPC.server;
using Grpc.Net.Client;
using MQTTnet.Client;
using MQTTnet;

namespace Benchmark;

public class TestBase
{
    public Communication.CommunicationClient GrpcClient;
    public IMqttClient Mqttclient;
    public TestBase()
    {
        SetUpGrpcConfiguration();
        SetUpMqttConfiguration();
    }

    public void SetUpGrpcConfiguration()
    {
        var channel = GrpcChannel.ForAddress("http://localhost:8082");

        // Create a client for your gRPC service
        GrpcClient = new Communication.CommunicationClient(channel);
        try
        {
            channel.ConnectAsync().GetAwaiter().GetResult();
            while (channel.State != Grpc.Core.ConnectivityState.Ready)
            {
                Thread.Sleep(1000);
            }
            Console.WriteLine("channel state is:" + channel.State);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public void SetUpMqttConfiguration()
    {
        var mqttFactory = new MqttFactory();
        Mqttclient = mqttFactory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithClientId(Guid.NewGuid().ToString())
            .WithTcpServer("localhost", 1883)
            .WithCleanSession()
            .Build();

        var result = Mqttclient.ConnectAsync(options, CancellationToken.None).GetAwaiter().GetResult();

        if (result.ResultCode != MqttClientConnectResultCode.Success)
        {
            Console.WriteLine("Can not connect to MQTT broker");
        }
        Console.WriteLine("Connected to MQTT broker successfully.");
    }
}