namespace Poker.Models.Players
{
    using System.Collections.Generic;
    using Interfaces;

    public abstract class Player : IPlayer
    {
        private const int DefaultStartChips = 10000;

        private int chips;

        protected Player(string name)
        {
            this.Name = name;
            this.Chips = DefaultStartChips;

            //Initialize player with two cards, the default number of cards for texas poker
            //Expandable for poker with 5 cards
            this.Cards = new List<ICard>
            {
                new Card(),
                new Card()
            };
        }

        public string Name { get; set; }

        public int Chips
        {
            get
            {
                return this.chips;
            }

            set
            {
                if (value < 0)
                {
                    // TODO: throw or set to 0
                }

                this.chips = value;
            }
        }

        public IList<ICard> Cards { get; private set; }
    }
}
