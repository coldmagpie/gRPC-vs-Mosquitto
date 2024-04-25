using gRPC.server;
using Grpc.Net.Client;

// Create a gRPC channel to connect to the server
var channel = GrpcChannel.ForAddress("http://grpc.server:8082");

// Create a client for your gRPC service
var client = new Communication.CommunicationClient(channel);
await channel.ConnectAsync();
while (channel.State != Grpc.Core.ConnectivityState.Ready)
{
    Thread.Sleep(1000);
}
Console.WriteLine("channel state is:" + channel.State);

// Call a service method
var request = new Message() { MessageId = Guid.NewGuid().ToString(), Sender = "Maggie", Recipient = "Jessica", Content = "Hi, Jessica, do you have time tomorrow?" };
var response = client.SendMessage(request);

// Handle the response
Console.WriteLine("Response received: " + response);
