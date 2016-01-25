using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Poker.UserInterface
{
    using Interfaces;
    using Models.Players;
    using Models;
    using Poker.Data;

    public partial class PokerTable : Form
    {
        #region Variables
        public const int NumberOfDeskCards = 17;

        private readonly IAssertHandType assertHandType;
        private readonly IHandType handType;
        private readonly IPlayer human;
        // TODO: put all bots in one list!
        // TODO: get all bots back to IPlayer!!!
        private readonly IBot firstBot;
        private readonly IBot secondBot;
        private readonly IBot thirdBot;
        private readonly IBot fourthBot;
        private readonly IBot fifthBot;
        private readonly IDatabase pokerDatabase;

        //private ProgressBar asd = new ProgressBar();
        //private int nm;

        private int neededChipsToCall = 500;
        private int foldedPlayers = 5;
        private double type;
        private int rounds;
        private int raise;
        private bool chipsAreAdded;
        private bool changed;
        private int height;
        private int width;
        private int winners = 0;

        private int flop = 1;
        private int turn = 2;
        private int river = 3;
        private int end = 4;

        private int maxLeft = 6;
        private int last = 123;
        private int raisedTurn = 1;
        private bool restart;
        private bool raising;
        private Type sorted;

        //TODO: previous name ImgLocation
        private string[] cardsImageLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);

        /*string[] cardsImageLocation ={
                   "Assets\\Cards\\33.png","Assets\\Cards\\22.png",
                    "Assets\\Cards\\29.png","Assets\\Cards\\21.png",
                    "Assets\\Cards\\36.png","Assets\\Cards\\17.png",
                    "Assets\\Cards\\40.png","Assets\\Cards\\16.png",
                    "Assets\\Cards\\5.png","Assets\\Cards\\47.png",
                    "Assets\\Cards\\37.png","Assets\\Cards\\13.png",
                    
                    "Assets\\Cards\\12.png",
                    "Assets\\Cards\\8.png","Assets\\Cards\\18.png",
                    "Assets\\Cards\\15.png","Assets\\Cards\\27.png"};*/

        //TODO: previous name Reserved
        private int[] reservedGameCardsIndeces = new int[17];

        private const int DefaultCardsInDesk = 52;

        //TODO: previous name Desk
        private readonly Image[] deskCardsAsImages = new Image[DefaultCardsInDesk];

        //TODO: previous name Holder
        private readonly PictureBox[] cardsPictureBoxList = new PictureBox[DefaultCardsInDesk];
        private Timer timer = new Timer();
        private Timer updates = new Timer();

        private int secondsLeft = 60;
        private int i;
        private int bigBlindValue = 500;
        private int smallBlindValue = 250;
        private int up = 10000000;
        private int turnCount = 0;
        #endregion

        public PokerTable()
        {
            this.handType = new HandType();
            this.assertHandType = new AssertHandType();
            this.pokerDatabase = new PokerDatabase();
            this.human = new Human("Player");
            this.firstBot = new Bot("Bot 1", 2, 420, 15, AnchorStyles.Bottom, AnchorStyles.Left);
            this.secondBot = new Bot("Bot 2", 4, 65, 75, AnchorStyles.Top, AnchorStyles.Left);
            this.thirdBot = new Bot("Bot 3", 6, 25, 590, AnchorStyles.Top, 0);
            this.fourthBot = new Bot("Bot 4", 8, 65, 1115, AnchorStyles.Top, AnchorStyles.Right);
            this.fifthBot = new Bot("Bot 5", 10, 420, 1160, AnchorStyles.Bottom, AnchorStyles.Right);

            //pokerDatabasePlayersGameStatus.Add(humanOutOfChips); pokerDatabasePlayersGameStatus.Add(firstBotOutOfChips); pokerDatabasePlayersGameStatus.Add(secondBotOutOfChips); pokerDatabasePlayersGameStatus.Add(thirdBotOutOfChips); pokerDatabasePlayersGameStatus.Add(fourthBotOutOfChips); pokerDatabasePlayersGameStatus.Add(fifthBotOutOfChips);
            this.neededChipsToCall = this.bigBlindValue;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.updates.Start();
            this.InitializeComponent();
            this.width = this.Width;
            this.height = this.Height;
            this.Shuffle();
            this.potStatus.Enabled = false;
            this.txtBoxHumanChips.Enabled = false;
            this.txtBoxFirstBotChips.Enabled = false;
            this.txtBoxSecondBotChips.Enabled = false;
            this.txtBoxThirdBotChips.Enabled = false;
            this.txtBoxFourthBotChips.Enabled = false;
            this.txtBoxFifthBotChips.Enabled = false;
            this.txtBoxHumanChips.Text = "Chips : " + this.human.Chips;
            this.txtBoxFirstBotChips.Text = "Chips : " + this.firstBot.Chips;
            this.txtBoxSecondBotChips.Text = "Chips : " + this.secondBot.Chips;
            this.txtBoxThirdBotChips.Text = "Chips : " + this.thirdBot.Chips;
            this.txtBoxFourthBotChips.Text = "Chips : " + this.fourthBot.Chips;
            this.txtBoxFifthBotChips.Text = "Chips : " + this.fifthBot.Chips;
            this.timer.Interval = (1 * 1 * 1000);
            this.timer.Tick += this.TimerTick;
            this.updates.Interval = (1 * 1 * 100);
            this.updates.Tick += this.UpdateTick;
            this.tbBigBlind.Visible = true;
            this.tbSmallBlind.Visible = true;
            this.buttonBigBlind.Visible = true;
            this.buttonSmallBlind.Visible = true;
            this.tbBigBlind.Visible = true;
            this.tbSmallBlind.Visible = true;
            this.buttonBigBlind.Visible = true;
            this.buttonSmallBlind.Visible = true;
            this.tbBigBlind.Visible = false;
            this.tbSmallBlind.Visible = false;
            this.buttonBigBlind.Visible = false;
            this.buttonSmallBlind.Visible = false;
            this.tbRaise.Text = (this.bigBlindValue * 2).ToString();
        }

        private async Task Shuffle()
        {
            this.pokerDatabase.PlayersGameStatus.Add(this.human.OutOfChips);
            this.pokerDatabase.PlayersGameStatus.Add(this.firstBot.OutOfChips);
            this.pokerDatabase.PlayersGameStatus.Add(this.secondBot.OutOfChips);
            this.pokerDatabase.PlayersGameStatus.Add(this.thirdBot.OutOfChips);
            this.pokerDatabase.PlayersGameStatus.Add(this.fourthBot.OutOfChips);
            this.pokerDatabase.PlayersGameStatus.Add(this.fifthBot.OutOfChips);
            this.buttonCall.Enabled = false;
            this.buttonRaise.Enabled = false;
            this.buttonFold.Enabled = false;
            this.buttonCheck.Enabled = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            bool check = false;
            Bitmap backImage = new Bitmap("..\\..\\Resources\\Assets\\Back\\Back.png");
            int horizontal = 580;
            int vertical = 480;

            var randomCardLocation = new Random();

            //Shuffle cards location
            for (int cardLocationIndex = DefaultCardsInDesk; cardLocationIndex > 0; cardLocationIndex--)
            {
                //Swaps two cards locations from the desk, taking one random and replacing it with the 
                //card location from the loop index
                int randomCardIndex = randomCardLocation.Next(cardLocationIndex);
                string oldCardLocation = this.cardsImageLocation[randomCardIndex];
                this.cardsImageLocation[randomCardIndex] = this.cardsImageLocation[cardLocationIndex - 1];
                this.cardsImageLocation[cardLocationIndex - 1] = oldCardLocation;
            }

            
            for (int cardNumber = 0; cardNumber < NumberOfDeskCards; cardNumber++)
            {
                this.deskCardsAsImages[cardNumber] = Image.FromFile(this.cardsImageLocation[cardNumber]);
                var partsToRemove = new[] { "..\\..\\Resources\\Assets\\Cards\\", ".png" };
                foreach (string part in partsToRemove)
                {
                    this.cardsImageLocation[cardNumber] = this.cardsImageLocation[cardNumber]
                        .Replace(part, string.Empty);
                }

                this.reservedGameCardsIndeces[cardNumber] = int.Parse(this.cardsImageLocation[cardNumber]) - 1;
                this.cardsPictureBoxList[cardNumber] = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Height = 130,
                    Width = 80
                };
                this.Controls.Add(this.cardsPictureBoxList[cardNumber]);
                this.cardsPictureBoxList[cardNumber].Name = "pb" + cardNumber.ToString();
                await Task.Delay(200);

                //TODO: Should this region be in the for loop at all?
                #region Throwing Cards
                if (cardNumber < 2)
                {
                    if (this.cardsPictureBoxList[0].Tag != null)
                    {
                        this.cardsPictureBoxList[1].Tag = this.reservedGameCardsIndeces[1];
                    }

                    this.cardsPictureBoxList[0].Tag = this.reservedGameCardsIndeces[0];
                    this.cardsPictureBoxList[cardNumber].Image = this.deskCardsAsImages[cardNumber];
                    this.cardsPictureBoxList[cardNumber].Anchor = (AnchorStyles.Bottom);
                    //cardsPictureBoxList[i].Dock = DockStyle.Top;
                    this.cardsPictureBoxList[cardNumber].Location = new Point(horizontal, vertical);
                    horizontal += this.cardsPictureBoxList[cardNumber].Width;
                    this.Controls.Add(this.human.Panel);
                    this.human.InitializePanel(new Point(
                        this.cardsPictureBoxList[0].Left - 10,
                        this.cardsPictureBoxList[0].Top - 10));
                }
                
                // partly solved the problem with the code repetition
                DealCardsForBots(this.firstBot, cardNumber, backImage, ref check, ref horizontal, ref vertical);
                DealCardsForBots(this.secondBot, cardNumber, backImage, ref check, ref horizontal, ref vertical);
                DealCardsForBots(this.thirdBot, cardNumber, backImage, ref check, ref horizontal, ref vertical);
                DealCardsForBots(this.fourthBot, cardNumber, backImage, ref check, ref horizontal, ref vertical);
                DealCardsForBots(this.fifthBot, cardNumber, backImage, ref check, ref horizontal, ref vertical);
                //if (this.firstBot.Chips > 0)
                //{
                //    this.foldedPlayers--;
                //    if (cardNumber >= 2 && cardNumber < 4)
                //    {
                //        if (this.cardsPictureBoxList[2].Tag != null)
                //        {
                //            this.cardsPictureBoxList[3].Tag = this.reservedGameCardsIndeces[3];
                //        }

                //        this.cardsPictureBoxList[2].Tag = this.reservedGameCardsIndeces[2];
                //        if (!check)
                //        {
                //            horizontal = 15;
                //            vertical = 420;
                //        }

                //        check = true;
                //        this.cardsPictureBoxList[cardNumber].Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
                //        this.cardsPictureBoxList[cardNumber].Image = backImage;
                //        //cardsPictureBoxList[i].Image = deskCardsAsImages[i];
                //        this.cardsPictureBoxList[cardNumber].Location = new Point(horizontal, vertical);
                //        horizontal += this.cardsPictureBoxList[cardNumber].Width;
                //        this.cardsPictureBoxList[cardNumber].Visible = true;
                //        this.Controls.Add(this.firstBot.Panel);
                //        this.firstBot.InitializePanel(new Point(
                //            this.cardsPictureBoxList[2].Left - 10,
                //            this.cardsPictureBoxList[2].Top - 10));

                //        if (cardNumber == 3)
                //        {
                //            check = false;
                //        }
                //    }
                //}

                //if (this.secondBot.Chips > 0)
                //{
                //    foldedPlayers--;
                //    if (cardNumber >= 4 && cardNumber < 6)
                //    {
                //        if (this.cardsPictureBoxList[4].Tag != null)
                //        {
                //            this.cardsPictureBoxList[5].Tag = this.reservedGameCardsIndeces[5];
                //        }

                //        this.cardsPictureBoxList[4].Tag = this.reservedGameCardsIndeces[4];
                //        if (!check)
                //        {
                //            horizontal = 75;
                //            vertical = 65;
                //        }

                //        check = true;
                //        this.cardsPictureBoxList[cardNumber].Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                //        this.cardsPictureBoxList[cardNumber].Image = backImage;
                //        //cardsPictureBoxList[i].Image = deskCardsAsImages[i];
                //        this.cardsPictureBoxList[cardNumber].Location = new Point(horizontal, vertical);
                //        horizontal += this.cardsPictureBoxList[cardNumber].Width;
                //        this.cardsPictureBoxList[cardNumber].Visible = true;
                //        this.Controls.Add(this.secondBot.Panel);
                //        this.secondBot.InitializePanel(new Point(
                //            this.cardsPictureBoxList[4].Left - 10,
                //            this.cardsPictureBoxList[4].Top - 10));

                //        if (cardNumber == 5)
                //        {
                //            check = false;
                //        }
                //    }
                //}

                //if (this.thirdBot.Chips > 0)
                //{
                //    foldedPlayers--;
                //    if (cardNumber >= 6 && cardNumber < 8)
                //    {
                //        if (this.cardsPictureBoxList[6].Tag != null)
                //        {
                //            this.cardsPictureBoxList[7].Tag = this.reservedGameCardsIndeces[7];
                //        }

                //        this.cardsPictureBoxList[6].Tag = this.reservedGameCardsIndeces[6];
                //        if (!check)
                //        {
                //            horizontal = 590;
                //            vertical = 25;
                //        }
                //        check = true;
                //        this.cardsPictureBoxList[cardNumber].Anchor = (AnchorStyles.Top);
                //        this.cardsPictureBoxList[cardNumber].Image = backImage;
                //        //cardsPictureBoxList[i].Image = deskCardsAsImages[i];
                //        this.cardsPictureBoxList[cardNumber].Location = new Point(horizontal, vertical);
                //        horizontal += this.cardsPictureBoxList[cardNumber].Width;
                //        this.cardsPictureBoxList[cardNumber].Visible = true;
                //        this.Controls.Add(this.thirdBot.Panel);
                //        this.thirdBot.InitializePanel(new Point(
                //            this.cardsPictureBoxList[6].Left - 10,
                //            this.cardsPictureBoxList[6].Top - 10));

                //        if (cardNumber == 7)
                //        {
                //            check = false;
                //        }
                //    }
                //}

                //if (this.fourthBot.Chips > 0)
                //{
                //    foldedPlayers--;
                //    if (cardNumber >= 8 && cardNumber < 10)
                //    {
                //        if (this.cardsPictureBoxList[8].Tag != null)
                //        {
                //            this.cardsPictureBoxList[9].Tag = this.reservedGameCardsIndeces[9];
                //        }

                //        this.cardsPictureBoxList[8].Tag = this.reservedGameCardsIndeces[8];
                //        if (!check)
                //        {
                //            horizontal = 1115;
                //            vertical = 65;
                //        }

                //        check = true;
                //        this.cardsPictureBoxList[cardNumber].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                //        this.cardsPictureBoxList[cardNumber].Image = backImage;
                //        //cardsPictureBoxList[i].Image = deskCardsAsImages[i];
                //        this.cardsPictureBoxList[cardNumber].Location = new Point(horizontal, vertical);
                //        horizontal += this.cardsPictureBoxList[cardNumber].Width;
                //        this.cardsPictureBoxList[cardNumber].Visible = true;
                //        this.Controls.Add(this.fourthBot.Panel);
                //        this.fourthBot.InitializePanel(new Point(
                //            this.cardsPictureBoxList[8].Left - 10,
                //            this.cardsPictureBoxList[8].Top - 10));

                //        if (cardNumber == 9)
                //        {
                //            check = false;
                //        }
                //    }
                //}

                //if (this.fifthBot.Chips > 0)
                //{
                //    foldedPlayers--;
                //    if (cardNumber >= 10 && cardNumber < 12)
                //    {
                //        if (this.cardsPictureBoxList[10].Tag != null)
                //        {
                //            this.cardsPictureBoxList[11].Tag = this.reservedGameCardsIndeces[11];
                //        }

                //        this.cardsPictureBoxList[10].Tag = this.reservedGameCardsIndeces[10];
                //        if (!check)
                //        {
                //            horizontal = 1160;
                //            vertical = 420;
                //        }
                //        check = true;
                //        this.cardsPictureBoxList[cardNumber].Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
                //        this.cardsPictureBoxList[cardNumber].Image = backImage;
                //        //cardsPictureBoxList[i].Image = deskCardsAsImages[i];
                //        this.cardsPictureBoxList[cardNumber].Location = new Point(horizontal, vertical);
                //        horizontal += this.cardsPictureBoxList[cardNumber].Width;
                //        this.cardsPictureBoxList[cardNumber].Visible = true;
                //        this.Controls.Add(this.fifthBot.Panel);
                //        this.fifthBot.InitializePanel(
                //            new Point(
                //                this.cardsPictureBoxList[10].Left - 10,
                //                this.cardsPictureBoxList[10].Top - 10));

                //        if (cardNumber == 11)
                //        {
                //            check = false;
                //        }
                //    }
                //}

                if (cardNumber >= 12)
                {
                    this.cardsPictureBoxList[12].Tag = this.reservedGameCardsIndeces[12];
                    if (cardNumber > 12)
                    {
                        this.cardsPictureBoxList[13].Tag = this.reservedGameCardsIndeces[13];
                    }

                    if (cardNumber > 13)
                    {
                        this.cardsPictureBoxList[14].Tag = this.reservedGameCardsIndeces[14];
                    }

                    if (cardNumber > 14)
                    {
                        this.cardsPictureBoxList[15].Tag = this.reservedGameCardsIndeces[15];
                    }

                    if (cardNumber > 15)
                    {
                        this.cardsPictureBoxList[16].Tag = this.reservedGameCardsIndeces[16];

                    }

                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }

                    check = true;
                    if (this.cardsPictureBoxList[cardNumber] != null)
                    {
                        this.cardsPictureBoxList[cardNumber].Anchor = AnchorStyles.None;
                        this.cardsPictureBoxList[cardNumber].Image = backImage;
                        //cardsPictureBoxList[i].Image = deskCardsAsImages[i];
                        this.cardsPictureBoxList[cardNumber].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }

                #endregion

                //TODO: extract methods logic below is completely identical
                #region CheckForDefeatedBots
                CheckForDefeatedBots(this.firstBot, cardNumber);
                CheckForDefeatedBots(this.secondBot, cardNumber);
                CheckForDefeatedBots(this.thirdBot, cardNumber);
                CheckForDefeatedBots(this.fourthBot, cardNumber);
                CheckForDefeatedBots(this.fifthBot, cardNumber);
                //if (this.firstBot.Chips <= 0)
                //{
                //    this.firstBot.OutOfChips = true;
                //    this.cardsPictureBoxList[2].Visible = false;
                //    this.cardsPictureBoxList[3].Visible = false;
                //}
                //else
                //{
                //    this.firstBot.OutOfChips = false;
                //    if (cardNumber == 3)
                //    {
                //        if (this.cardsPictureBoxList[3] != null)
                //        {
                //            //TODO: Is this working properly?
                //            this.cardsPictureBoxList[2].Visible = true;
                //            this.cardsPictureBoxList[3].Visible = true;
                //        }
                //    }
                //}

                //if (this.secondBot.Chips <= 0)
                //{
                //    this.secondBot.OutOfChips = true;
                //    this.cardsPictureBoxList[4].Visible = false;
                //    this.cardsPictureBoxList[5].Visible = false;
                //}
                //else
                //{
                //    this.secondBot.OutOfChips = false;
                //    if (cardNumber == 5)
                //    {
                //        if (this.cardsPictureBoxList[5] != null)
                //        {
                //            this.cardsPictureBoxList[4].Visible = true;
                //            this.cardsPictureBoxList[5].Visible = true;
                //        }
                //    }
                //}

                //if (this.thirdBot.Chips <= 0)
                //{
                //    this.thirdBot.OutOfChips = true;
                //    this.cardsPictureBoxList[6].Visible = false;
                //    this.cardsPictureBoxList[7].Visible = false;
                //}
                //else
                //{
                //    this.thirdBot.OutOfChips = false;
                //    if (cardNumber == 7)
                //    {
                //        if (this.cardsPictureBoxList[7] != null)
                //        {
                //            this.cardsPictureBoxList[6].Visible = true;
                //            this.cardsPictureBoxList[7].Visible = true;
                //        }
                //    }
                //}

                //if (this.fourthBot.Chips <= 0)
                //{
                //    this.fourthBot.OutOfChips = true;
                //    this.cardsPictureBoxList[8].Visible = false;
                //    this.cardsPictureBoxList[9].Visible = false;
                //}
                //else
                //{
                //    this.fourthBot.OutOfChips = false;
                //    if (cardNumber == 9)
                //    {
                //        if (this.cardsPictureBoxList[9] != null)
                //        {
                //            this.cardsPictureBoxList[8].Visible = true;
                //            this.cardsPictureBoxList[9].Visible = true;
                //        }
                //    }
                //}

                //if (this.fifthBot.Chips <= 0)
                //{
                //    this.fifthBot.OutOfChips = true;
                //    this.cardsPictureBoxList[10].Visible = false;
                //    this.cardsPictureBoxList[11].Visible = false;
                //}
                //else
                //{
                //    this.fifthBot.OutOfChips = false;
                //    if (cardNumber == 11)
                //    {
                //        if (this.cardsPictureBoxList[11] != null)
                //        {
                //            this.cardsPictureBoxList[10].Visible = true;
                //            this.cardsPictureBoxList[11].Visible = true;
                //        }
                //    }
                //}

                if (cardNumber == 16)
                {
                    if (!restart)
                    {
                        MaximizeBox = true;
                        MinimizeBox = true;
                    }
                    timer.Start();
                }
                #endregion
            }

            //TODO: GameOver state
            #region GameOverState?
            if (foldedPlayers == 5)
            {
                DialogResult dialogResult =
                    MessageBox.Show(
                        "Would You Like To Play Again ?",
                        "You Won , Congratulations ! ",
                        MessageBoxButtons.YesNo);

                if (dialogResult == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else if (dialogResult == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            else
            {
                foldedPlayers = 5;
            }

            if (i == 17)
            {
                this.buttonRaise.Enabled = true;
                this.buttonCall.Enabled = true;
                this.buttonRaise.Enabled = true;
                this.buttonRaise.Enabled = true;
                this.buttonFold.Enabled = true;
            }
            #endregion
        }

        // TODO: put the appropriate methods/properties in the IBot interface and change from Bot to IBot
        private void DealCardsForBots(IBot bot, int cardNumber, Bitmap backImage, 
            ref bool check, ref int horizontal, ref int vertical)
        {
            if (bot.Chips > 0)
            {
                this.foldedPlayers--;
                if (cardNumber >= bot.StartCard && cardNumber < bot.StartCard + 2)
                {
                    if (this.cardsPictureBoxList[bot.StartCard].Tag != null)
                    {
                        this.cardsPictureBoxList[bot.StartCard + 1].Tag = this.reservedGameCardsIndeces[bot.StartCard + 1];
                    }

                    this.cardsPictureBoxList[bot.StartCard].Tag = this.reservedGameCardsIndeces[bot.StartCard];
                    if (!check)
                    {
                        horizontal = bot.HorizontalLocationCoordinate;
                        vertical = bot.VerticalLocationCoordinate;
                    }

                    check = true;
                    this.cardsPictureBoxList[cardNumber].Anchor = bot.GetAnchorStyles();
                    this.cardsPictureBoxList[cardNumber].Image = backImage;
                    //cardsPictureBoxList[i].Image = deskCardsAsImages[i];
                    this.cardsPictureBoxList[cardNumber].Location = new Point(horizontal, vertical);
                    horizontal += this.cardsPictureBoxList[cardNumber].Width;
                    this.cardsPictureBoxList[cardNumber].Visible = true;
                    this.Controls.Add(this.firstBot.Panel);
                    this.firstBot.InitializePanel(new Point(
                        this.cardsPictureBoxList[bot.StartCard].Left - 10,
                        this.cardsPictureBoxList[bot.StartCard].Top - 10));

                    if (cardNumber == bot.StartCard + 1)
                    {
                        check = false;
                    }
                }
            }
        }

        private void CheckForDefeatedBots(IBot bot, int cardNumber)
        {
            if (bot.Chips <= 0)
            {
                bot.OutOfChips = true;
                this.cardsPictureBoxList[bot.StartCard].Visible = false;
                this.cardsPictureBoxList[bot.StartCard + 1].Visible = false;
            }
            else
            {
                bot.OutOfChips = false;
                if (cardNumber == bot.StartCard + 1)
                {
                    if (this.cardsPictureBoxList[bot.StartCard + 1] != null)
                    {
                        //TODO: Is this working properly?
                        this.cardsPictureBoxList[bot.StartCard].Visible = true;
                        this.cardsPictureBoxList[bot.StartCard + 1].Visible = true;
                    }
                }
            }
        }

        private async Task Turns()
        {
            #region Rotating
            if (!this.human.OutOfChips)
            {
                if (this.human.CanMakeTurn)
                {
                    this.FixCall(this.humanStatus, this.human.Call, this.human.Raise, 1);
                    //MessageBox.Show("Player's turn");
                    this.pbTimer.Visible = true;
                    this.pbTimer.Value = 1000;
                    this.secondsLeft = 60;
                    this.up = 10000000;
                    this.timer.Start();
                    this.buttonRaise.Enabled = true;
                    this.buttonCall.Enabled = true;
                    this.buttonRaise.Enabled = true;
                    this.buttonRaise.Enabled = true;
                    this.buttonFold.Enabled = true;
                    this.turnCount++;
                    this.FixCall(this.humanStatus, this.human.Call, this.human.Raise, 2);
                }
            }

            if (this.human.OutOfChips || !this.human.CanMakeTurn)
            {
                await AllIn();
                if (this.human.OutOfChips && !this.human.Folded)
                {
                    if (this.buttonCall.Text.Contains("All in") == false || this.buttonRaise.Text.Contains("All in") == false)
                    {
                        this.pokerDatabase.PlayersGameStatus.RemoveAt(0);
                        this.pokerDatabase.PlayersGameStatus.Insert(0, null);
                        this.maxLeft--;
                        this.human.Folded = true;
                    }
                }

                await CheckRaise(0, 0);
                pbTimer.Visible = false;
                this.buttonRaise.Enabled = false;
                this.buttonCall.Enabled = false;
                this.buttonRaise.Enabled = false;
                this.buttonRaise.Enabled = false;
                this.buttonFold.Enabled = false;
                timer.Stop();
                this.firstBot.CanMakeTurn = true;
                if (!this.firstBot.OutOfChips)
                {
                    if (this.firstBot.CanMakeTurn)
                    {
                        FixCall(this.firstBotStatus, this.firstBot.Call, this.firstBot.Raise, 1);
                        FixCall(this.firstBotStatus, this.firstBot.Call, this.firstBot.Raise, 2);
                        this.Rules(2, 3, "Bot 1", this.firstBot);
                        MessageBox.Show("Bot 1's turn");
                        this.AI(2, 3, this.firstBotStatus, 0, this.firstBot);
                        this.turnCount++;
                        last = 1;
                        this.firstBot.CanMakeTurn = false;
                        this.secondBot.CanMakeTurn = true;
                    }
                }

                if (this.firstBot.OutOfChips && !this.firstBot.Folded)
                {
                    this.pokerDatabase.PlayersGameStatus.RemoveAt(1);
                    this.pokerDatabase.PlayersGameStatus.Insert(1, null);
                    this.maxLeft--;
                    this.firstBot.Folded = true;
                }

                if (this.firstBot.OutOfChips || !this.firstBot.CanMakeTurn)
                {
                    await CheckRaise(1, 1);
                    this.secondBot.CanMakeTurn = true;
                }

                if (!this.secondBot.OutOfChips)
                {
                    if (this.secondBot.CanMakeTurn)
                    {
                        FixCall(this.secondBotStatus, this.secondBot.Call, this.secondBot.Raise, 1);
                        FixCall(this.secondBotStatus, this.secondBot.Call, this.secondBot.Raise, 2);
                        this.Rules(4, 5, "Bot 2", this.secondBot);
                        MessageBox.Show("Bot 2's turn");
                        this.AI(4, 5, this.secondBotStatus, 1, this.secondBot);
                        this.turnCount++;
                        last = 2;
                        this.secondBot.CanMakeTurn = false;
                        this.thirdBot.CanMakeTurn = true;
                    }
                }

                if (this.secondBot.OutOfChips && !this.secondBot.Folded)
                {
                    this.pokerDatabase.PlayersGameStatus.RemoveAt(2);
                    this.pokerDatabase.PlayersGameStatus.Insert(2, null);
                    this.maxLeft--;
                    this.secondBot.Folded = true;
                }

                if (this.secondBot.OutOfChips || !this.secondBot.CanMakeTurn)
                {
                    await CheckRaise(2, 2);
                    this.thirdBot.CanMakeTurn = true;
                }

                if (!this.thirdBot.OutOfChips)
                {
                    if (this.thirdBot.CanMakeTurn)
                    {
                        FixCall(this.thirdBotStatus, this.thirdBot.Call, this.thirdBot.Raise, 1);
                        FixCall(this.thirdBotStatus, this.thirdBot.Call, this.thirdBot.Raise, 2);
                        this.Rules(6, 7, "Bot 3", this.thirdBot);
                        MessageBox.Show("Bot 3's turn");
                        this.AI(6, 7, this.thirdBotStatus, 2, this.thirdBot);
                        this.turnCount++;
                        last = 3;
                        this.thirdBot.CanMakeTurn = false;
                        this.fourthBot.CanMakeTurn = true;
                    }
                }

                if (this.thirdBot.OutOfChips && !this.thirdBot.Folded)
                {
                    this.pokerDatabase.PlayersGameStatus.RemoveAt(3);
                    this.pokerDatabase.PlayersGameStatus.Insert(3, null);
                    this.maxLeft--;
                    this.thirdBot.Folded = true;
                }

                if (this.thirdBot.OutOfChips || !this.thirdBot.CanMakeTurn)
                {
                    await CheckRaise(3, 3);
                    this.fourthBot.CanMakeTurn = true;
                }

                if (!this.fourthBot.OutOfChips)
                {
                    if (this.fourthBot.CanMakeTurn)
                    {
                        FixCall(this.fourthBotStatus, this.fourthBot.Call, this.fourthBot.Raise, 1);
                        FixCall(this.fourthBotStatus, this.fourthBot.Call, this.fourthBot.Raise, 2);
                        this.Rules(8, 9, "Bot 4", this.fourthBot);
                        MessageBox.Show("Bot 4's turn");
                        this.AI(8, 9, this.fourthBotStatus, 3, this.fourthBot);
                        this.turnCount++;
                        last = 4;
                        this.fourthBot.CanMakeTurn = false;
                        this.fifthBot.CanMakeTurn = true;
                    }
                }

                if (this.fourthBot.OutOfChips && !this.fourthBot.Folded)
                {
                    this.pokerDatabase.PlayersGameStatus.RemoveAt(4);
                    this.pokerDatabase.PlayersGameStatus.Insert(4, null);
                    this.maxLeft--;
                    this.fourthBot.Folded = true;
                }

                if (this.fourthBot.OutOfChips || !this.fourthBot.CanMakeTurn)
                {
                    await CheckRaise(4, 4);
                    this.fifthBot.CanMakeTurn = true;
                }

                if (!this.fifthBot.OutOfChips)
                {
                    if (this.fifthBot.CanMakeTurn)
                    {
                        FixCall(this.fifthBotStatus, this.fifthBot.Call, this.fifthBot.Raise, 1);
                        FixCall(this.fifthBotStatus, this.fifthBot.Call, this.fifthBot.Raise, 2);
                        this.Rules(10, 11, "Bot 5", this.fifthBot);
                        MessageBox.Show("Bot 5's turn");
                        this.AI(10, 11, this.fifthBotStatus, 4, this.fifthBot);
                        this.turnCount++;
                        last = 5;
                        this.fifthBot.CanMakeTurn = false;
                    }
                }

                if (this.fifthBot.OutOfChips && !this.fifthBot.Folded)
                {
                    this.pokerDatabase.PlayersGameStatus.RemoveAt(5);
                    this.pokerDatabase.PlayersGameStatus.Insert(5, null);
                    this.maxLeft--;
                    this.fifthBot.Folded = true;
                }

                if (this.fifthBot.OutOfChips || !this.fifthBot.CanMakeTurn)
                {
                    await CheckRaise(5, 5);
                    this.human.CanMakeTurn = true;
                }

                if (this.human.OutOfChips && !this.human.Folded)
                {
                    if (this.buttonCall.Text.Contains("All in") == false || this.buttonRaise.Text.Contains("All in") == false)
                    {
                        this.pokerDatabase.PlayersGameStatus.RemoveAt(0);
                        this.pokerDatabase.PlayersGameStatus.Insert(0, null);
                        this.maxLeft--;
                        this.human.Folded = true;
                    }
                }
                #endregion

                await AllIn();
                if (!restart)
                {
                    await Turns();
                }

                this.restart = false;
            }
        }

        private void Rules(int card1, int card2, string currentText, IPlayer player)
        {
            if (card1 == 0 && card2 == 1)
            {
            }

            if (!player.OutOfChips || 
                card1 == 0 && card2 == 1 &&
                !this.humanStatus.Text.Contains("Fold"))
            {
                #region Variables
                bool done = false;
                bool vf = false;
                int[] cardsOnBoard = new int[5];
                int[] Straight = new int[7];
                Straight[0] = this.reservedGameCardsIndeces[card1];
                Straight[1] = this.reservedGameCardsIndeces[card2];
                cardsOnBoard[0] = Straight[2] = this.reservedGameCardsIndeces[12];
                cardsOnBoard[1] = Straight[3] = this.reservedGameCardsIndeces[13];
                cardsOnBoard[2] = Straight[4] = this.reservedGameCardsIndeces[14];
                cardsOnBoard[3] = Straight[5] = this.reservedGameCardsIndeces[15];
                cardsOnBoard[4] = Straight[6] = this.reservedGameCardsIndeces[16];
                int[] getClubes = Straight.Where(o => o % 4 == 0).ToArray();
                int[] getDimonds = Straight.Where(o => o % 4 == 1).ToArray();
                int[] getHearts = Straight.Where(o => o % 4 == 2).ToArray();
                int[] getSpades = Straight.Where(o => o % 4 == 3).ToArray();
                int[] clubes = getClubes.Select(o => o / 4).Distinct().ToArray();
                int[] diamonds = getDimonds.Select(o => o / 4).Distinct().ToArray();
                int[] hearts = getHearts.Select(o => o / 4).Distinct().ToArray();
                int[] spades = getSpades.Select(o => o / 4).Distinct().ToArray();
                Array.Sort(Straight);
                Array.Sort(clubes);
                Array.Sort(diamonds);
                Array.Sort(hearts);
                Array.Sort(spades);
                #endregion
                for (int index = 0; index < 16; index++)
                {
                    if (this.reservedGameCardsIndeces[index] == int.Parse(this.cardsPictureBoxList[card1].Tag.ToString()) &&
                        this.reservedGameCardsIndeces[index + 1] == int.Parse(this.cardsPictureBoxList[card2].Tag.ToString()))
                    {
                        this.assertHandType.rPairFromHand(player, index, this.pokerDatabase.Win, ref this.sorted, ref reservedGameCardsIndeces);

                        this.assertHandType.rPairTwoPair(player, index, this.pokerDatabase.Win, ref this.sorted, ref reservedGameCardsIndeces);

                        this.assertHandType.rTwoPair(player, index, this.pokerDatabase.Win, ref this.sorted, ref this.reservedGameCardsIndeces);

                        this.assertHandType.rThreeOfAKind(player, Straight, index, this.pokerDatabase.Win, ref this.sorted);

                        this.assertHandType.rStraight(player, Straight, index, this.pokerDatabase.Win, ref this.sorted);

                        this.assertHandType.rFlush(player, ref vf, cardsOnBoard, ref index, this.pokerDatabase.Win, ref this.sorted, ref this.reservedGameCardsIndeces);

                        this.assertHandType.rFullHouse(player, ref done, Straight, this.pokerDatabase.Win, ref this.sorted, ref this.type);

                        this.assertHandType.rFourOfAKind(player, Straight, this.pokerDatabase.Win, ref this.sorted);

                        this.assertHandType.rStraightFlush(player, clubes, diamonds, hearts, spades, this.pokerDatabase.Win, ref this.sorted);

                        this.assertHandType.rHighCard(player, index, this.pokerDatabase.Win, ref this.sorted, ref this.reservedGameCardsIndeces);
                    }
                }
            }
        }

        private void Winner(IPlayer player, string lastly)
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }

            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (this.cardsPictureBoxList[j].Visible)
                {
                    this.cardsPictureBoxList[j].Image = this.deskCardsAsImages[j];
                }
            }

            if (player.Type == sorted.Current)
            {
                if (player.Power == sorted.Power)
                {
                    this.winners++;
                    this.pokerDatabase.CheckWinners.Add(player.Name);
                    if (player.Type == -1)
                    {
                        MessageBox.Show(player.Type + " High Card ");
                    }

                    if (player.Type == 1 || player.Type == 0)
                    {
                        MessageBox.Show(player.Type + " Pair ");
                    }

                    if (player.Type == 2)
                    {
                        MessageBox.Show(player.Type + " Two Pair ");
                    }

                    if (player.Type == 3)
                    {
                        MessageBox.Show(player.Type + " Three of a Kind ");
                    }

                    if (player.Type == 4)
                    {
                        MessageBox.Show(player.Type + " Straight ");
                    }

                    if (player.Type == 5 || player.Type == 5.5)
                    {
                        MessageBox.Show(player.Type + " Flush ");
                    }

                    if (player.Type == 6)
                    {
                        MessageBox.Show(player.Type + " Full House ");
                    }

                    if (player.Type == 7)
                    {
                        MessageBox.Show(player.Type + " Four of a Kind ");
                    }

                    if (player.Type == 8)
                    {
                        MessageBox.Show(player.Type + " Straight Flush ");
                    }

                    if (player.Type == 9)
                    {
                        MessageBox.Show(player.Type + " Royal Flush ! ");
                    }
                }
            }
            if (player.Name == lastly)//lastfixed
            {
                if (this.winners > 1)
                {
                    if (this.pokerDatabase.CheckWinners.Contains("Player"))
                    {
                        this.human.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.txtBoxHumanChips.Text = this.human.Chips.ToString();
                        //playerPanel.Visible = true;

                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 1"))
                    {
                        this.firstBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.txtBoxFirstBotChips.Text = this.firstBot.Chips.ToString();
                        //bot1Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 2"))
                    {
                        this.secondBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.txtBoxSecondBotChips.Text = this.secondBot.Chips.ToString();
                        //bot2Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 3"))
                    {
                        this.thirdBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.txtBoxThirdBotChips.Text = this.thirdBot.Chips.ToString();
                        //bot3Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 4"))
                    {
                        this.fourthBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.txtBoxFourthBotChips.Text = this.fourthBot.Chips.ToString();
                        //bot4Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 5"))
                    {
                        this.fifthBot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.txtBoxFifthBotChips.Text = this.fifthBot.Chips.ToString();
                        //bot5Panel.Visible = true;
                    }
                    //await Finish(1);
                }
                if (this.winners == 1)
                {
                    if (this.pokerDatabase.CheckWinners.Contains("Player"))
                    {
                        this.human.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //playerPanel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 1"))
                    {
                        this.firstBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot1Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 2"))
                    {
                        this.secondBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot2Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 3"))
                    {
                        this.thirdBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot3Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 4"))
                    {
                        this.fourthBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot4Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 5"))
                    {
                        this.fifthBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot5Panel.Visible = true;
                    }
                }
            }
        }

        private async Task CheckRaise(int currentTurn, int raiseTurn)
        {
            if (this.raising)
            {
                this.turnCount = 0;
                this.raising = false;
                this.raisedTurn = currentTurn;
                this.changed = true;
            }
            else
            {
                if (this.turnCount >= this.maxLeft - 1 ||
                    !this.changed && this.turnCount == this.maxLeft)
                {
                    if (currentTurn == this.raisedTurn - 1 ||
                        !this.changed && this.turnCount == this.maxLeft ||
                        this.raisedTurn == 0 && currentTurn == 5)
                    {
                        this.changed = false;
                        this.turnCount = 0;
                        this.raise = 0;
                        this.neededChipsToCall = 0;
                        this.raisedTurn = 123;
                        this.rounds++;
                        if (!this.human.OutOfChips)
                        {
                            this.humanStatus.Text = "";
                        }

                        if (!this.firstBot.OutOfChips)
                        {
                            this.firstBotStatus.Text = "";
                        }

                        if (!this.secondBot.OutOfChips)
                        {
                            this.secondBotStatus.Text = "";
                        }

                        if (!this.thirdBot.OutOfChips)
                        {
                            this.thirdBotStatus.Text = "";
                        }

                        if (!this.fourthBot.OutOfChips)
                        {
                            this.fourthBotStatus.Text = "";
                        }

                        if (!this.fifthBot.OutOfChips)
                        {
                            this.fifthBotStatus.Text = "";
                        }

                    }
                }
            }
            if (this.rounds == this.flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (this.cardsPictureBoxList[j].Image != this.deskCardsAsImages[j])
                    {
                        this.ResetPlayersStats(j);
                    }
                }
            }
            if (this.rounds == this.turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (this.cardsPictureBoxList[j].Image != this.deskCardsAsImages[j])
                    {
                        this.ResetPlayersStats(j);
                    }
                }
            }
            if (this.rounds == this.river)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (this.cardsPictureBoxList[j].Image != this.deskCardsAsImages[j])
                    {
                        this.ResetPlayersStats(j);
                    }
                }
            }

            if (this.rounds == this.end && this.maxLeft == 6)
            {
                string fixedLast = "qwerty";
                if (!this.humanStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    this.Rules(0, 1, "Player", this.human);
                }

                if (!this.firstBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    this.Rules(2, 3, "Bot 1", this.firstBot);
                }

                if (!this.secondBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    this.Rules(4, 5, "Bot 2", this.thirdBot);
                }

                if (!this.thirdBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    this.Rules(6, 7, "Bot 3", this.thirdBot);
                }

                if (!this.fourthBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    this.Rules(8, 9, "Bot 4", this.fourthBot);
                }

                if (!this.fifthBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    this.Rules(10, 11, "Bot 5", this.fifthBot);
                }

                //TODO: COnsider passing the entire player object
                //TODO: COde repetition
                this.Winner(this.human, fixedLast);
                this.Winner(this.firstBot, fixedLast);
                this.Winner(this.secondBot, fixedLast);
                this.Winner(this.thirdBot, fixedLast);
                this.Winner(this.fourthBot, fixedLast);
                this.Winner(this.fifthBot, fixedLast);

                restart = true;

                this.human.CanMakeTurn = true;
                this.human.OutOfChips = false;
                this.firstBot.OutOfChips = false;
                this.secondBot.OutOfChips = false;
                this.thirdBot.OutOfChips = false;
                this.fourthBot.OutOfChips = false;
                this.fifthBot.OutOfChips = false;
                if (this.human.Chips <= 0)
                {
                    AddChips chipAdder = new AddChips();
                    chipAdder.ShowDialog();
                    if (chipAdder.chipsAmount != 0)
                    {
                        this.human.Chips = chipAdder.chipsAmount;
                        this.firstBot.Chips += chipAdder.chipsAmount;
                        this.secondBot.Chips += chipAdder.chipsAmount;
                        this.thirdBot.Chips += chipAdder.chipsAmount;
                        this.fourthBot.Chips += chipAdder.chipsAmount;
                        this.fifthBot.Chips += chipAdder.chipsAmount;
                        this.human.OutOfChips = false;
                        this.human.CanMakeTurn = true;
                        this.buttonRaise.Enabled = true;
                        this.buttonFold.Enabled = true;
                        this.buttonCheck.Enabled = true;
                        this.buttonRaise.Text = "Raise";
                    }
                }

                this.human.Panel.Visible = false;
                this.firstBot.Panel.Visible = false;
                this.secondBot.Panel.Visible = false;
                this.thirdBot.Panel.Visible = false;
                this.fourthBot.Panel.Visible = false;
                this.fifthBot.Panel.Visible = false;
                this.human.Call = 0;

                this.human.Raise = 0;
                this.firstBot.Call = 0;
                this.firstBot.Raise = 0;
                this.secondBot.Call = 0;
                this.secondBot.Raise = 0;
                this.thirdBot.Call = 0;
                this.thirdBot.Raise = 0;
                this.fourthBot.Call = 0;
                this.fourthBot.Raise = 0;
                this.fifthBot.Call = 0;
                this.fifthBot.Raise = 0;
                this.last = 0;
                this.neededChipsToCall = this.bigBlindValue;
                this.raise = 0;
                this.cardsImageLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                this.pokerDatabase.PlayersGameStatus.Clear();
                this.rounds = 0;
                this.type = 0;

                this.human.Power = 0;
                this.firstBot.Power = 0;
                this.secondBot.Power = 0;
                this.thirdBot.Power = 0;
                this.fourthBot.Power = 0;
                this.fifthBot.Power = 0;

                this.firstBot.Type = -1;
                this.secondBot.Type = -1;
                this.thirdBot.Type = -1;
                this.fourthBot.Type = -1;
                this.fifthBot.Type = -1;
                this.human.Type = -1;

                this.pokerDatabase.Chips.Clear();
                this.pokerDatabase.CheckWinners.Clear();
                this.winners = 0;
                this.pokerDatabase.Win.Clear();
                this.sorted.Current = 0;
                this.sorted.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    this.cardsPictureBoxList[os].Image = null;
                    this.cardsPictureBoxList[os].Invalidate();
                    this.cardsPictureBoxList[os].Visible = false;
                }

                this.potStatus.Text = "0";
                this.humanStatus.Text = "";
                await Shuffle();
                await Turns();
            }
        }

        private void ResetPlayersStats(int j)
        {
            this.cardsPictureBoxList[j].Image = this.deskCardsAsImages[j];
            this.human.Call = 0;
            this.human.Raise = 0;
            this.firstBot.Call = 0;
            this.firstBot.Raise = 0;
            this.secondBot.Call = 0;
            this.secondBot.Raise = 0;
            this.thirdBot.Call = 0;
            this.thirdBot.Raise = 0;
            this.fourthBot.Call = 0;
            this.fourthBot.Raise = 0;
            this.fifthBot.Call = 0;
            this.fifthBot.Raise = 0;
        }

        private void FixCall(Label status, int cCall, int cRaise, int options)
        {
            if (this.rounds != 4)
            {
                if (options == 1)
                {
                    if (status.Text.Contains("Raise"))
                    {
                        var changeRaise = status.Text.Substring(6);
                        cRaise = int.Parse(changeRaise);
                    }

                    if (status.Text.Contains("Call"))
                    {
                        var changeCall = status.Text.Substring(5);
                        cCall = int.Parse(changeCall);
                    }

                    if (status.Text.Contains("PlayerCheck"))
                    {
                        cRaise = 0;
                        cCall = 0;
                    }
                }
                if (options == 2)
                {
                    if (cRaise != this.raise && cRaise <= this.raise)
                    {
                        this.neededChipsToCall = Convert.ToInt32(this.raise) - cRaise;
                    }

                    if (cCall != this.neededChipsToCall || cCall <= this.neededChipsToCall)
                    {
                        this.neededChipsToCall = this.neededChipsToCall - cCall;
                    }

                    if (cRaise == this.raise && this.raise > 0)
                    {
                        this.neededChipsToCall = 0;
                        this.buttonCall.Enabled = false;
                        this.buttonCall.Text = "Callisfuckedup";
                    }
                }
            }
        }

        private async Task AllIn()
        {
            #region All in
            if (this.human.Chips <= 0 && !this.chipsAreAdded)
            {
                if (this.humanStatus.Text.Contains("Raise"))
                {
                    this.pokerDatabase.Chips.Add(this.human.Chips);
                    this.chipsAreAdded = true;
                }

                if (this.humanStatus.Text.Contains("Call"))
                {
                    this.pokerDatabase.Chips.Add(this.human.Chips);
                    this.chipsAreAdded = true;
                }
            }

            this.chipsAreAdded = false;

            if (this.firstBot.Chips <= 0 && !this.firstBot.OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.pokerDatabase.Chips.Add(this.firstBot.Chips);
                    this.chipsAreAdded = true;
                }

                this.chipsAreAdded = false;
            }

            if (this.secondBot.Chips <= 0 && !this.secondBot.OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.pokerDatabase.Chips.Add(this.secondBot.Chips);
                    this.chipsAreAdded = true;
                }

                this.chipsAreAdded = false;
            }

            if (this.thirdBot.Chips <= 0 && !this.thirdBot.OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.pokerDatabase.Chips.Add(this.thirdBot.Chips);
                    this.chipsAreAdded = true;
                }

                this.chipsAreAdded = false;
            }

            if (this.fourthBot.Chips <= 0 && !this.fourthBot.OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.pokerDatabase.Chips.Add(this.fourthBot.Chips);
                    this.chipsAreAdded = true;
                }

                this.chipsAreAdded = false;
            }

            if (this.fifthBot.Chips <= 0 && !this.fifthBot.OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.pokerDatabase.Chips.Add(this.fifthBot.Chips);
                    this.chipsAreAdded = true;
                }
            }

            if (this.pokerDatabase.Chips.ToArray().Length == this.maxLeft)
            {
                await Finish(2);
            }
            else
            {
                this.pokerDatabase.Chips.Clear();
            }
            #endregion
            //TODO: previous name abs
            var leftPlayers = this.pokerDatabase.PlayersGameStatus.Count(x => x == false);

            #region LastManStanding
            if (leftPlayers == 1)
            {
                int index = this.pokerDatabase.PlayersGameStatus.IndexOf(false);
                if (index == 0)
                {
                    this.human.Chips += int.Parse(this.potStatus.Text);
                    this.txtBoxHumanChips.Text = this.human.Chips.ToString();
                    this.human.Panel.Visible = true;
                    MessageBox.Show("Player Wins");
                }

                if (index == 1)
                {
                    this.firstBot.Chips += int.Parse(this.potStatus.Text);
                    this.txtBoxHumanChips.Text = this.firstBot.Chips.ToString();
                    this.firstBot.Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }

                if (index == 2)
                {
                    this.secondBot.Chips += int.Parse(this.potStatus.Text);
                    this.txtBoxHumanChips.Text = this.secondBot.Chips.ToString();
                    this.secondBot.Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }

                if (index == 3)
                {
                    this.thirdBot.Chips += int.Parse(this.potStatus.Text);
                    this.txtBoxHumanChips.Text = this.thirdBot.Chips.ToString();
                    this.thirdBot.Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }

                if (index == 4)
                {
                    this.fourthBot.Chips += int.Parse(this.potStatus.Text);
                    this.txtBoxHumanChips.Text = this.fourthBot.Chips.ToString();
                    this.fourthBot.Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }

                if (index == 5)
                {
                    this.fifthBot.Chips += int.Parse(this.potStatus.Text);
                    this.txtBoxHumanChips.Text = this.fifthBot.Chips.ToString();
                    this.fifthBot.Panel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }

                for (int j = 0; j <= 16; j++)
                {
                    this.cardsPictureBoxList[j].Visible = false;
                }

                await Finish(1);
            }
            this.chipsAreAdded = false;
            #endregion

            #region FiveOrLessLeft
            if (leftPlayers < 6 && leftPlayers > 1 && this.rounds >= this.end)
            {
                await Finish(2);
            }
            #endregion
        }

        private async Task Finish(int n)
        {
            if (n == 2)
            {
                FixWinners();
            }

            this.human.Panel.Visible = false;
            this.firstBot.Panel.Visible = false;
            this.secondBot.Panel.Visible = false;
            this.thirdBot.Panel.Visible = false;
            this.fourthBot.Panel.Visible = false;
            this.fifthBot.Panel.Visible = false;

            this.neededChipsToCall = this.bigBlindValue;
            this.raise = 0;
            this.foldedPlayers = 5;
            this.type = 0;
            this.rounds = 0;

            this.firstBot.Power = 0;
            this.secondBot.Power = 0;
            this.thirdBot.Power = 0;
            this.fourthBot.Power = 0;
            this.fifthBot.Power = 0;
            this.human.Power = 0;

            this.raise = 0;

            this.human.Type = -1;
            this.firstBot.Type = -1;
            this.secondBot.Type = -1;
            this.thirdBot.Type = -1;
            this.fourthBot.Type = -1;
            this.fifthBot.Type = -1;

            this.firstBot.CanMakeTurn = false;
            this.secondBot.CanMakeTurn = false;
            this.thirdBot.CanMakeTurn = false;
            this.fourthBot.CanMakeTurn = false;
            this.fifthBot.CanMakeTurn = false;
            this.firstBot.OutOfChips = false;
            this.secondBot.OutOfChips = false;
            this.thirdBot.OutOfChips = false;
            this.fourthBot.OutOfChips = false;
            this.fifthBot.OutOfChips = false;
            this.human.Folded = false;
            this.firstBot.Folded = false;
            this.secondBot.Folded = false;
            this.thirdBot.Folded = false;
            this.fourthBot.Folded = false;
            this.fifthBot.Folded = false;
            this.human.OutOfChips = false;
            this.human.CanMakeTurn = true;
            this.restart = false;
            this.raising = false;
            this.human.Call = 0;
            this.firstBot.Call = 0;
            this.secondBot.Call = 0;
            this.thirdBot.Call = 0;
            this.fourthBot.Call = 0;
            this.fifthBot.Call = 0;
            this.human.Raise = 0;
            this.firstBot.Raise = 0;
            this.secondBot.Raise = 0;
            this.thirdBot.Raise = 0;
            this.fourthBot.Raise = 0;
            this.fifthBot.Raise = 0;
            this.height = 0;
            this.width = 0;
            this.winners = 0;
            this.flop = 1;
            this.turn = 2;
            this.river = 3;
            this.end = 4;
            this.maxLeft = 6;
            this.last = 123;
            this.raisedTurn = 1;
            this.pokerDatabase.PlayersGameStatus.Clear();
            this.pokerDatabase.CheckWinners.Clear();
            this.pokerDatabase.Chips.Clear();
            this.pokerDatabase.Win.Clear();
            this.sorted.Current = 0;
            this.sorted.Power = 0;
            this.potStatus.Text = "0";
            this.secondsLeft = 60;
            this.up = 10000000;
            this.turnCount = 0;
            this.humanStatus.Text = string.Empty;
            this.firstBotStatus.Text = string.Empty;
            this.secondBotStatus.Text = string.Empty;
            this.thirdBotStatus.Text = string.Empty;
            this.fourthBotStatus.Text = string.Empty;
            this.fifthBotStatus.Text = string.Empty;

            if (this.human.Chips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.chipsAmount != 0)
                {
                    this.human.Chips = f2.chipsAmount;
                    this.fifthBot.Chips += f2.chipsAmount;
                    this.secondBot.Chips += f2.chipsAmount;
                    this.thirdBot.Chips += f2.chipsAmount;
                    this.fourthBot.Chips += f2.chipsAmount;
                    this.fifthBot.Chips += f2.chipsAmount;
                    this.human.OutOfChips = false;
                    this.human.CanMakeTurn = true;
                    this.buttonRaise.Enabled = true;
                    this.buttonFold.Enabled = true;
                    this.buttonCheck.Enabled = true;
                    this.buttonRaise.Text = "Raise";
                }
            }

            this.cardsImageLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
            for (int os = 0; os < 17; os++)
            {
                this.cardsPictureBoxList[os].Image = null;
                this.cardsPictureBoxList[os].Invalidate();
                this.cardsPictureBoxList[os].Visible = false;
            }

            await Shuffle();
            //await Turns();
        }

        private void FixWinners()
        {
            this.pokerDatabase.Win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            string fixedLast = "qwerty";
            if (!this.humanStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                this.Rules(0, 1, "Player", this.human);
            }

            if (!this.firstBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                this.Rules(2, 3, "Bot 1", this.firstBot);
            }

            if (!this.secondBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                this.Rules(4, 5, "Bot 2", this.secondBot);
            }

            if (!this.thirdBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                this.Rules(6, 7, "Bot 3", this.thirdBot);
            }

            if (!this.fourthBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                this.Rules(8, 9, "Bot 4", this.fourthBot);
            }

            if (!this.fifthBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                this.Rules(10, 11, "Bot 5", this.fifthBot);
            }

            //TODO: code repetition
            this.Winner(this.human, fixedLast);
            this.Winner(this.firstBot, fixedLast);
            this.Winner(this.secondBot, fixedLast);
            this.Winner(this.thirdBot, fixedLast);
            this.Winner(this.fourthBot, fixedLast);
            this.Winner(this.fifthBot, fixedLast);
        }

        //TODO: Prevous name AI
        private void AI(int c1, int c2, Label sStatus, int name, IPlayer player)
        {
            if (!player.OutOfChips)
            {
                if (player.Type == -1)
                {
                    this.handType.HighCard(player, sStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising);
                }

                if (player.Type == 0)
                {
                    this.handType.PairTable(player, sStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising);
                }

                if (player.Type == 1)
                {
                    this.handType.PairHand(player, sStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 2)
                {
                    this.handType.TwoPair(player, sStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 3)
                {
                    this.handType.ThreeOfAKind(player, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 4)
                {
                    this.handType.Straight(player, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 5 || player.Type == 5.5)
                {
                    this.handType.Flush(player, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 6)
                {
                    this.handType.FullHouse(player, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 7)
                {
                    this.handType.FourOfAKind(player, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 8 || player.Type == 9)
                {
                    this.handType.StraightFlush(player, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }
            }

            if (player.OutOfChips)
            {
                this.cardsPictureBoxList[c1].Visible = false;
                this.cardsPictureBoxList[c2].Visible = false;
            }
        }

        #region UI
        private async void TimerTick(object sender, object e)
        {
            if (pbTimer.Value <= 0)
            {
                this.human.OutOfChips = true;
                await this.Turns();
            }

            if (this.secondsLeft > 0)
            {
                this.secondsLeft--;
                pbTimer.Value = (this.secondsLeft / 6) * 100;
            }
        }

        private void UpdateTick(object sender, object e)
        {
            if (this.human.Chips <= 0)
            {
                this.txtBoxHumanChips.Text = "Chips : 0";
            }

            if (this.firstBot.Chips <= 0)
            {
                this.txtBoxFirstBotChips.Text = "Chips : 0";
            }

            if (this.secondBot.Chips <= 0)
            {
                this.txtBoxSecondBotChips.Text = "Chips : 0";
            }

            if (this.thirdBot.Chips <= 0)
            {
                this.txtBoxThirdBotChips.Text = "Chips : 0";
            }

            if (this.fourthBot.Chips <= 0)
            {
                this.txtBoxFourthBotChips.Text = "Chips : 0";
            }

            if (this.fifthBot.Chips <= 0)
            {
                this.txtBoxFifthBotChips.Text = "Chips : 0";
            }

            //TODO: extact in method
            this.txtBoxHumanChips.Text = "Chips : " + this.human.Chips;
            this.txtBoxFirstBotChips.Text = "Chips : " + this.firstBot.Chips;
            this.txtBoxSecondBotChips.Text = "Chips : " + this.secondBot.Chips;
            this.txtBoxThirdBotChips.Text = "Chips : " + this.thirdBot.Chips;
            this.txtBoxFourthBotChips.Text = "Chips : " + this.fourthBot.Chips;
            this.txtBoxFifthBotChips.Text = "Chips : " + this.fifthBot.Chips;

            if (this.human.Chips <= 0)
            {
                this.human.CanMakeTurn = false;
                this.human.OutOfChips = true;
                this.buttonCall.Enabled = false;
                this.buttonRaise.Enabled = false;
                this.buttonFold.Enabled = false;
                this.buttonCheck.Enabled = false;
            }

            if (up > 0)
            {
                up--;
            }

            if (this.human.Chips >= this.neededChipsToCall)
            {
                this.buttonCall.Text = "Call " + this.neededChipsToCall.ToString();
            }
            else
            {
                this.buttonCall.Text = "All in";
                this.buttonRaise.Enabled = false;
            }

            if (this.neededChipsToCall > 0)
            {
                this.buttonCheck.Enabled = false;
            }

            if (this.neededChipsToCall <= 0)
            {
                this.buttonCheck.Enabled = true;
                this.buttonCall.Text = "Call";
                this.buttonCall.Enabled = false;
            }

            if (this.human.Chips <= 0)
            {
                this.buttonRaise.Enabled = false;
            }

            int parsedValue;

            if (tbRaise.Text != "" && int.TryParse(tbRaise.Text, out parsedValue))
            {
                if (this.human.Chips <= int.Parse(tbRaise.Text))
                {
                    this.buttonRaise.Text = "All in";
                }
                else
                {
                    this.buttonRaise.Text = "Raise";
                }
            }

            if (this.human.Chips < this.neededChipsToCall)
            {
                this.buttonRaise.Enabled = false;
            }
        }

        private async void ButtonFold_Click(object sender, EventArgs e)
        {
            this.humanStatus.Text = "Fold";
            this.human.CanMakeTurn = false;
            this.human.OutOfChips = true;
            await Turns();
        }

        private async void ButtonCheck_Click(object sender, EventArgs e)
        {
            if (this.neededChipsToCall <= 0)
            {
                this.human.CanMakeTurn = false;
                this.humanStatus.Text = "Check";
            }
            else
            {
                //humanStatus.Text = "All in " + Chips;
                this.buttonCheck.Enabled = false;
            }
            await Turns();
        }

        private async void ButtonCall_Click(object sender, EventArgs e)
        {
            this.Rules(0, 1, "Player", this.human);
            if (this.human.Chips >= this.neededChipsToCall)
            {
                this.human.Chips -= this.neededChipsToCall;
                this.txtBoxHumanChips.Text = "Chips : " + this.human.Chips.ToString();
                if (this.potStatus.Text != "")
                {
                    this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.neededChipsToCall).ToString();
                }
                else
                {
                    this.potStatus.Text = this.neededChipsToCall.ToString();
                }

                this.human.CanMakeTurn = false;
                this.humanStatus.Text = "Call " + this.neededChipsToCall;
                this.human.Call = this.neededChipsToCall;
            }
            else if (this.human.Chips <= this.neededChipsToCall && this.neededChipsToCall > 0)
            {
                this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.human.Chips).ToString();
                this.humanStatus.Text = "All in " + this.human.Chips;
                this.human.Chips = 0;
                this.txtBoxHumanChips.Text = "Chips : " + this.human.Chips.ToString();
                this.human.CanMakeTurn = false;
                this.buttonFold.Enabled = false;
                this.human.Call = this.human.Chips;
            }

            await this.Turns();
        }

        private async void ButtonRaise_Click(object sender, EventArgs e)
        {
            this.Rules(0, 1, "Player", this.human);
            int parsedValue;
            if (tbRaise.Text != "" && int.TryParse(tbRaise.Text, out parsedValue))
            {
                if (this.human.Chips > this.neededChipsToCall)
                {
                    if (this.raise * 2 > int.Parse(tbRaise.Text))
                    {
                        tbRaise.Text = (this.raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (this.human.Chips >= int.Parse(tbRaise.Text))
                        {
                            this.neededChipsToCall = int.Parse(tbRaise.Text);
                            this.raise = int.Parse(tbRaise.Text);
                            this.humanStatus.Text = "Raise " + this.neededChipsToCall.ToString();
                            this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.neededChipsToCall).ToString();
                            this.buttonCall.Text = "Call";
                            this.human.Chips -= int.Parse(tbRaise.Text);
                            this.raising = true;
                            last = 0;
                            this.human.Raise = Convert.ToInt32(this.raise);
                        }
                        else
                        {
                            this.neededChipsToCall = this.human.Chips;
                            this.raise = this.human.Chips;
                            this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.human.Chips).ToString();
                            this.humanStatus.Text = "raise " + this.neededChipsToCall.ToString();
                            this.human.Chips = 0;
                            this.raising = true;
                            last = 0;
                            this.human.Raise = Convert.ToInt32(this.raise);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }

            this.human.CanMakeTurn = false;
            await Turns();
        }

        //TODO: validate pokerDatabaseChips are integer
        private void ButtonAddChips_Click(object sender, EventArgs e)
        {
            if (tbAdd.Text == "")
            {

            }
            else
            {
                this.human.Chips += int.Parse(tbAdd.Text);
                this.firstBot.Chips += int.Parse(tbAdd.Text);
                this.secondBot.Chips += int.Parse(tbAdd.Text);
                this.thirdBot.Chips += int.Parse(tbAdd.Text);
                this.fourthBot.Chips += int.Parse(tbAdd.Text);
                this.fifthBot.Chips += int.Parse(tbAdd.Text);
            }

            this.txtBoxHumanChips.Text = "Chips : " + this.human.Chips;
        }

        private void ButtonOptions_Click(object sender, EventArgs e)
        {
            this.tbBigBlind.Text = this.bigBlindValue.ToString();
            this.tbSmallBlind.Text = this.smallBlindValue.ToString();

            if (this.tbBigBlind.Visible == false)
            {
                this.tbBigBlind.Visible = true;
                this.tbSmallBlind.Visible = true;
                this.buttonBigBlind.Visible = true;
                buttonSmallBlind.Visible = true;
            }
            else
            {
                this.tbBigBlind.Visible = false;
                this.tbSmallBlind.Visible = false;
                this.buttonBigBlind.Visible = false;
                buttonSmallBlind.Visible = false;
            }
        }

        //TODO: Small and big blind have similar logic, extract in different method
        private void ButtonSmallBlind_Click(object sender, EventArgs e)
        {
            int smallBlindNewValue;
            if (this.tbSmallBlind.Text.Contains(",") || this.tbSmallBlind.Text.Contains("."))
            {
                MessageBox.Show("The Small Blind can be only round number !");
                this.tbSmallBlind.Text = this.smallBlindValue.ToString();

                return;
            }

            if (!int.TryParse(this.tbSmallBlind.Text, out smallBlindNewValue))
            {
                MessageBox.Show("This is a number only field");
                this.tbSmallBlind.Text = this.smallBlindValue.ToString();

                return;
            }

            if (int.Parse(this.tbSmallBlind.Text) > 100000)
            {
                MessageBox.Show("The maximum of the Small Blind is 100 000 $");
                this.tbSmallBlind.Text = this.smallBlindValue.ToString();
            }

            if (int.Parse(this.tbSmallBlind.Text) < 250)
            {
                MessageBox.Show("The minimum of the Small Blind is 250 $");
            }

            if (int.Parse(this.tbSmallBlind.Text) >= 250 && int.Parse(this.tbSmallBlind.Text) <= 100000)
            {
                this.smallBlindValue = int.Parse(this.tbSmallBlind.Text);
                MessageBox.Show(
                    "The changes have been saved ! They will become available the next hand you play. ");
            }
        }

        private void ButtonBigBlind_Click(object sender, EventArgs e)
        {
            int bigBlindNewValue;
            if (this.tbBigBlind.Text.Contains(",") || this.tbBigBlind.Text.Contains("."))
            {
                MessageBox.Show("The Big Blind can be only round number !");
                this.tbBigBlind.Text = this.bigBlindValue.ToString();
                return;
            }

            if (!int.TryParse(tbBigBlind.Text, out bigBlindNewValue))
            {
                MessageBox.Show("This is a number only field");
                tbBigBlind.Text = this.bigBlindValue.ToString();
                return;
            }

            if (int.Parse(this.tbBigBlind.Text) > 200000)
            {
                MessageBox.Show("The maximum of the Big Blind is 200 000");
                this.tbBigBlind.Text = this.bigBlindValue.ToString();
            }

            if (int.Parse(this.tbBigBlind.Text) < 500)
            {
                MessageBox.Show("The minimum of the Big Blind is 500 $");
            }

            if (int.Parse(this.tbBigBlind.Text) >= 500 && int.Parse(this.tbBigBlind.Text) <= 200000)
            {
                this.bigBlindValue = int.Parse(this.tbBigBlind.Text);
                MessageBox.Show(
                    "The changes have been saved ! They will become available the next hand you play. ");
            }
        }

        private void ChangeLayout(object sender, LayoutEventArgs e)
        {
            width = this.Width;
            height = this.Height;
        }
        #endregion
    }
}