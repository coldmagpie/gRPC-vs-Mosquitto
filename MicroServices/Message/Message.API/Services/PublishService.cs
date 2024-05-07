using Domain.Dtos;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;
using Newtonsoft.Json;

namespace Message.API.Services;
public class PublishService(ILogger<PublishService> logger) : IPublishService
{
    private readonly ILogger<PublishService> _logger = logger;
    public async Task PublishAsync(MessageDto messageDto)
    {
        var mqttFactory = new MqttFactory();
        var client = mqttFactory.CreateMqttClient();

        var options = new MqttClientOptionsBuilder()
            .WithClientId(Guid.NewGuid().ToString())
            .WithTcpServer("mosquitto", 1883) // Use "mosquitto" as the hostname since it's the service name in Docker Compose
            .WithCleanSession()
            .Build();

        var result = await client.ConnectAsync(options, CancellationToken.None);

        if (result.ResultCode != MqttClientConnectResultCode.Success)
        {
            _logger.LogError("Can not connect to MQTT broker");
            return;
               
        }
        _logger.LogInformation("Connected to MQTT broker successfully.");
       
        var topic = "messages";
        var payload = GetMessagePayload(messageDto);

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

        try
        {
            // Publish the message
            await client.PublishAsync(message, CancellationToken.None);
            _logger.LogInformation($"Published message: {payload}");
            Thread.Sleep(1000);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to publish message to MQTT broker.");
        }
    }
    private string GetMessagePayload(MessageDto message)
    {
        // Convert message object to JSON 
        return JsonConvert.SerializeObject(message);
    }
}
