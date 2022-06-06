namespace MiniGames.Games
{
    public abstract class Item
    {
        public int X { get; }
        public int Y { get; }
        
        public Item (int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}