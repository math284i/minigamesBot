
using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.Logging;

namespace MiniGames.Games
{
    public class GameFactory
    {
        private readonly IServiceProvider _services;
        //private readonly LoggerFactory _loggerFactory;

        public GameFactory(IServiceProvider services)
        {
            _services = services;
            //_loggerFactory = loggerFactory;
        }

        public  IGame StartMinesweeper(int difficulty, SocketCommandContext context)
        {
            return new Minesweeper.Minesweeper(_services, difficulty, context);
        }
    }
}