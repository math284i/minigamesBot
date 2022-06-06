using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MiniGames.Database;

namespace MiniGames.Games
{
    public abstract class GameBase : IGame, IDatabaseBase
    {
        protected readonly SocketCommandContext Context;
        private readonly DatabaseHandler _db;
        protected readonly ILogger Logger;
        public DatabaseBase DbBase { get; set; }

        protected GameBase(IServiceProvider services,SocketCommandContext context)
        {
            Context = context;
            _db = services.GetRequiredService<DatabaseHandler>();
            Logger = services.GetRequiredService<ILogger<LoggingService>>();
            DbBase = services.GetRequiredService<DatabaseBase>();
        }

        public abstract Task GameLoop(int coins);
        protected async Task GameFinished(GameData gameData)
        {
            var userName = Context.User.Username;
            var userData = GetDataFromDatabase(Context.User);
            if (!CheckIfUserExist(userData, Context)) return;
            if (gameData.Won)
            {
                userData.Coins += gameData.Coins;
                await SendEmbed(new string[]{$"Congratulation {userName} you won!"
                        , $"{userName} Your balance is now {userData.Coins} coin(s)"}
                        , gameData.Author, gameData.Color);
            }
            else
            {
                userData.Coins -= gameData.Coins;
                if (userData.Coins < 0) userData.Coins = 0;
                await SendEmbed(new string[]{$"{userName} You lose!"
                        , $"{userName} Your balance is now {userData.Coins} coin(s)"}
                        , gameData.Author, gameData.Color);
            }
            
            await _db.SaveChangesAsync();
        }

        protected async Task SendEmbed(string[] messages, string author, Color color)
        {
            //Build embed
            var embed = new EmbedBuilder()
            {
                Title = "Playing: " + author,
            };
            var description = messages.Aggregate("", (current, message) => current + (message + "\n"));
            embed.WithCurrentTimestamp()
                .WithColor(color)
                .WithDescription(description);
            //Send embed
            await Context.Channel.SendMessageAsync(embed: embed.Build());
        }

        public DatabaseData GetDataFromDatabase(SocketUser user)
        {
            return DbBase.GetDataFromDatabase(user).Result;
        }

        public bool CheckIfUserExist(DatabaseData userData, SocketCommandContext context)
        {
            return DbBase.CheckIfUserExist(userData, context);
        }
    }
}