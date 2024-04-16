using Domain.Dtos;
using MQTTnet.Client;

namespace Message.API.Services;

public interface IPublishService
{
    Task ConnectAsync();
    Task PublisMessageAsync(MessageDto message);
}
