using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolyPaint.Models
{
    public class Game
    {
        public string Name { get; set; }
        public string Mode { get; set; }
        public string NbPlayers { get; set; }

        public Game(string name, string mode, string nbPlayers)
        {
            this.Name = name;
            this.Mode = mode;
            this.NbPlayers = nbPlayers;
        }

    }
}
