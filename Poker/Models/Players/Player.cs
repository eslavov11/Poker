namespace Poker.Models.Players
{
    using Interfaces;

    public abstract class Player : IPlayer
    {
        protected Player(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }
}
