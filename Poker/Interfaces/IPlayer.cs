namespace Poker.Interfaces
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    public interface IPlayer
    {
        string Name { get; }

        IList<ICard> Cards { get; }

        Panel Panel { get; }

        double Type { get; set; }

        double Power { get; set; }

        int Chips { get; set; }

        int Call { get; set; }

        int Raise { get; set; }

        bool CanMakeTurn { get; set; }

        bool OutOfChips { get; set; }

        bool Folded { get; set; }

        void InitializePanel(Point location);
    }
}
