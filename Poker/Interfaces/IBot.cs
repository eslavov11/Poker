namespace Poker.Interfaces
{
    using System.Windows.Forms;

    /// <summary>
    /// Interface which contains properties and methods for the bot's behavior.
    /// </summary>
    /// <seealso cref="Poker.Interfaces.IPlayer" />
    public interface IBot : IPlayer
    {
        int StartCard { get; set; }

        int VerticalLocationCoordinate { get; set; }

        int HorizontalLocationCoordinate { get; set; }

        AnchorStyles VerticalLocation { get; set; }

        AnchorStyles HorizontalLocation { get; set; }

        Label Status { get; set; }

        TextBox TextBoxBotChips { get; set; }

        /// <summary>
        /// Gets the anchor styles.
        /// </summary>
        /// <returns></returns>
        AnchorStyles GetAnchorStyles();
    }
}
