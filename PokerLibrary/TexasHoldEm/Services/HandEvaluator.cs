using PokerLibrary.TexasHoldEm.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PokerLibrary.TexasHoldEm.Services
{
    public static class HandEvaluator
    {

		public static HandStrength EvaluateBestHand(List<Card> cardsOnTable, Hand hand)
        {
			if (cardsOnTable.Count != 5 || hand.Cards.Count != 2)
				throw new ArgumentException($"Cards on board {cardsOnTable.Count} and on hand {hand.Cards.Count} is not 5 and 2 respectively");
			var handStrengths = new List<HandStrength>();

			var allCards = new List<Card>(cardsOnTable);
			allCards.AddRange(hand.Cards);
			var cardPermutations = allCards.GetPermutations(5);
			
			foreach(var cardList in cardPermutations)
            {
				handStrengths.Add(GetStrength(cardList));
            }

			return handStrengths.Max();
        }

		public static HandStrength GetStrength(IEnumerable<Card> cards)
		{
			if (cards.Count() == 5)
			{
				var strength = new HandStrength
				{
					Kickers = new List<int>()
				};

				cards = cards.OrderBy(card => card.PrimeRank * 100 + card.PrimeSuit).ToList();

				int rankProduct = cards.Select(card => card.PrimeRank).Aggregate((acc, r) => acc * r);
				int suitProduct = cards.Select(card => card.PrimeSuit).Aggregate((acc, r) => acc * r);

				bool straight =
					rankProduct == 8610         // 5-high straight
					|| rankProduct == 2310      // 6-high straight
					|| rankProduct == 15015     // 7-high straight
					|| rankProduct == 85085     // 8-high straight
					|| rankProduct == 323323    // 9-high straight
					|| rankProduct == 1062347   // T-high straight
					|| rankProduct == 2800733   // J-high straight
					|| rankProduct == 6678671   // Q-high straight
					|| rankProduct == 14535931  // K-high straight
					|| rankProduct == 31367009; // A-high straight

				bool flush =
					suitProduct == 147008443        // Spades
					|| suitProduct == 229345007     // Hearts
					|| suitProduct == 418195493     // Diamonds
					|| suitProduct == 714924299;    // Clubs

				var cardCounts = cards.GroupBy(card => (int)card.Rank).Select(group => group).ToList();

				var fourOfAKind = -1;
				var threeOfAKind = -1;
				var onePair = -1;
				var twoPair = -1;

				foreach (var group in cardCounts)
				{
					var rank = group.Key;
					var count = group.Count();
					if (count == 4) fourOfAKind = rank;
					else if (count == 3) threeOfAKind = rank;
					else if (count == 2)
					{
						twoPair = onePair;
						onePair = rank;
					}
				}

				if (straight && flush)
				{
					strength.HandRanking = HandRanking.StraightFlush;
					strength.Kickers = cards.Select(card => (int)card.Rank).Reverse().ToList();
				}
				else if (fourOfAKind >= 0)
				{
					strength.HandRanking = HandRanking.FourOfAKind;
					strength.Kickers.Add(fourOfAKind);
					strength.Kickers.AddRange(cards
						.Where(card => (int)card.Rank != fourOfAKind)
						.Select(card => (int)card.Rank));
				}
				else if (threeOfAKind >= 0 && onePair >= 0)
				{
					strength.HandRanking = HandRanking.FullHouse;
					strength.Kickers.Add(threeOfAKind);
					strength.Kickers.Add(onePair);
				}
				else if (flush)
				{
					strength.HandRanking = HandRanking.Flush;
					strength.Kickers.AddRange(cards
						.Select(card => (int)card.Rank)
						.Reverse());
				}
				else if (straight)
				{
					strength.HandRanking = HandRanking.Straight;
					strength.Kickers.AddRange(cards
						.Select(card => (int)card.Rank)
						.Reverse());
				}
				else if (threeOfAKind >= 0)
				{
					strength.HandRanking = HandRanking.ThreeOfAKind;
					strength.Kickers.Add(threeOfAKind);
					strength.Kickers.AddRange(cards
						.Where(card => (int)card.Rank != threeOfAKind)
						.Select(card => (int)card.Rank));
				}
				else if (twoPair >= 0)
				{
					strength.HandRanking = HandRanking.TwoPair;
					strength.Kickers.Add(Math.Max(twoPair, onePair));
					strength.Kickers.Add(Math.Min(twoPair, onePair));
					strength.Kickers.AddRange(cards
						.Where(card => (int)card.Rank != twoPair && (int)card.Rank != onePair)
						.Select(card => (int)card.Rank));
				}
				else if (onePair >= 0)
				{
					strength.HandRanking = HandRanking.Pair;
					strength.Kickers.Add(onePair);
					strength.Kickers.AddRange(cards
						.Where(card => (int)card.Rank != onePair)
						.Select(card => (int)card.Rank));
				}
				else
				{
					strength.HandRanking = HandRanking.HighCard;
					strength.Kickers.AddRange(cards
						.Select(card => (int)card.Rank)
						.Reverse());
				}

				return strength;
			}

			else
			{
				return null;
			}
		}
	}
}
