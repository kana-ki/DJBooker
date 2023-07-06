using System;
using System.Collections.Generic;
using Discord;

namespace DJBooker.Infrastructure;

public interface ICommandCartographer
{
    (SlashCommandBuilder[], Dictionary<string, Type>) Discover();
}