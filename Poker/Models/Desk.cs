namespace Poker.Models
{
    using System;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;

    public class Desk
    {
        //Five bots and one person * 2 cards each + 5 cards on the table
        private const int DefaultCardsInAGameWithSixPlayers = 17;
        private const int DefaultCardsInDesk = 52;


        private readonly Image[] deskCardsAsImages;

        private readonly string[] cardsImageLocation;

        public Desk()
        {
            this.deskCardsAsImages = new Image[DefaultCardsInDesk];
            CardsPictures = new PictureBox[DefaultCardsInDesk];
            this.cardsImageLocation = Directory.GetFiles(
                "Assets\\Cards", "*.png", 
                SearchOption.TopDirectoryOnly);
            this.InitializeDesk();
        }

        private void InitializeDesk()
        {
            var randomCardLocation = new Random();

            //Shuffle cards location
            for (int cardLocationIndex = DefaultCardsInDesk; 
                cardLocationIndex > 0; 
                cardLocationIndex--)
            {
                //Swaps two cards from the desk, taking one random and replacing it with the 
                //card from the loop index
                int randomCardIndex = randomCardLocation.Next(cardLocationIndex);
                string oldCardLocation = this.cardsImageLocation[randomCardIndex];
                this.cardsImageLocation[randomCardIndex] = this.cardsImageLocation[cardLocationIndex - 1];
                this.cardsImageLocation[cardLocationIndex - 1] = oldCardLocation;
            }
        }
 

        public static PictureBox[] CardsPictures { get; private set; }
    }
}
