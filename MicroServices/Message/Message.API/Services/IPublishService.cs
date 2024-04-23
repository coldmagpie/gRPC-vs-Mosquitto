using Domain.Dtos;

namespace Message.API.Services;

public interface IPublishService
{
    Task PublishAsync(MessageDto messageDto);
    //Task PublisMessageAsync(MessageDto message);
}
