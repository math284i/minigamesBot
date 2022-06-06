using System;
using System.Collections.Generic;
using System.Linq;
using Discord;
using Microsoft.Extensions.Logging;

namespace MiniGames.Games.Minesweeper
{
    public class BombController
    {
        public List<Fields> Bombs;
        
        public void GenerateBombs(int amount, Fields[,] map, int width, int height)
        {
            Console.WriteLine($"Amount {amount}");
            var bombs = new List<Fields>();
            var hSet = new HashSet<Fields>();
            var rnd = new Random();
            
            while (amount > 0)
            {
                //Console.WriteLine($"PossibleBombX {possibleBombXpos.ToArray()}");
                //Console.WriteLine($"PossibleBombY {possibleBombYpos.ToArray()}");
                //var x = possibleBombXpos[rnd.Next(0, possibleBombXpos.Count) - 1];
                //var y = possibleBombYpos[rnd.Next(0, possibleBombXpos.Count) - 1];
                //possibleBombXpos.Remove(x);
                //possibleBombYpos.Remove(y);
                //bombs.Add(new Bomb(x, y));
                amount--;
            }

            Bombs = bombs;
        }

        public int CountBombs(int x, int y)
        {
            var amount = 0;
            
            //Check all neighbours if they are a bomb
            foreach (var bomb in Bombs)
            {
                //Top Left
                if (bomb.X.Equals(x - 1) && bomb.Y.Equals(y - 1)) amount++; 

                //Top Middle
                if (bomb.X.Equals(x) && bomb.Y.Equals(y - 1)) amount++;
                
                //Top Right
                if (bomb.X.Equals(x + 1) && bomb.Y.Equals(y - 1)) amount++;
          
                //Middle Left
                if (bomb.X.Equals(x - 1) && bomb.Y.Equals(y)) amount++;
                
                //Middle Right
                if (bomb.X.Equals(x + 1) && bomb.Y.Equals(y)) amount++;
                
                //Bottom left
                if (bomb.X.Equals(x - 1) && bomb.Y.Equals(y + 1)) amount++;
                
                //Bottom Middle
                if (bomb.X.Equals(x) && bomb.Y.Equals(y + 1)) amount++;
                
                //Bottom Right
                if (bomb.X.Equals(x + 1) && bomb.Y.Equals(y + 1)) amount++;
            }
            
            return amount;
        }
        
    }
}