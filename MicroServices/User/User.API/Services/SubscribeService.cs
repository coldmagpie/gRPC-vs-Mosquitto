using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Server;
using System.Text;


namespace User.API.Services;
public class SubscribeService(ILogger<SubscribeService> logger) : ISubscribeService
{
    private readonly ILogger<SubscribeService> _logger = logger;
    public async Task SubscribeMessageAsync() 
    {
        var mqttFactory = new MqttFactory();
        IMqttClient client = mqttFactory.CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
            .WithClientId(Guid.NewGuid().ToString())
            .WithTcpServer("mosquitto", 1883) // Use "mosquitto" as the hostname since it's the service name in Docker Compose
            .WithCleanSession()
            .Build();

        var topicFilter = new MqttTopicFilterBuilder()
            .WithTopic("messages")
            .Build();
        var connectResult = await client.ConnectAsync(options, CancellationToken.None);
        if (connectResult.ResultCode == MqttClientConnectResultCode.Success)
        {
            _logger.LogInformation("Connected to MQTT broker successfully.");
            await client.SubscribeAsync(topicFilter);
        }
        
        client.ApplicationMessageReceivedAsync += e =>
        {
            _logger.LogInformation($"Received message: {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}");
            return Task.CompletedTask;
        };
        await client.ConnectAsync(options);
        await client.DisconnectAsync();

    }

}
