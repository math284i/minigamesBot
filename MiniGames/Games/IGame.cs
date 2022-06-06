using System.Threading.Tasks;

namespace MiniGames.Games
{
    public interface IGame
    {
        Task GameLoop(int coins);
    }
}