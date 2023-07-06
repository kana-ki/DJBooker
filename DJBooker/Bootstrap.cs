using Discord;
using Discord.WebSocket;
using DJBooker.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// Configuration
var config = new ConfigurationBuilder()
    .AddJsonFile("config.json", optional: true)
    .AddUserSecrets<Program>(optional: true)
    .AddEnvironmentVariables("DJBOOKER_")
    .Build();

// Discord Client Setup
var discordKey = config.GetValue<string>("DiscordBotToken");
if (discordKey == null) throw new Exception("Discord Bot Token not set!");
var client = new DiscordSocketClient();
client.Log += async msg => Console.WriteLine(msg.Message);
await client.LoginAsync(TokenType.Bot, discordKey);
await client.StartAsync();

// Service Collection
var serviceCollection = new ServiceCollection();
serviceCollection.AddSingleton<IConfiguration>(config);
serviceCollection.AddSingleton<ICommandBroker, CommandBroker>();
serviceCollection.AddSingleton<ICommandCartographer, CommandCartographer>();
serviceCollection.AddSingleton<ICommandBroker, CommandBroker>();
serviceCollection.AddSingleton(client);
var serviceProvider = serviceCollection.BuildServiceProvider();

client.Ready += () =>
{
    var commandBroker = serviceProvider.GetService<ICommandBroker>()!;
    commandBroker.AddFromAssembly();
    return commandBroker.RegisterAllGloballyAsync();
};

client.MessageReceived += message =>
{
    if (message.Channel is SocketDMChannel && !message.Author.IsBot)
        return message.Channel.SendMessageAsync("Meow!");
    return Task.CompletedTask;
};

client.SlashCommandExecuted += command =>
{
    var commandBroker = serviceProvider.GetService<ICommandBroker>()!;
    return commandBroker.HandleAsync(command);
};

await Task.Delay(Timeout.Infinite);
Console.WriteLine("Meow");