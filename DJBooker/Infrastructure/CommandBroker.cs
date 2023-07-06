using Discord;
using Discord.WebSocket;
using DJBooker.Utils;

using Microsoft.Extensions.DependencyInjection;

namespace DJBooker.Infrastructure;

internal class CommandBroker : ICommandBroker
{
    private readonly DiscordSocketClient _discordClient;
    private readonly List<SlashCommandProperties> _commands;
    private readonly TypeMap<ICommandHandler> _handlers;
    private readonly ICommandCartographer _cartographer;

    public CommandBroker(IServiceProvider serviceProvider)
    {
        this._commands = new();
        this._handlers = new(serviceProvider);
        this._cartographer = serviceProvider.GetService<ICommandCartographer>()!;
        this._discordClient = serviceProvider.GetService<DiscordSocketClient>()!;
    }

    public void AddFromAssembly()
    {
        var (commands, handlers) = this._cartographer.Discover();
        foreach (var handler in handlers)
            this._handlers.Add(handler.Key, handler.Value);
        this._commands.AddRange(commands.Select(c => c.Build()));
    }

    public Task RegisterAllGloballyAsync() =>
        this._discordClient.BulkOverwriteGlobalApplicationCommandsAsync(this._commands.ToArray());

    public async Task HandleAsync(SocketSlashCommand command)
    {
        var handler = this._handlers.Activate(command.CommandName);
        if (handler == null)
        {
            var commandPath = this.GetCommandName(command);
            handler = this._handlers.Activate(commandPath);
        }

        if (handler != null)
            await handler.HandleAsync(command);
    }

    private string GetCommandName(SocketSlashCommand slashCommand)
    {
        var commandPath = slashCommand.CommandName;
        var command = this._commands.FirstOrDefault(c => c.Name.Value == commandPath);
        if (command == null) return commandPath;
        for (var i = 0; true; i++)
        {
            var subCommandName = slashCommand.Data.Options.Skip(i).FirstOrDefault()?.Name;
            if (subCommandName == null) break;
            var subCommand = command.Options.Value?.FirstOrDefault(o => o.Name == subCommandName);
            if (subCommand == null) break;
            if (subCommand.Type == ApplicationCommandOptionType.SubCommandGroup)
            {
                commandPath += " " + subCommandName;
                continue;
            }

            if (subCommand.Type == ApplicationCommandOptionType.SubCommand)
                commandPath += " " + subCommandName;
            break;
        }

        return commandPath;
    }
}
