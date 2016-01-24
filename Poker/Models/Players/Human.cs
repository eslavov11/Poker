namespace Poker.Models.Players
{
    using Interfaces;

    public class Human : Player, IHuman
    {
        public Human(string name)
            : base(name)
        {
            this.CanMakeTurn = true;
        }
    }
}
