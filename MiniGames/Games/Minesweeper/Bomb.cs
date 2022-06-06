using System;
using Discord;
using Newtonsoft.Json.Serialization;

namespace MiniGames.Games.Minesweeper
{
    public class Bomb : Fields
    {
        private bool _visited = false;
        private readonly Emoji _initEmoji;
        public Bomb(int x, int y, Emoji emoji) : base(x, y, emoji)
        {
            _initEmoji = emoji;
        }

        public bool IsVisited()
        {
            return _visited;
        }

        public void SetVisited(bool visited)
        {
            _visited = visited;
            Emoji = _visited ? new Emoji("💣") : _initEmoji;
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null) return false;
            var other = (Bomb) obj;
            return CompareEquals(other);
        }

        private bool CompareEquals(Bomb other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override int GetHashCode()
        {
            var prime = 31;
            var result = 1;
            result *= prime + X;
            result *= prime + Y;
            return result;
        }
        
    }
}