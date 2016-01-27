namespace Poker.Interfaces
{
    using System.Windows.Forms;

    public interface IHandType
    {
        void HighCard(IPlayer player, Label playerStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising);

        void PairTable(IPlayer player, Label playerStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising);

        void PairHand(IPlayer player, Label playerStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void TwoPair(IPlayer player, Label playerStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void ThreeOfAKind(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void Straight(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void Flush(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void FullHouse(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void FourOfAKind(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);

        void StraightFlush(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds);
    }
}
