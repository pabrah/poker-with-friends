using System;
using System.Collections.Generic;
using System.Text;

namespace PokerLibrary.TexasHoldEm.Models
{
    public class Player
    {
        public Hand Hand { get; set; }
        public HandStrength HandStrength { get; set; }
        public string Name { get; set; }
        public int CurrentBet { get; set; }
        public int Stack { get; set; }
        public int Seat { get; set; }
    }
}
