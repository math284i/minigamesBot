using System;
using System.ComponentModel.DataAnnotations;

namespace MiniGames.Database
{
    public class DatabaseData
    {
        [Key]
        public ulong Id { get; init; }
        public int Coins { get; set; }
        public int SecurityLevel { get; set; }
    }
}