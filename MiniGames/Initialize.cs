using System;
using System.ComponentModel.Design;
using System.Security.Authentication.ExtendedProtection;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MiniGames.Database;
using MiniGames.Games;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;


namespace MiniGames
{
    public class Initialize
    {
        private readonly CommandService _commands;
        private readonly DiscordSocketClient _client;
        private readonly IConfiguration _config;

        /// <summary>
        /// Ask if there are exisiting CommandService and DiscordSocketClient
        /// instance, If there are, we retrieve them and add them to the
        /// DI container; if not, we create our own.
        /// </summary>
        public Initialize(CommandService commands = null, DiscordSocketClient client = null, IConfiguration config = null)
        {
            _commands = commands ?? new CommandService();
            _client = client ?? new DiscordSocketClient();
            _config = config ?? new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json").Build();
            Log.Logger = new LoggerConfiguration()
                //.WriteTo.File("logs/csharpi.log", rollingInterval: RollingInterval.Day)
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();
        }

        public IServiceProvider BuildServiceProvider() => new ServiceCollection()
            .AddSingleton(_client)
            .AddSingleton(_commands)
            .AddSingleton(_config)
            .AddSingleton<CommandHandler>()
            .AddSingleton<GameFactory>()
            .AddDbContext<DatabaseHandler>()
            .AddSingleton<DatabaseBase>()
            .AddSingleton<LoggingService>()
            .AddLogging(configure => configure.AddSerilog())
            .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel.Information) //In future, allow user to set this from discord.
            .BuildServiceProvider();
    }
}