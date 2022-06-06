using Discord;

namespace MiniGames.Games.Minesweeper
{
    public class Fields : Item
    {
        public Emoji Emoji;
        public bool IsBomb;
        
        public Fields(int x, int y, Emoji emoji) : base(x, y)
        {
            Emoji = emoji;
        }
    }
}