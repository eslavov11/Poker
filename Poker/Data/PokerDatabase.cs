namespace Poker.Data
{
    using System.Collections.Generic;
    using Interfaces;
    using Models;

    /// <summary>
    /// Class which holds various collections needed for the game
    /// </summary>
    /// <seealso cref="Poker.Interfaces.IDatabase" />
    public class PokerDatabase : IDatabase
    {
        private List<bool?> playersGameStatus;

        private List<Type> winners;

        private List<string> checkWinners;

        private List<int> chips;

        public PokerDatabase()
        {
            this.PlayersGameStatus = new List<bool?>();
            this.Winners = new List<Type>();
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

        public List<Type> Winners
        {
            get
            {
                return this.winners;
            }

            set
            {
                this.winners = value;
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
