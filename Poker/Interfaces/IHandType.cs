namespace Poker.Interfaces
{
    using System.Windows.Forms;

    /// <summary>
    /// Interface containing methods for processing bots artificial intelligence.
    /// </summary>
    public interface IHandType
    {
        /// <summary>
        /// Method defining the bot's next move if his hand type is high card.
        /// </summary>
        void HighCard(IPlayer player, Label playerStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising);

        /// <summary>
        /// Method defining the bot's next move if his hand type is pair table.
        /// </summary>
        void PairTable(IPlayer player, Label playerStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising);

        /// <summary>
        /// Method defining the bot's next move if his hand type is a pair.
        /// </summary>
        void PairHand(IPlayer player, Label playerStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// Method defining the bot's next move if his hand type is a pair of two.
        /// </summary>
        void TwoPair(IPlayer player, Label playerStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// Method defining the bot's next move if his hand type is three of a kind.
        /// </summary>
        void ThreeOfAKind(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// Method defining the bot's next move if his hand type is straight.
        /// </summary>
        void Straight(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// Method defining the bot's next move if his hand type is flush.
        /// </summary>
        void Flush(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// Method defining the bot's next move if his hand type is Full House.
        /// </summary>
        void FullHouse(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// Method defining the bot's next move if his hand type is Four Of A Kind.
        /// </summary>
        void FourOfAKind(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        /// <summary>
        /// Method defining the bot's next move if his hand type is Straight Flush.
        /// </summary>
        void StraightFlush(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);
    }
}
