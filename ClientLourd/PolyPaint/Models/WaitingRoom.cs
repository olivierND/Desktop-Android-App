using PolyPaint.DataModels;

namespace PolyPaint.Models
{
    public class WaitingRoom
    {
        public string Name => Channel?.name;

        public int NbPlayer { get; set; } = 0;

        public Channel Channel { get; set; }

        public GameMode Mode { get; set; }

        public Player Host { get; set; }  // Member 1 of team 1 by default
        public Player TeamOnePlayerTwo { get; set; }  
        public Player TeamTwoPlayerOne { get; set; }  
        public Player TeamTwoPlayerTwo { get; set; }  
    }
}