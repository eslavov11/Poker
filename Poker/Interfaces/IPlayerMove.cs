namespace Poker.Interfaces
{
    using System.Windows.Forms;

    public interface IPlayerMove
    {
        void Fold(IPlayer pokerPlayer, Label playerStatus, ref bool rising);

        void Check(IPlayer pokerPlayer, Label playerStatus, ref bool raising);

        void Call(IPlayer pokerPlayer, Label playerStatus, ref bool raising, ref int neededChipsToCall, TextBox potStatus);

        void Raised(IPlayer pokerPlayer, Label sStatus, ref bool raising, ref int raise, ref int neededChipsToCall, TextBox potStatus);

        void HP(IPlayer pokerPlayer, Label sStatus, int n, int n1, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising);

        void PH(IPlayer player, Label sStatus, int n, int n1, int r, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, int rounds);
    }
}
