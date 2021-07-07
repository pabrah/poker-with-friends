using PokerLibrary.TexasHoldEm.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace PokerLibrary.TexasHoldEm.Dtos
{
    public class BoardDto
    {
        //playerdto
        public string Code { get; set; }
        public int BetStack { get; set; }
        public int SmallBlindSeat { get; set; }
        public int BigBlindSeat { get; set; }
        public int SmallBlindSize { get; set; }
        public List<Card> CardsOnTable { get; set; }
    }
}
