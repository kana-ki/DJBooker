using Discord.WebSocket;
using DJBooker.Infrastructure;
using DJBooker.Infrastructure.Attributes;

namespace DJBooker.Commands;

[DiscordCommand("meow", "Meow at me DJ!")]
public class MeowCommand : ICommandHandler
{
    public Task HandleAsync(SocketSlashCommand slashCommand)
    {
        return slashCommand.RespondAsync("Meow to you too " + slashCommand.User.Mention + "!");
    }
}