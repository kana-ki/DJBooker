using Discord.WebSocket;

namespace DJBooker.Infrastructure;

public interface ICommandHandler
{
    Task HandleAsync(SocketSlashCommand slashCommand);
}

