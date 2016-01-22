namespace Poker.Data
{
    using System.Collections.Generic;
    using Interfaces;

    public class PokerDatabase : IDatabase
    {
        //TODO: Check if name is proper, previous name - bools
        private List<bool?> playersGameStatus;
        private List<Type> win;
        private List<string> checkWinners;
        //TODO: Check if name is proper, previous name - chips
        private List<int> chips;

        public PokerDatabase()
        {
            this.PlayersGameStatus = new List<bool?>();
            this.Win = new List<Type>();
            this.CheckWinners = new List<string>();
            this.Chips = new List<int>();
        }

        public List<int> Chips
        {
            get
            {
                return this.chips;
            }
            set
            {
                this.chips = value;
            }
        }

        public List<string> CheckWinners
        {
            get
            {
                return this.checkWinners;
            }
            set
            {
                this.checkWinners = value;
            }
        }

        public List<Type> Win
        {
            get
            {
                return this.win;
            }
            set
            {
                this.win = value;
            }
        }

        public List<bool?> PlayersGameStatus
        {
            get
            {
                return this.playersGameStatus;
            }
            set
            {
                this.playersGameStatus = value;
            }
        }
    }
}
