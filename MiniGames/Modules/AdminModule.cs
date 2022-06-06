using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniGames.Database;
using MiniGames.Games;

namespace MiniGames.Modules
{
    /// <summary>
    /// <list type="bullet">
    ///<listheader>
    ///<term>Security levels</term>
    ///<description>0/1/2/3</description>
    ///</listheader>
    ///<item>
    ///<term>0</term>
    ///<description> - Casual user</description>
    ///</item>
    /// <item>
    ///<term>1</term>
    ///<description> - Admin</description>
    ///</item>
    ///<item>
    ///<term>2</term>
    ///<description> - Team leader</description>
    ///</item>
    ///<item>
    ///<term>3</term>
    ///<description> - Developer</description>
    ///</item>
    ///</list>
    /// 
    /// </summary>
    [Group("Admin")]
    public class AdminModule: ModuleBase<SocketCommandContext>, IDatabaseBase
    {
        private readonly DatabaseHandler _db;
        public DatabaseBase DbBase { get; set; }

        public AdminModule(IServiceProvider services)
        {
        _db = services.GetRequiredService<DatabaseHandler>();
        DbBase = services.GetRequiredService<DatabaseBase>();
        }

        [Command("AddCoins")]
        [Alias("ac")]
        public async Task AddCoinsAsync(
            [Summary("The user that receives")] SocketUser user, int amount)
        {
            var commandUserData = GetDataFromDatabase(Context.User);
            if (!CheckIfUserExist(commandUserData, Context)) return;

            commandUserData.Coins += amount;
            await _db.SaveChangesAsync();
            await ReplyAsync($"{user.Username} now has {commandUserData.Coins} coin(s)");
        }
        [Command("SetSecurityLevel")]
        [Alias("ssl")]
        public async Task SetSecurityLevel(
            [Summary("the user to edit")] SocketUser user, int securityLevel)
        {
            var commandUserData = GetDataFromDatabase(Context.User);
            if (!CheckIfUserExist(commandUserData, Context)) return;
            if (securityLevel >= commandUserData.SecurityLevel)
            {
                await ReplyAsync(
                    $"You don't have high enough Security level to set a level at **{DbBase.SecurityNames[securityLevel]}** \n" +
                            $"your current level is **{DbBase.SecurityNames[commandUserData.SecurityLevel]}** and you need at least " +
                            $"**{DbBase.SecurityNames[securityLevel + 1]}** or higher");
                return;
            }

            var userData = GetDataFromDatabase(user);
            if (!CheckIfUserExist(userData, Context)) return;
            userData.SecurityLevel = securityLevel;
            await _db.SaveChangesAsync();
            await ReplyAsync($"{user.Username} security level is now: **{DbBase.SecurityNames[userData.SecurityLevel]}**");
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