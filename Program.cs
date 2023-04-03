using System;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.Entities;
using dotenv.net;
using Microsoft.Extensions.DependencyInjection;
using DSharpPlus.CommandsNext;
using DSharpPlus.SlashCommands;

namespace MyFirstBot
{
    public sealed class Program
    {
        static async Task Main(string[] args)
        {
            DotEnv.Load();
            var bot = new Program();
            Console.Clear();
            await bot.RunBotAsync();
        }

        private async Task RunBotAsync() {
            
            string? token = Environment.GetEnvironmentVariable("TOKEN");
            if (string.IsNullOrWhiteSpace(token))
            {
                Console.WriteLine("Please specify a token in the TOKEN environment variable.");
                Environment.Exit(1);
                return;
            }

            DiscordConfiguration config = new()
            {
                Token = token,
                TokenType = TokenType.Bot,
                Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents
            };

            DiscordClient client = new(config);
            DiscordActivity status = new("discord.gg/uckGjFUvec", ActivityType.Playing);

            ServiceCollection serviceCollection = new();
            serviceCollection.AddSingleton(Random.Shared); // We're using the shared instance of Random for simplicity.

            SlashCommandsConfiguration slashConfig = new() 
            {
                Services = new ServiceCollection().AddSingleton(Random.Shared).BuildServiceProvider()
            };

            CommandsNextConfiguration commandsConfig = new()
            {
                // Add the service provider which will allow CommandsNext to inject the Random instance.
                Services = serviceCollection.BuildServiceProvider(),
                StringPrefixes = new[] { "$" }
            };

            SlashCommandsExtension slashCommands = client.UseSlashCommands(slashConfig);
            CommandsNextExtension commandsNext = client.UseCommandsNext(commandsConfig);

            slashCommands.RegisterCommands(typeof(Program).Assembly);
            commandsNext.RegisterCommands(typeof(Program).Assembly);

            await client.ConnectAsync(status, UserStatus.Online);
            await Task.Delay(-1);
        }
    }
}