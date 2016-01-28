namespace Poker.Interfaces
{
    using System.Collections.Generic;
    using Models;

    /// <summary>
    /// Interface which holds various collections needed for the game's logic.
    /// </summary>
    public interface IDatabase
    {
        List<int> Chips { get; set; }

        List<string> CheckWinners { get; set; }

        List<Type> Winners { get; set; }

        List<bool?> PlayersGameStatus { get; set; }
    }
}
