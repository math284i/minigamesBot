using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace MiniGames.Database
{
    public interface IDatabaseBase
    {
        DatabaseBase DbBase { get; set; }
        public DatabaseData GetDataFromDatabase(SocketUser user);
        public bool CheckIfUserExist(DatabaseData userData, SocketCommandContext context);
    }
}