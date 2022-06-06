using System;
using System.IO;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;

namespace MiniGames
{
    public class Program
    {
        private DiscordSocketClient _client;
        private IServiceProvider _service;
        private IConfiguration _config;

        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private async Task MainAsync()
        { 
            _service = new Initialize().BuildServiceProvider();
            
            _client = _service.GetRequiredService<DiscordSocketClient>();
            _config = _service.GetRequiredService<IConfiguration>();

            _service.GetRequiredService<LoggingService>();
              
            await _client.LoginAsync(TokenType.Bot, _config["Token"]);
            await _client.StartAsync();
            
            await _service.GetRequiredService<CommandHandler>().InitializeAsync();
            // Block this task until the program is closed.
            await Task.Delay(-1);
        }
    }
}