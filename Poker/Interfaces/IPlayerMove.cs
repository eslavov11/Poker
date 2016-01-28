namespace Poker.Interfaces
{
    using System.Windows.Forms;

    /// <summary>
    /// Interface containing methods for every move the poker player can make.
    /// </summary>
    public interface IPlayerMove
    {
        /// <summary>
        /// Sets the poker player's condition to fold.
        /// </summary>
        /// <param name="pokerPlayer">The poker player.</param>
        /// <param name="playerStatus">The player status.</param>
        void Fold(IPlayer pokerPlayer, Label playerStatus, ref bool rising);

        /// <summary>
        /// The player checks the current state.
        /// </summary>
        /// <param name="pokerPlayer">The poker player.</param>
        /// <param name="playerStatus">The player status.</param>
        void Check(IPlayer pokerPlayer, Label playerStatus, ref bool raising);

        /// <summary>
        /// The player calls the raise, his chips are placed on the table.
        /// </summary>
        /// <param name="pokerPlayer">The poker player.</param>
        /// <param name="playerStatus">The player status.</param>
        void Call(IPlayer pokerPlayer, Label playerStatus, ref bool raising, ref int neededChipsToCall, TextBox potStatus);

        /// <summary>
        /// The player raises the bet, his chips are placed on the table.
        /// </summary>
        /// <param name="pokerPlayer">The poker player.</param>
        /// <param name="playerStatus">The player status.</param>
        void Raised(IPlayer pokerPlayer, Label playerStatus, ref bool risingIsActivated, ref int raise, ref int neededChipsToCall, TextBox potStatus);

        void HP(IPlayer pokerPlayer, Label playerStatus, int n, int n1, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising);

        void PH(IPlayer player, Label playerStatus, int n, int n1, int r, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, int rounds);
    }
}
