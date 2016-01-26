using System.Windows.Forms;
namespace Poker.Interfaces
{
    public interface IBot : IPlayer
    {
        int StartCard { get; set; }

        int VerticalLocationCoordinate { get; set; }

        int HorizontalLocationCoordinate { get; set; }

        AnchorStyles VerticalLocation { get; set; }

        AnchorStyles HorizontalLocation { get; set; }

        AnchorStyles GetAnchorStyles();

        Label Status { get; set; }

    }
}
