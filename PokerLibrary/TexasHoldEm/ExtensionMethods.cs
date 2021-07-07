using PokerLibrary.TexasHoldEm.Dtos;
using PokerLibrary.TexasHoldEm.Models;
using PokerLibrary.TexasHoldEm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerLibrary.TexasHoldEm
{
    public static class ExtensionMethods
    {
        private static Random rng = new Random();

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static IEnumerable<IEnumerable<T>> GetPermutations<T>(this IEnumerable<T> list, int length)
        {
            if (length == 1) return list.Select(t => new T[] { t });

            return GetPermutations(list, length - 1)
                .SelectMany(t => list.Where(e => !t.Contains(e)),
                    (t1, t2) => t1.Concat(new T[] { t2 }));
        }

        public static void ResetBets<T>(this IEnumerable<T> players) where T : Player
        {
            foreach (Player player in players)
            {
                player.Stack -= player.CurrentBet;
                player.CurrentBet = 0;
            }
        }

        public static PlayerDto ToDto(this Player player)
        {
            return new PlayerDto
            {
                CurrentBet = player.CurrentBet,
                Name = player.Name,
                Seat = player.Seat,
                Stack = player.Stack
            };
        }

        public static IEnumerable<PlayerDto> ToDtos(this IEnumerable<Player> players)
        {
            return players.Select(p => p.ToDto());
        }

        public static BoardDto ToDto(this Game game)
        {
            return new BoardDto
            {
                BetStack = game.TotalBets,
                SmallBlindSeat = game.GetSmallBlindSeat,
                BigBlindSeat = game.GetBigBlindSeat,
                SmallBlindSize = game.SmallBlind,
            }
        }
    }
}
