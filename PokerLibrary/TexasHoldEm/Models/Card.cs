using System;
using System.Collections.Generic;
using System.Text;

namespace PokerLibrary.TexasHoldEm.Models
{
    public class Card : IEquatable<Card>
    {
		public Rank Rank { get; private set; }
		public Suit Suit { get; private set; }

		private static readonly int[] rankPrimes = new int[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41 };
		private static readonly int[] suitPrimes = new int[] { 43, 47, 53, 59 };

		public int PrimeRank { get { return rankPrimes[(int)Rank]; } }
		public int PrimeSuit { get { return suitPrimes[(int)Suit]; } }

		public Card(Rank rank, Suit suit)
        {
			Rank = rank;
			Suit = suit;
        }

		public bool Equals(Card other)
		{
			return Rank == other.Rank && Suit == other.Suit;
		}

		public int GetHashCode(Card c)
		{
			return c.PrimeRank * c.PrimeSuit;
		}

	}
}
