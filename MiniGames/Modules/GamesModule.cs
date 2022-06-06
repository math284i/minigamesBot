using System;
using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using MiniGames.Games;

namespace MiniGames.Modules
{
    public class GamesModule : ModuleBase<SocketCommandContext>
    {
        private readonly GameFactory _gameFactory;

        public GamesModule(IServiceProvider services)
        {
            _gameFactory = services.GetRequiredService<GameFactory>();
        }

        [Command("MineSweeper")]
        [Alias("ms")]
        public async Task MineSweeperAsync(int coins, int difficulty = 1) //In Future take in coins and optional difficulty
        {
            if (difficulty is > 3 or < 1)
            {
                await ReplyAsync();
                return;
            }
            var minesweeper = _gameFactory.StartMinesweeper(difficulty, Context);
            await minesweeper.GameLoop(coins);
        }
        
    }
}