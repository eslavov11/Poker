namespace Poker.Models.Players
{
    using Interfaces;

    /// <summary>
    /// Class used for creating human players.
    /// </summary>
    /// <seealso cref="Poker.Models.Players.Player" />
    /// <seealso cref="Poker.Interfaces.IHuman" />
    public class Human : Player, IHuman
    {
        public Human(string name)
            : base(name)
        {
            this.CanMakeTurn = true;
        }
    }
}
