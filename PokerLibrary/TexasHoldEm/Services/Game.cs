using Newtonsoft.Json;
using PokerLibrary.TexasHoldEm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PokerLibrary.TexasHoldEm.Services
{
    public class Game
    {
        private IEnumerable<int> _availableSeats;
        private List<Player> Players { get; set; } = new List<Player>();
        private Deck Deck { get; set; } = new Deck();
        private List<Card> CardsOnTable { get; set; }
        private Random rng = new Random();

        private int _positionInDeck = 0;
        private int _maxNumberOfPlayers;
        private bool _randomBlindStart;
        private int _blindPos = 0;
        private bool _allowRebuy { get; set; }

        public int SmallBlind { get; set; } = 200;
        public int TotalBets { get; private set; } = 0;


        public Game(int maxNumberOfPlayers=6, bool allowRebuy = true, bool randomBlindStart = false)
        {
            _availableSeats = Enumerable.Range(1, maxNumberOfPlayers);
            _maxNumberOfPlayers = maxNumberOfPlayers;
            _allowRebuy = allowRebuy;
            _randomBlindStart = randomBlindStart;
        }

        public bool AddPlayer(string name, int buyInStack, int seatNumber)
        {
            if (Players.Count == _maxNumberOfPlayers || Players.Any(p=>p.Name == name))
                return false;
            Players.Add(
                new Player
                {
                    Seat = seatNumber,
                    Name = name,
                    Stack = buyInStack,
                    Hand = new Hand(),
                });
            _availableSeats = _availableSeats.Where(seat => seat != seatNumber);

            return true;
        }

        public bool AddMoney(string name, int buyin)
        {
            if (!_allowRebuy)
                return false;
            var player = Players.FirstOrDefault(p => p.Name == name);
            if (player == null)
                return false;
            player.Stack += buyin;
            return true;
        }

        public void StartDealing()
        {
            Players = Players.OrderBy(p => p.Seat).ToList();
            if (_randomBlindStart)
                _blindPos = rng.Next(0, Players.Count);
            Players[_blindPos].CurrentBet = SmallBlind;
            Players[_blindPos + 1].CurrentBet = 2 * SmallBlind;

            //Deal cards
            for(int dealtCards = 0; dealtCards < Players.Count * 2; dealtCards++)
            {
                int dealTo = (dealtCards + _blindPos) % Players.Count;
                Players[dealTo].Hand.Cards.Add(Deck.Cards[_positionInDeck++]);
            }
        }

        public void BlindBetsDone()
        {
            TotalBets = Players.Sum(p => p.CurrentBet);
            Players.ResetBets();
        }

        public string GetGameInfo()
        {
            return JsonConvert.SerializeObject(Players);
        }

        public int GetSmallBlindSeat => Players[_blindPos].Seat;
        public int GetBigBlindSeat => Players[(_blindPos+1)%Players.Count].Seat;
    }
}
