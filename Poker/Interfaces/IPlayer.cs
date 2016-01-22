namespace Poker.Interfaces
{
    using System.Collections.Generic;

    public interface IPlayer
    {
        string Name { get; }

        int Chips { get; }

        IList<ICard> Cards { get; }
    }
}
