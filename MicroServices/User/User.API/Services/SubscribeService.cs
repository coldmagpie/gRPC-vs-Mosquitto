using Domain.Dtos;
using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json;
using System.Text;

namespace User.API.Services;
public class SubscribeService(ILogger<SubscribeService> logger) : ISubscribeService
{
    private readonly ILogger<SubscribeService> _logger = logger;
    

    public async Task<string> SubscribeMessageAsync(Guid id) 
    {
        string receivedMessageContent = null;
        var mqttFactory = new MqttFactory();

        var client = mqttFactory.CreateMqttClient();
        var options = new MqttClientOptionsBuilder()
            .WithClientId(Guid.NewGuid().ToString())
            .WithTcpServer("mosquitto", 1883) // Use "mosquitto" as the hostname since it's the service name in Docker Compose
            .WithCleanSession()
            .Build();

        var topicFilter = new MqttTopicFilterBuilder()
            .WithTopic("messages")
            .Build();

        var result = await client.ConnectAsync(options, CancellationToken.None);

        if (result.ResultCode != MqttClientConnectResultCode.Success)
        {
            _logger.LogError("Can not connect to MQTT broker");
            return null;
        }
        _logger.LogInformation("Connected to MQTT broker successfully.");

        await client.SubscribeAsync(new MqttClientSubscribeOptionsBuilder().WithTopicFilter(topicFilter).Build());

        client.ApplicationMessageReceivedAsync +=  e  =>
        {
            // Process received message
            var payload = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);
            _logger.LogInformation($"Received message: {payload}");

            var userMessage = JsonConvert.DeserializeObject<MessageDto>(payload);
            if (!userMessage.Sender.Equals(id))
            {
                _logger.LogInformation("no message recieved for user");
            }
            receivedMessageContent = userMessage.Content;
            _logger.LogInformation(userMessage.Content);
            return Task.CompletedTask;
        };
        return receivedMessageContent;
    }  
    
}
