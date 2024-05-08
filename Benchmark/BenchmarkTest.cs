using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using gRPC.server;
using MQTTnet.Client;
using MQTTnet.Protocol;
using MQTTnet;
using Newtonsoft.Json;
using BenchmarkDotNet.Jobs;

namespace Benchmark;

[SimpleJob( RunStrategy.ColdStart, RuntimeMoniker.Net80, launchCount: 1, warmupCount: 0, iterationCount: 50, baseline: true), MemoryDiagnoser]
[MinColumn, MaxColumn, MeanColumn, MedianColumn]
[RPlotExporter]

public class BenchmarkTest : TestBase
{
    [Benchmark]
    public void GrpcTest()
    {
        var message = new Message()
        {
            Sender = Guid.NewGuid().ToString(), 
            Recipient = Guid.NewGuid().ToString(), 
            Content = "test message"
        };

        GrpcClient.SendMessage(message);
    }

    [Benchmark]
    public void MqttTest()
    {

        var messageToSend = new Message()
        {
            Sender = Guid.NewGuid().ToString(),
            Recipient = Guid.NewGuid().ToString(),
            Content = "test message"
        };

        var topic = "messages";
        var topicFilter = new MqttTopicFilterBuilder()
            .WithTopic(topic)
            .Build();

        Mqttclient.SubscribeAsync(new MqttClientSubscribeOptionsBuilder().WithTopicFilter(topicFilter).Build()).GetAwaiter().GetResult();

        var payload = GetMessagePayload(messageToSend);

        var message = new MqttApplicationMessageBuilder()
            .WithTopic(topic)
            .WithPayload(payload)
            .WithQualityOfServiceLevel(MqttQualityOfServiceLevel.AtLeastOnce)
            .Build();

        Mqttclient.PublishAsync(message, CancellationToken.None).GetAwaiter().GetResult();
    }

    private string GetMessagePayload(Message message)
    {
        // Convert message object to JSON 
        return JsonConvert.SerializeObject(message);
    }
}