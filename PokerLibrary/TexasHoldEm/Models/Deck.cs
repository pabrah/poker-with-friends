using System;
using System.Collections.Generic;
using System.Text;

namespace PokerLibrary.TexasHoldEm.Models
{
    public class Deck
    {
        public IReadOnlyList<Card> Cards { get; private set; }

        private List<Card> InternalCards { get; set; }

        public Deck()
        {
            CreateCards();
            ShuffleCards();
        }

        private void CreateCards()
        {
            InternalCards = new List<Card>();
            foreach(Rank rank in Enum.GetValues(typeof(Rank)))
            {
                foreach(Suit suit in Enum.GetValues(typeof(Suit)))
                {
                    InternalCards.Add(new Card(rank, suit));
                }
            }
        }

        public void ShuffleCards()
        {
            InternalCards.Shuffle();
            Cards = InternalCards;
        }
    }
}
