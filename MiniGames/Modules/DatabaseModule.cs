using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniGames.Database;

namespace MiniGames.Modules
{
    public class DatabaseModule : ModuleBase<SocketCommandContext>, IDatabaseBase
    {
        private readonly DatabaseHandler _db;
        public DatabaseBase DbBase { get; set; }

        public DatabaseModule(IServiceProvider services)
        {
            _db = services.GetRequiredService<DatabaseHandler>();
            DbBase = services.GetRequiredService<DatabaseBase>();
        }

        [Command("Start")]
        public async Task StartDatabase()
        {
            var user = Context.User;
            var userFromDb = GetDataFromDatabase(user);
            if (userFromDb != null)
            {
                await ReplyAsync($"Hi {user.Username} you have already received ur starting bonus! " +
                                 $"\n You currently have {userFromDb.Coins} coin(s)");
            }
            else
            {
                await _db.AddAsync(new DatabaseData
                {
                    Id = user.Id,
                    Coins = 100,
                    SecurityLevel = 0
                });
                await _db.SaveChangesAsync();
                await ReplyAsync($"Hello {user.Username}, since this is ur first time here, imma " +
                                 $"give u 100 coins to start of with, happy gaming!");
            }
        }

        [Command("Balance")]
        [Alias("B")]
        public async Task BalanceAsync(SocketUser user = null)
        {
            var userInfo = user ?? Context.User;
            var userDataFromDB = GetDataFromDatabase(userInfo);
            await ReplyAsync($"Your balance is {userDataFromDB.Coins} coin(s)");
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