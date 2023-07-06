using Discord.WebSocket;

namespace DJBooker.Infrastructure;
    
public interface ICommandBroker
{
    void AddFromAssembly();
    Task HandleAsync(SocketSlashCommand context);
    Task RegisterAllGloballyAsync();
}
