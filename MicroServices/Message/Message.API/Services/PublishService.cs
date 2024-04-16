using Domain.Dtos;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;

namespace Message.API.Services;
public class PublishService(ILogger<PublishService> logger, IMqttClient client) : IPublishService
{
    private readonly ILogger<PublishService> _logger = logger;
    private readonly IMqttClient _mqttClient = client;
    public async Task ConnectAsync()
    {
        var mqttFactory = new MqttFactory();    
        var client = mqttFactory.CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
            .WithClientId(Guid.NewGuid().ToString())
            .WithTcpServer("mosquitto", 1883) // Use "mosquitto" as the hostname since it's the service name in Docker Compose
            .WithCleanSession()
            .Build();

        await client.ConnectAsync(options, CancellationToken.None);
        _logger.LogInformation("Connected to MQTT broker successfully.");
    }

    public async Task PublisMessageAsync(MessageDto messageDto)
    {
        var topic = "messages";
        var payload = GetMessagePayload(messageDto);

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .Build();

        if (_mqttClient.IsConnected)
        {
            await _mqttClient.PublishAsync(message, CancellationToken.None);
            _logger.LogInformation($"Published message: {payload}");
        }
        _logger.LogError("failed to connect mqtt broker");
    }

    private string GetMessagePayload(MessageDto message)
    {
        // Convert message object to JSON 
        return JsonConvert.SerializeObject(message);
    }
}
