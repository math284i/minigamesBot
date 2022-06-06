using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.WebSocket;
using System.Linq;
using Discord.Commands;
using Microsoft.AspNetCore.CookiePolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MiniGames.Database
{
    public class DatabaseBase
    {
        private readonly DatabaseHandler _db;
        public readonly IDictionary SecurityNames;
        private readonly IConfiguration _config;
        
        public DatabaseBase(IServiceProvider services)
        {
            _db = services.GetRequiredService<DatabaseHandler>();
            _config = services.GetRequiredService<IConfiguration>();
            SecurityNames = new Dictionary<int, string>()
            {
                {0, "Normie"},
                {1, "Admin"},
                {2, "Team leader"},
                {3, "Developer"}
            };
        }

        public async Task<DatabaseData> GetDataFromDatabase(SocketUser user)
        {
            var users = await _db.UsersById.ToListAsync();
            return users.FirstOrDefault(u => u.Id.Equals(user.Id));
        }

        public bool CheckIfUserExist(DatabaseData userData, SocketCommandContext context)
        {
            if (userData != null) return true;
            context.Channel.SendMessageAsync($"I couldn't find {context.User.Username} in the database! \n" +
                                             $" Try typing {_config["Prefix"]}Start");
            return false;
        }
    }
}