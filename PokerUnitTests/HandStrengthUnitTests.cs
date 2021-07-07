using PokerLibrary.TexasHoldEm.Models;
using PokerLibrary.TexasHoldEm.Services;
using System;
using System.Collections.Generic;
using Xunit;

namespace PokerUnitTests
{
    public class HandStrengthUnitTests
    {
        [Fact]
        public void CheckSingleHand()
        {
            var deck = new Deck();

            var hand = new Hand() { Cards = new List<Card>() { deck.Cards[0], deck.Cards[1] } };
            var board = new List<Card>();
            for (int i = 2; i < 7; i++)
            {
                board.Add(deck.Cards[i]);
            }

            var handStrength = HandEvaluator.EvaluateBestHand(board, hand);
            Console.WriteLine(handStrength);
        }

        [Fact]
        public void CheckGameInfo()
        {
            var game = new Game();
            game.AddPlayer("some dude", 20000, 2);
            game.AddPlayer("Hooray", 4000, 4);
            game.StartDealing();
            var info = game.GetGameInfo();

            Console.WriteLine(info);
        }
    }
}
