using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;

namespace MiniGames.Games.Minesweeper
{
    public class Minesweeper : GameBase
    {
        private int _width;
        private int _height;
        private BombController _bombController;
        private Fields[,] _map;
        private readonly int _difficulty;
        
        //Constants
        private static readonly Emoji UNDISCOCVEREDTILEEMOTE = new Emoji("⬛");
        private static readonly Emoji DISCOVEREDTILEEMOTE = new Emoji("");
        private const string AUTHOR = "Minesweeper";
        private static readonly Color MAINCOLOR = Color.Blue;

        public Minesweeper(IServiceProvider services, int difficulty, SocketCommandContext context) 
            : base(services, context)
        {
            _difficulty = difficulty;
        }

        public override async Task GameLoop(int coins)
        {
            InitializeGameObjects();
            //Fill in map:
            _map = new Fields[_width, _height];
            for (var i = 0; i < _width; i++)
            {
                for (var j = 0; j < _height; j++)
                {
                    _map[i, j] = new Fields(i, j, UNDISCOCVEREDTILEEMOTE);
                }
            }

            await PrintMap();
            
            
            
            
            //Game finished
            var gameData = new GameData
            {
                Author = AUTHOR,
                Coins = coins,
                Color = MAINCOLOR,
                Won = true, //Fix this
            };
            await GameFinished(gameData);
        }

        public async Task PrintMap()
        {
            var s = "```\n";
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _height; j++)
                {
                    s += $"{_map[i, j].Emoji}";
                }

                s += "\n";
            }

            s += "```";
            
            //Build embed
            var embed = new EmbedBuilder()
            {
                Title = "Playing: " + AUTHOR,
            };
            var description = s;
            embed.WithCurrentTimestamp()
                .WithColor(MAINCOLOR)
                .WithDescription(description);
            //Send embed
            await Context.Channel.SendMessageAsync(embed: embed.Build());
            
        }

        private int GetBombCount()
        {
            //15 % of tiles should be bombs
            return 1 + Convert.ToInt32(_width * _height * 0.15d); 
        }

        private void InitializeGameObjects()
        {
            _bombController = new BombController();
            _bombController.GenerateBombs(GetBombCount(), _map, _width, _height);
            switch (_difficulty)
            {
                case 1: 
                    _width = 10;
                    _height = 10;
                    break;
            }
        }

        private void PathChecker(int x, int y)
        {
            if (x > _width - 1 
                || x < 0 
                || y > _height - 1 
                || y < 0 
                || _map[x, y].Emoji.Equals(UNDISCOCVEREDTILEEMOTE)) return;

            
       
        }
        
        /*

        public async Task test()
        {
            var test = Context.Channel.Name;
        }

        public async Task PrintMap()
        {
            
        }

        public async Task PrintFinalMap()
        {
            
        }

        public async Task PathChecker()
        {
            
        }
        */
        
    }
}