namespace Poker.Interfaces
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    // Interface containing similar logig for both human poker player and bot.
    /// </summary>
    public interface IPlayer
    {
        string Name { get; }

        Panel Panel { get; }

        double HandType { get; set; }

        double HandPower { get; set; }

        int Chips { get; set; }

        int Call { get; set; }

        int Raise { get; set; }

        bool CanMakeTurn { get; set; }

        bool OutOfChips { get; set; }

        bool Folded { get; set; }

        /// <summary>
        /// Initializes the poker player's panel.
        /// </summary>
        /// <param name="location">The player's location.</param>
        void InitializePanel(Point location);
    }
}
