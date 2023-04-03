using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.SlashCommands;
using DSharpPlus.Entities;
using DSharpPlus;

namespace PingCommands
{
    // Inherit from BaseCommandModule so that CommandsNext can recognize this class as a command.
    public sealed class PingCommand : BaseCommandModule
    {
        [Command("ping"), Description("Pings the bot and returns the gateway latency."), Aliases("pong")]
        [SuppressMessage("Style", "IDE0022", Justification = "Paragraph.")]
        public async Task PingAsync(CommandContext context)
        {
            await context.RespondAsync($"Pong! The gateway latency is {context.Client.Ping}ms.");
        }
    }

    public sealed class PingSlashCommand : ApplicationCommandModule
    {
        [SlashCommand("ping", "Pings the bot and returns the gateway latency. (slash)")]
        [SuppressMessage("Style", "IDE0022", Justification = "Paragraph.")]
        public async Task PingAsync(InteractionContext context)
        {
            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"Pong! The gateway latency is {context.Client.Ping}ms."));
        }
    }
}