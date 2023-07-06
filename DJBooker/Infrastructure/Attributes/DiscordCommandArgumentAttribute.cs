using Discord;

namespace DJBooker.Infrastructure.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
public class DiscordCommandArgumentAttribute : Attribute
{
    public string Name { get; }
    public string Description { get; }
    public ApplicationCommandOptionType Type { get; }
    public bool Required { get; }

    public DiscordCommandArgumentAttribute(string name, string description, ApplicationCommandOptionType type)
    {
        Name = name;
        Description = description;
        Type = type;
    }
    
}