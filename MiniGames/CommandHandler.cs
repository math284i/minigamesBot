using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MiniGames
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commands;
        private readonly IServiceProvider _services;
        private readonly IConfiguration _config;
        private readonly ILogger _loqger;

        public CommandHandler(IServiceProvider services)
        {
            _services = services;
            _commands = _services.GetRequiredService<CommandService>();
            _client = _services.GetRequiredService<DiscordSocketClient>();                               
            _config = _services.GetRequiredService<IConfiguration>();
            _loqger = _services.GetRequiredService<ILogger<CommandHandler>>();
        }

        public async Task InitializeAsync()
        {
            // Hook the MessageReceived event into our command handler
            _client.MessageReceived += HandleCommandAsync;
            _commands.CommandExecuted += CommandExecutedAsync;
            await _commands.AddModulesAsync(
                assembly: Assembly.GetEntryAssembly(), 
                services: _services);
        }

        private async Task CommandExecutedAsync(Optional<CommandInfo> command, ICommandContext context, IResult result)
        {
            // if command isn't found
            if (!command.IsSpecified)
            {
                _loqger.LogError($"Command failed to execute for [{command.ToString()}] <-> [{result.ErrorReason}]!");
                return;
            }

            if (result.IsSuccess)
            {
                _loqger.LogInformation($"Command [{command.ToString()}] executed for [{context.User.Username}] on [{context.Channel}]");
                return;
            }

            await context.Channel.SendMessageAsync($"Sorry, .... something went wrong ->[{result.ErrorReason}]");
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            
            // Create a number to track where the prefix ends and the command begins
            var argPos = 0;
            
            // Determine if the message is a command based on the prefix and make sure no bots trigger commands
            if (!(message.HasCharPrefix(Convert.ToChar(_config["Prefix"]), ref argPos)) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos) ||
                message.Author.IsBot) return;

            // Create a WebSocket-based command context based on the message
            var context = new SocketCommandContext(_client, message);
            
            // Execute the command with the command context we just
            // created, along with the service provider for precondition checks.
            await _commands.ExecuteAsync(
                context: context
                , argPos: argPos
                , services: _services);
        }
    }
}