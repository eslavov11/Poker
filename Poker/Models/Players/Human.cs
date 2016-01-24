namespace Poker.Models.Players
{
    using Interfaces;

    public class Human : Player, IHuman
    {
        public Human()
            : base()
        {
            this.CanMakeTurn = true;
        }
    }
}
