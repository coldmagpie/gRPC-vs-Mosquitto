
namespace User.API.Services;
public interface ISubscribeService
{
    Task<string> SubscribeMessageAsync(Guid id);
}
