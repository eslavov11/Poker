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

    public partial class PokerTable : Form
    {
        #region Variables
        //private readonly ICheckHandType checkHand;
        private readonly IHandType handType;

        private IPlayer human;
        private IPlayer firstBot;
        private IPlayer secondBot;
        private IPlayer thirdBot;
        private IPlayer fourthBot;
        private IPlayer fifthBot;


        private ProgressBar asd = new ProgressBar();
        private int Nm;
        //private Panel playerPanel = new Panel();
        //private Panel bot1Panel = new Panel();
        //private Panel bot2Panel = new Panel();
        //private Panel bot3Panel = new Panel();
        //private Panel bot4Panel = new Panel();
        //private Panel bot5Panel = new Panel();
        
        private int neededChipsToCall = 500;
        private int foldedPlayers = 5;

        //private int Chips = 10000;
        //private int bot1Chips = 10000;
        //private int bot2Chips = 10000;
        //private int bot3Chips = 10000;
        //private int bot4Chips = 10000;
        //private int bot5Chips = 10000;

        private double type;
        private int rounds;

        //private double b1Power;
        //private double b2Power;
        //private double b3Power;
        //private double b4Power;
        //private double b5Power;
        //private double pPower;

        private int raise;

        //private double humanBotType = -1;
        //private double firstBotType = -1;
        //private double secondBotType = -1;
        //private double thirdBotType = -1;
        //private double fourthBotType = -1;
        //private double fifthBotType = -1;

        //private bool firstBotCanMakeTurn;
        //private bool secondBotCanMakeTurn;
        //private bool thirdBotCanMakeTurn;
        //private bool fourthBotCanMakeTurn;
        //private bool fifthBotCanMakeTurn;
        //private bool humanCanMakeTurn = true;


        //B1Fturn
        //private bool firstBotOutOfChips;
        //private bool secondBotOutOfChips;
        //private bool thirdBotOutOfChips;
        //private bool fourthBotOutOfChips;
        //private bool fifthBotOutOfChips;
        //private bool humanOutOfChips;

        //private bool humanFolded;
        //private bool firstBotFolded;
        //private bool secondBotFolded;
        //private bool thirdBotFolded;
        //private bool fourthBotFolded;
        //private bool fifthBotFolded;

        private bool intsadded;
        private bool changed;

        //private int humanCall;
        //private int firstBotCall;
        //private int secondBotCall;
        //private int thirdBotCall;
        //private int fourthBotCall;
        //private int fifthBotCall;

        //private int humanRaise;
        //private int firstBotRaise;
        //private int secondBotRaise;
        //private int thirdBotRaise;
        //private int fourthBotRaise;
        //private int fifthBotRaise;

        private int height;
        private int width;

        private int winners = 0;


        private int Flop = 1;
        private int Turn = 2;
        private int River = 3;
        private int End = 4;

        private int maxLeft = 6;
        private int last = 123;
        private int raisedTurn = 1;

        //TODO: PlayerCheck if name is proper, previous name - bools
        private List<bool?> playersGameStatus = new List<bool?>();
        private List<Type> Win = new List<Type>();
        private List<string> CheckWinners = new List<string>();
        //TODO: PlayerCheck if name is proper, previous name - chips
        private List<int> chips = new List<int>();

        private bool restart = false;
        private bool raising = false;
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
        private Timer Updates = new Timer();

        private int t = 60;
        private int i;
        private int bigBlindValue = 500;
        private int smallBlindValue = 250;
        private int up = 10000000;
        private int turnCount = 0;
        #endregion

        public PokerTable()
        {
            this.human = new Human();
            this.firstBot = new Bot();
            this.secondBot = new Bot();
            this.thirdBot = new Bot();
            this.fourthBot = new Bot();
            this.fifthBot = new Bot();

            //playersGameStatus.Add(humanOutOfChips); playersGameStatus.Add(firstBotOutOfChips); playersGameStatus.Add(secondBotOutOfChips); playersGameStatus.Add(thirdBotOutOfChips); playersGameStatus.Add(fourthBotOutOfChips); playersGameStatus.Add(fifthBotOutOfChips);
            this.neededChipsToCall = this.bigBlindValue;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Updates.Start();
            this.InitializeComponent();
            this.width = this.Width;
            this.height = this.Height;
            this.Shuffle();
            this.potStatus.Enabled = false;
            this.tbChips.Enabled = false;
            this.tbBotChips1.Enabled = false;
            this.tbBotChips2.Enabled = false;
            this.tbBotChips3.Enabled = false;
            this.tbBotChips4.Enabled = false;
            this.tbBotChips5.Enabled = false;
            this.tbChips.Text = "Chips : " + this.human.Chips;
            this.tbBotChips1.Text = "Chips : " + this.firstBot.Chips;
            this.tbBotChips2.Text = "Chips : " + this.secondBot.Chips;
            this.tbBotChips3.Text = "Chips : " + this.thirdBot.Chips;
            this.tbBotChips4.Text = "Chips : " + this.fourthBot.Chips;
            this.tbBotChips5.Text = "Chips : " + this.fifthBot.Chips;
            this.timer.Interval = (1 * 1 * 1000);
            this.timer.Tick += this.TimerTick;
            this.Updates.Interval = (1 * 1 * 100);
            this.Updates.Tick += this.UpdateTick;
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

        async Task Shuffle()
        {
            this.playersGameStatus.Add(this.human.OutOfChips);
            this.playersGameStatus.Add(this.firstBot.OutOfChips);
            this.playersGameStatus.Add(this.secondBot.OutOfChips);
            this.playersGameStatus.Add(this.thirdBot.OutOfChips);
            this.playersGameStatus.Add(this.fourthBot.OutOfChips);
            this.playersGameStatus.Add(this.fifthBot.OutOfChips);
            this.bCall.Enabled = false;
            this.bRaise.Enabled = false;
            this.bFold.Enabled = false;
            this.bCheck.Enabled = false;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            bool check = false;
            Bitmap backImage = new Bitmap("..\\..\\Resources\\Assets\\Back\\Back.png");
            int horizontal = 580;
            int vertical = 480;

            var randomCardLocation = new Random();

            //Shuffle cards location
            for (int cardLocationIndex = DefaultCardsInDesk;
                cardLocationIndex > 0;
                cardLocationIndex--)
            {
                //Swaps two cards locations from the desk, taking one random and replacing it with the 
                //card location from the loop index
                int randomCardIndex = randomCardLocation.Next(cardLocationIndex);
                string oldCardLocation = this.cardsImageLocation[randomCardIndex];
                this.cardsImageLocation[randomCardIndex] = this.cardsImageLocation[cardLocationIndex - 1];
                this.cardsImageLocation[cardLocationIndex - 1] = oldCardLocation;
            }

            for (i = 0; i < 17; i++)
            {

                this.deskCardsAsImages[i] = Image.FromFile(this.cardsImageLocation[i]);
                var partsToRemove = new[] { "..\\..\\Resources\\Assets\\Cards\\", ".png" };
                foreach (string part in partsToRemove)
                {
                    this.cardsImageLocation[i] = this.cardsImageLocation[i]
                        .Replace(part, string.Empty);
                }

                this.reservedGameCardsIndeces[i] = int.Parse(this.cardsImageLocation[i]) - 1;
                this.cardsPictureBoxList[i] = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Height = 130,
                    Width = 80
                };
                this.Controls.Add(this.cardsPictureBoxList[i]);
                this.cardsPictureBoxList[i].Name = "pb" + i.ToString();
                await Task.Delay(200);

                //TODO: Should this region be in the for loop at all?
                #region Throwing Cards
                if (i < 2)
                {
                    if (this.cardsPictureBoxList[0].Tag != null)
                    {
                        this.cardsPictureBoxList[1].Tag = this.reservedGameCardsIndeces[1];
                    }

                    this.cardsPictureBoxList[0].Tag = this.reservedGameCardsIndeces[0];
                    this.cardsPictureBoxList[i].Image = this.deskCardsAsImages[i];
                    this.cardsPictureBoxList[i].Anchor = (AnchorStyles.Bottom);
                    //cardsPictureBoxList[i].Dock = DockStyle.Top;
                    this.cardsPictureBoxList[i].Location = new Point(horizontal, vertical);
                    horizontal += this.cardsPictureBoxList[i].Width;
                    this.Controls.Add(this.human.Panel);
                    this.human.InitializePanel(new Point(
                        this.cardsPictureBoxList[0].Left - 10,
                        this.cardsPictureBoxList[0].Top - 10));
                }

                if (this.firstBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 2 && i < 4)
                    {
                        if (this.cardsPictureBoxList[2].Tag != null)
                        {
                            this.cardsPictureBoxList[3].Tag = this.reservedGameCardsIndeces[3];
                        }

                        this.cardsPictureBoxList[2].Tag = this.reservedGameCardsIndeces[2];
                        if (!check)
                        {
                            horizontal = 15;
                            vertical = 420;
                        }

                        check = true;
                        this.cardsPictureBoxList[i].Anchor = (AnchorStyles.Bottom | AnchorStyles.Left);
                        this.cardsPictureBoxList[i].Image = backImage;
                        //cardsPictureBoxList[i].Image = deskCardsAsImages[i];
                        this.cardsPictureBoxList[i].Location = new Point(horizontal, vertical);
                        horizontal += this.cardsPictureBoxList[i].Width;
                        this.cardsPictureBoxList[i].Visible = true;
                        this.Controls.Add(this.firstBot.Panel);
                        this.firstBot.InitializePanel(new Point(
                            this.cardsPictureBoxList[2].Left - 10,
                            this.cardsPictureBoxList[2].Top - 10));

                        if (i == 3)
                        {
                            check = false;
                        }
                    }
                }

                if (this.secondBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 4 && i < 6)
                    {
                        if (this.cardsPictureBoxList[4].Tag != null)
                        {
                            this.cardsPictureBoxList[5].Tag = this.reservedGameCardsIndeces[5];
                        }
                        this.cardsPictureBoxList[4].Tag = this.reservedGameCardsIndeces[4];
                        if (!check)
                        {
                            horizontal = 75;
                            vertical = 65;
                        }
                        check = true;
                        this.cardsPictureBoxList[i].Anchor = (AnchorStyles.Top | AnchorStyles.Left);
                        this.cardsPictureBoxList[i].Image = backImage;
                        //cardsPictureBoxList[i].Image = deskCardsAsImages[i];
                        this.cardsPictureBoxList[i].Location = new Point(horizontal, vertical);
                        horizontal += this.cardsPictureBoxList[i].Width;
                        this.cardsPictureBoxList[i].Visible = true;
                        this.Controls.Add(this.secondBot.Panel);
                        this.secondBot.InitializePanel(new Point(
                            this.cardsPictureBoxList[4].Left - 10,
                            this.cardsPictureBoxList[4].Top - 10));

                        if (i == 5)
                        {
                            check = false;
                        }
                    }
                }

                if (this.thirdBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 6 && i < 8)
                    {
                        if (this.cardsPictureBoxList[6].Tag != null)
                        {
                            this.cardsPictureBoxList[7].Tag = this.reservedGameCardsIndeces[7];
                        }

                        this.cardsPictureBoxList[6].Tag = this.reservedGameCardsIndeces[6];
                        if (!check)
                        {
                            horizontal = 590;
                            vertical = 25;
                        }
                        check = true;
                        this.cardsPictureBoxList[i].Anchor = (AnchorStyles.Top);
                        this.cardsPictureBoxList[i].Image = backImage;
                        //cardsPictureBoxList[i].Image = deskCardsAsImages[i];
                        this.cardsPictureBoxList[i].Location = new Point(horizontal, vertical);
                        horizontal += this.cardsPictureBoxList[i].Width;
                        this.cardsPictureBoxList[i].Visible = true;
                        this.Controls.Add(this.thirdBot.Panel);
                        this.thirdBot.InitializePanel(new Point(
                            this.cardsPictureBoxList[6].Left - 10,
                            this.cardsPictureBoxList[6].Top - 10));

                        if (i == 7)
                        {
                            check = false;
                        }
                    }
                }

                if (this.fourthBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 8 && i < 10)
                    {
                        if (this.cardsPictureBoxList[8].Tag != null)
                        {
                            this.cardsPictureBoxList[9].Tag = this.reservedGameCardsIndeces[9];
                        }

                        this.cardsPictureBoxList[8].Tag = this.reservedGameCardsIndeces[8];
                        if (!check)
                        {
                            horizontal = 1115;
                            vertical = 65;
                        }

                        check = true;
                        this.cardsPictureBoxList[i].Anchor = (AnchorStyles.Top | AnchorStyles.Right);
                        this.cardsPictureBoxList[i].Image = backImage;
                        //cardsPictureBoxList[i].Image = deskCardsAsImages[i];
                        this.cardsPictureBoxList[i].Location = new Point(horizontal, vertical);
                        horizontal += this.cardsPictureBoxList[i].Width;
                        this.cardsPictureBoxList[i].Visible = true;
                        this.Controls.Add(this.fourthBot.Panel);
                        this.fourthBot.InitializePanel(new Point(
                            this.cardsPictureBoxList[8].Left - 10,
                            this.cardsPictureBoxList[8].Top - 10));

                        if (i == 9)
                        {
                            check = false;
                        }
                    }
                }

                if (this.fifthBot.Chips > 0)
                {
                    foldedPlayers--;
                    if (i >= 10 && i < 12)
                    {
                        if (this.cardsPictureBoxList[10].Tag != null)
                        {
                            this.cardsPictureBoxList[11].Tag = this.reservedGameCardsIndeces[11];
                        }

                        this.cardsPictureBoxList[10].Tag = this.reservedGameCardsIndeces[10];
                        if (!check)
                        {
                            horizontal = 1160;
                            vertical = 420;
                        }
                        check = true;
                        this.cardsPictureBoxList[i].Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
                        this.cardsPictureBoxList[i].Image = backImage;
                        //cardsPictureBoxList[i].Image = deskCardsAsImages[i];
                        this.cardsPictureBoxList[i].Location = new Point(horizontal, vertical);
                        horizontal += this.cardsPictureBoxList[i].Width;
                        this.cardsPictureBoxList[i].Visible = true;
                        this.Controls.Add(this.fifthBot.Panel);
                        this.fifthBot.InitializePanel(
                            new Point(
                                this.cardsPictureBoxList[10].Left - 10,
                                this.cardsPictureBoxList[10].Top - 10));

                        if (i == 11)
                        {
                            check = false;
                        }
                    }
                }

                if (i >= 12)
                {
                    this.cardsPictureBoxList[12].Tag = this.reservedGameCardsIndeces[12];
                    if (i > 12)
                    {
                        this.cardsPictureBoxList[13].Tag = this.reservedGameCardsIndeces[13];
                    }

                    if (i > 13)
                    {
                        this.cardsPictureBoxList[14].Tag = this.reservedGameCardsIndeces[14];
                    }

                    if (i > 14)
                    {
                        this.cardsPictureBoxList[15].Tag = this.reservedGameCardsIndeces[15];
                    }

                    if (i > 15)
                    {
                        this.cardsPictureBoxList[16].Tag = this.reservedGameCardsIndeces[16];

                    }

                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }

                    check = true;
                    if (this.cardsPictureBoxList[i] != null)
                    {
                        this.cardsPictureBoxList[i].Anchor = AnchorStyles.None;
                        this.cardsPictureBoxList[i].Image = backImage;
                        //cardsPictureBoxList[i].Image = deskCardsAsImages[i];
                        this.cardsPictureBoxList[i].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }

                #endregion

                //TODO: extract methods logic below is completely identical
                #region CheckForDefeatedBots
                if (this.firstBot.Chips <= 0)
                {
                    this.firstBot.OutOfChips = true;
                    this.cardsPictureBoxList[2].Visible = false;
                    this.cardsPictureBoxList[3].Visible = false;
                }
                else
                {
                    this.firstBot.OutOfChips = false;
                    if (i == 3)
                    {
                        if (this.cardsPictureBoxList[3] != null)
                        {
                            //TODO: Is this working properly?
                            this.cardsPictureBoxList[2].Visible = true;
                            this.cardsPictureBoxList[3].Visible = true;
                        }
                    }
                }

                if (this.secondBot.Chips <= 0)
                {
                    this.secondBot.OutOfChips = true;
                    this.cardsPictureBoxList[4].Visible = false;
                    this.cardsPictureBoxList[5].Visible = false;
                }
                else
                {
                    this.secondBot.OutOfChips = false;
                    if (i == 5)
                    {
                        if (this.cardsPictureBoxList[5] != null)
                        {
                            this.cardsPictureBoxList[4].Visible = true;
                            this.cardsPictureBoxList[5].Visible = true;
                        }
                    }
                }

                if (this.thirdBot.Chips <= 0)
                {
                    this.thirdBot.OutOfChips = true;
                    this.cardsPictureBoxList[6].Visible = false;
                    this.cardsPictureBoxList[7].Visible = false;
                }
                else
                {
                    this.thirdBot.OutOfChips = false;
                    if (i == 7)
                    {
                        if (this.cardsPictureBoxList[7] != null)
                        {
                            this.cardsPictureBoxList[6].Visible = true;
                            this.cardsPictureBoxList[7].Visible = true;
                        }
                    }
                }

                if (this.fourthBot.Chips <= 0)
                {
                    this.fourthBot.OutOfChips = true;
                    this.cardsPictureBoxList[8].Visible = false;
                    this.cardsPictureBoxList[9].Visible = false;
                }
                else
                {
                    this.fourthBot.OutOfChips = false;
                    if (i == 9)
                    {
                        if (this.cardsPictureBoxList[9] != null)
                        {
                            this.cardsPictureBoxList[8].Visible = true;
                            this.cardsPictureBoxList[9].Visible = true;
                        }
                    }
                }

                if (this.fifthBot.Chips <= 0)
                {
                    this.fifthBot.OutOfChips = true;
                    this.cardsPictureBoxList[10].Visible = false;
                    this.cardsPictureBoxList[11].Visible = false;
                }
                else
                {
                    this.fifthBot.OutOfChips = false;
                    if (i == 11)
                    {
                        if (this.cardsPictureBoxList[11] != null)
                        {
                            this.cardsPictureBoxList[10].Visible = true;
                            this.cardsPictureBoxList[11].Visible = true;
                        }
                    }
                }

                if (i == 16)
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
                bRaise.Enabled = true;
                bCall.Enabled = true;
                bRaise.Enabled = true;
                bRaise.Enabled = true;
                bFold.Enabled = true;
            }
            #endregion
        }

        async Task Turns()
        {
            #region Rotating
            if (!this.human.OutOfChips)
            {
                if (this.human.CanMakeTurn)
                {
                    FixCall(this.humanStatus, this.human.Call, this.human.Raise, 1);
                    //MessageBox.Show("Player's Turn");
                    pbTimer.Visible = true;
                    pbTimer.Value = 1000;
                    t = 60;
                    up = 10000000;
                    timer.Start();
                    bRaise.Enabled = true;
                    bCall.Enabled = true;
                    bRaise.Enabled = true;
                    bRaise.Enabled = true;
                    bFold.Enabled = true;
                    turnCount++;
                    FixCall(this.humanStatus, this.human.Call, this.human.Raise, 2);
                }
            }

            if (this.human.OutOfChips || !this.human.CanMakeTurn)
            {
                await AllIn();
                if (this.human.OutOfChips && !this.human.Folded)
                {
                    if (bCall.Text.Contains("All in") == false || bRaise.Text.Contains("All in") == false)
                    {
                        this.playersGameStatus.RemoveAt(0);
                        this.playersGameStatus.Insert(0, null);
                        maxLeft--;
                        this.human.Folded = true;
                    }
                }

                await CheckRaise(0, 0);
                pbTimer.Visible = false;
                bRaise.Enabled = false;
                bCall.Enabled = false;
                bRaise.Enabled = false;
                bRaise.Enabled = false;
                bFold.Enabled = false;
                timer.Stop();
                this.firstBot.CanMakeTurn = true;
                if (!this.firstBot.OutOfChips)
                {
                    if (this.firstBot.CanMakeTurn)
                    {
                        FixCall(this.firstBotStatus, this.firstBot.Call, this.firstBot.Raise, 1);
                        FixCall(this.firstBotStatus, this.firstBot.Call, this.firstBot.Raise, 2);
                        Rules(2, 3, "Bot 1", this.firstBot.Type, this.firstBot.Power, this.firstBot.OutOfChips);
                        MessageBox.Show("Bot 1's Turn");
                        this.AI(2, 3, this.firstBotStatus, 0, this.firstBot);
                        turnCount++;
                        last = 1;
                        this.firstBot.CanMakeTurn = false;
                        this.secondBot.CanMakeTurn = true;
                    }
                }

                if (this.firstBot.OutOfChips && !this.firstBot.Folded)
                {
                    this.playersGameStatus.RemoveAt(1);
                    this.playersGameStatus.Insert(1, null);
                    maxLeft--;
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
                        Rules(4, 5, "Bot 2", this.secondBot.Type, this.secondBot.Power, this.secondBot.OutOfChips);
                        MessageBox.Show("Bot 2's Turn");
                        this.AI(4, 5, this.secondBotStatus, 1, this.secondBot);
                        turnCount++;
                        last = 2;
                        this.secondBot.CanMakeTurn = false;
                        this.thirdBot.CanMakeTurn = true;
                    }
                }

                if (this.secondBot.OutOfChips && !this.secondBot.Folded)
                {
                    this.playersGameStatus.RemoveAt(2);
                    this.playersGameStatus.Insert(2, null);
                    maxLeft--;
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
                        Rules(6, 7, "Bot 3", this.thirdBot.Type, this.thirdBot.Power, this.thirdBot.OutOfChips);
                        MessageBox.Show("Bot 3's Turn");
                        this.AI(6, 7, this.thirdBotStatus, 2, this.thirdBot);
                        turnCount++;
                        last = 3;
                        this.thirdBot.CanMakeTurn = false;
                        this.fourthBot.CanMakeTurn = true;
                    }
                }

                if (this.thirdBot.OutOfChips && !this.thirdBot.Folded)
                {
                    this.playersGameStatus.RemoveAt(3);
                    this.playersGameStatus.Insert(3, null);
                    maxLeft--;
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
                        Rules(8, 9, "Bot 4", this.fourthBot.Type, this.fourthBot.Power, this.fourthBot.OutOfChips);
                        MessageBox.Show("Bot 4's Turn");
                        this.AI(8, 9, this.fourthBotStatus, 3, this.fourthBot);
                        turnCount++;
                        last = 4;
                        this.fourthBot.CanMakeTurn = false;
                        this.fifthBot.CanMakeTurn = true;
                    }
                }

                if (this.fourthBot.OutOfChips && !this.fourthBot.Folded)
                {
                    this.playersGameStatus.RemoveAt(4);
                    this.playersGameStatus.Insert(4, null);
                    maxLeft--;
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
                        Rules(10, 11, "Bot 5", this.fifthBot.Type, this.fifthBot.Power, this.fifthBot.OutOfChips);
                        MessageBox.Show("Bot 5's Turn");
                        this.AI(10, 11, this.fifthBotStatus, 4, this.fifthBot);
                        turnCount++;
                        last = 5;
                        this.fifthBot.CanMakeTurn = false;
                    }
                }

                if (this.fifthBot.OutOfChips && !this.fifthBot.Folded)
                {
                    this.playersGameStatus.RemoveAt(5);
                    this.playersGameStatus.Insert(5, null);
                    maxLeft--;
                    this.fifthBot.Folded = true;
                }

                if (this.fifthBot.OutOfChips || !this.fifthBot.CanMakeTurn)
                {
                    await CheckRaise(5, 5);
                    this.human.CanMakeTurn = true;
                }

                if (this.human.OutOfChips && !this.human.Folded)
                {
                    if (bCall.Text.Contains("All in") == false || bRaise.Text.Contains("All in") == false)
                    {
                        this.playersGameStatus.RemoveAt(0);
                        this.playersGameStatus.Insert(0, null);
                        maxLeft--;
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

        void Rules(
            // TODO: validate for real var name: Previous names c1 and c2, renamed to cardOne and CardTwo
            int cardOne,
            int cardTwo,
            string currentText,
            double current,
            double Power,
            bool foldedTurn)
        {
            if (cardOne == 0 && cardTwo == 1)
            {
            }

            if (!foldedTurn || cardOne == 0 && cardTwo == 1 && this.humanStatus.Text.Contains("Fold") == false)
            {
                #region Variables
                bool done = false, vf = false;
                int[] Straight1 = new int[5];

                //TODO: Should Straight be an Enum?
                int[] Straight = new int[7];
                Straight[0] = this.reservedGameCardsIndeces[cardOne];
                Straight[1] = this.reservedGameCardsIndeces[cardTwo];
                Straight1[0] = Straight[2] = this.reservedGameCardsIndeces[12];
                Straight1[1] = Straight[3] = this.reservedGameCardsIndeces[13];
                Straight1[2] = Straight[4] = this.reservedGameCardsIndeces[14];
                Straight1[3] = Straight[5] = this.reservedGameCardsIndeces[15];
                Straight1[4] = Straight[6] = this.reservedGameCardsIndeces[16];
                var a = Straight.Where(o => o % 4 == 0).ToArray();
                var b = Straight.Where(o => o % 4 == 1).ToArray();
                var c = Straight.Where(o => o % 4 == 2).ToArray();
                var d = Straight.Where(o => o % 4 == 3).ToArray();
                var st1 = a.Select(o => o / 4).Distinct().ToArray();
                var st2 = b.Select(o => o / 4).Distinct().ToArray();
                var st3 = c.Select(o => o / 4).Distinct().ToArray();
                var st4 = d.Select(o => o / 4).Distinct().ToArray();
                Array.Sort(Straight);
                Array.Sort(st1);
                Array.Sort(st2);
                Array.Sort(st3);
                Array.Sort(st4);
                #endregion

                for (i = 0; i < 16; i++)
                {
                    if (this.reservedGameCardsIndeces[i] == int.Parse(this.cardsPictureBoxList[cardOne].Tag.ToString()) &&
                        this.reservedGameCardsIndeces[i + 1] == int.Parse(this.cardsPictureBoxList[cardTwo].Tag.ToString()))
                    {
                        //Pair from Hand current = 1

                        rPairFromHand(ref current, ref Power);

                        #region Pair or Two Pair from Table current = 2 || 0
                        rPairTwoPair(ref current, ref Power);
                        #endregion

                        #region Two Pair current = 2
                        rTwoPair(ref current, ref Power);
                        #endregion

                        #region Three of a kind current = 3
                        rThreeOfAKind(ref current, ref Power, Straight);
                        #endregion

                        #region Straight current = 4
                        rStraight(ref current, ref Power, Straight);
                        #endregion

                        #region Flush current = 5 || 5.5
                        rFlush(ref current, ref Power, ref vf, Straight1);
                        #endregion

                        #region Full House current = 6
                        rFullHouse(ref current, ref Power, ref done, Straight);
                        #endregion

                        #region Four of a Kind current = 7
                        rFourOfAKind(ref current, ref Power, Straight);
                        #endregion

                        #region Straight Flush current = 8 || 9
                        rStraightFlush(ref current, ref Power, st1, st2, st3, st4);
                        #endregion

                        #region High Card current = -1
                        rHighCard(ref current, ref Power);
                        #endregion
                    }
                }
            }
        }

        private void rStraightFlush(ref double current, ref double Power, int[] st1, int[] st2, int[] st3, int[] st4)
        {
            if (current >= -1)
            {
                if (st1.Length >= 5)
                {
                    if (st1[0] + 4 == st1[4])
                    {
                        current = 8;
                        Power = (st1.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 8 });
                        sorted = Win
                            .OrderByDescending(op1 => op1.Current)
                            .ThenByDescending(op1 => op1.Power).First();
                    }

                    if (st1[0] == 0 && st1[1] == 9 && st1[2] == 10 && st1[3] == 11 && st1[0] + 12 == st1[4])
                    {
                        current = 9;
                        Power = (st1.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 9 });
                        sorted = Win
                            .OrderByDescending(op1 => op1.Current)
                            .ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (st2.Length >= 5)
                {
                    if (st2[0] + 4 == st2[4])
                    {
                        current = 8;
                        Power = (st2.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 8 });
                        sorted = Win
                            .OrderByDescending(op1 => op1.Current)
                            .ThenByDescending(op1 => op1.Power)
                            .First();
                    }

                    if (st2[0] == 0 && st2[1] == 9 && st2[2] == 10 && st2[3] == 11 && st2[0] + 12 == st2[4])
                    {
                        current = 9;
                        Power = (st2.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 9 });
                        sorted = Win
                            .OrderByDescending(op1 => op1.Current)
                            .ThenByDescending(op1 => op1.Power)
                            .First();
                    }
                }

                if (st3.Length >= 5)
                {
                    if (st3[0] + 4 == st3[4])
                    {
                        current = 8;
                        Power = (st3.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 8 });
                        sorted = Win
                            .OrderByDescending(op1 => op1.Current)
                            .ThenByDescending(op1 => op1.Power)
                            .First();
                    }

                    if (st3[0] == 0 &&
                        st3[1] == 9 &&
                        st3[2] == 10 &&
                        st3[3] == 11 &&
                        st3[0] + 12 == st3[4])
                    {
                        current = 9;
                        Power = (st3.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 9 });
                        sorted = Win
                            .OrderByDescending(op1 => op1.Current)
                            .ThenByDescending(op1 => op1.Power)
                            .First();
                    }
                }

                if (st4.Length >= 5)
                {
                    if (st4[0] + 4 == st4[4])
                    {
                        current = 8;
                        Power = (st4.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 8 });
                        sorted = Win
                            .OrderByDescending(op1 => op1.Current)
                            .ThenByDescending(op1 => op1.Power)
                            .First();
                    }

                    if (st4[0] == 0 &&
                        st4[1] == 9 &&
                        st4[2] == 10 &&
                        st4[3] == 11 &&
                        st4[0] + 12 == st4[4])
                    {
                        current = 9;
                        Power = (st4.Max()) / 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 9 });
                        sorted = Win
                            .OrderByDescending(op1 => op1.Current)
                            .ThenByDescending(op1 => op1.Power)
                            .First();
                    }
                }
            }
        }

        private void rFourOfAKind(ref double current, ref double Power, int[] Straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (Straight[j] / 4 == Straight[j + 1] / 4 &&
                        Straight[j] / 4 == Straight[j + 2] / 4 &&
                        Straight[j] / 4 == Straight[j + 3] / 4)
                    {
                        current = 7;
                        Power = (Straight[j] / 4) * 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 7 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (Straight[j] / 4 == 0 && Straight[j + 1] / 4 == 0 && Straight[j + 2] / 4 == 0 && Straight[j + 3] / 4 == 0)
                    {
                        current = 7;
                        Power = 13 * 4 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 7 });
                        sorted = Win
                            .OrderByDescending(op1 => op1.Current)
                            .ThenByDescending(op1 => op1.Power)
                            .First();
                    }
                }
            }
        }

        private void rFullHouse(ref double current, ref double Power, ref bool done, int[] Straight)
        {
            if (current >= -1)
            {
                type = Power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                current = 6;
                                Power = 13 * 2 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 6 });
                                sorted = Win
                                    .OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                                break;
                            }

                            if (fh.Max() / 4 > 0)
                            {
                                current = 6;
                                Power = fh.Max() / 4 * 2 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 6 });
                                sorted = Win
                                    .OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                                break;
                            }
                        }

                        if (!done)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                Power = 13;
                                done = true;
                                j = -1;
                            }
                            else
                            {
                                Power = fh.Max() / 4;
                                done = true;
                                j = -1;
                            }
                        }
                    }
                }

                if (current != 6)
                {
                    Power = type;
                }
            }
        }

        private void rFlush(ref double current, ref double Power, ref bool vf, int[] Straight1)
        {
            if (current >= -1)
            {
                var f1 = Straight1.Where(o => o % 4 == 0).ToArray();
                var f2 = Straight1.Where(o => o % 4 == 1).ToArray();
                var f3 = Straight1.Where(o => o % 4 == 2).ToArray();
                var f4 = Straight1.Where(o => o % 4 == 3).ToArray();
                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (this.reservedGameCardsIndeces[i] % 4 == this.reservedGameCardsIndeces[i + 1] % 4 && this.reservedGameCardsIndeces[i] % 4 == f1[0] % 4)
                    {
                        if (this.reservedGameCardsIndeces[i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win
                                .OrderByDescending(op1 => op1.Current)
                                .ThenByDescending(op1 => op1.Power)
                                .First();
                            vf = true;
                        }

                        if (this.reservedGameCardsIndeces[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win
                                .OrderByDescending(op1 => op1.Current)
                                .ThenByDescending(op1 => op1.Power)
                                .First();
                            vf = true;
                        }
                        else if (this.reservedGameCardsIndeces[i] / 4 < f1.Max() / 4 && this.reservedGameCardsIndeces[i + 1] / 4 < f1.Max() / 4)
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win
                                .OrderByDescending(op1 => op1.Current)
                                .ThenByDescending(op1 => op1.Power)
                                .First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 4)//different cards in hand
                {
                    if (this.reservedGameCardsIndeces[i] % 4 != this.reservedGameCardsIndeces[i + 1] % 4 && this.reservedGameCardsIndeces[i] % 4 == f1[0] % 4)
                    {
                        if (this.reservedGameCardsIndeces[i] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win
                                .OrderByDescending(op1 => op1.Current)
                                .ThenByDescending(op1 => op1.Power)
                                .First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (this.reservedGameCardsIndeces[i + 1] % 4 != this.reservedGameCardsIndeces[i] % 4 && this.reservedGameCardsIndeces[i + 1] % 4 == f1[0] % 4)
                    {
                        if (this.reservedGameCardsIndeces[i + 1] / 4 > f1.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win
                                .OrderByDescending(op1 => op1.Current)
                                .ThenByDescending(op1 => op1.Power)
                                .First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f1.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win
                                .OrderByDescending(op1 => op1.Current)
                                .ThenByDescending(op1 => op1.Power)
                                .First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 5)
                {
                    if (this.reservedGameCardsIndeces[i] % 4 == f1[0] % 4 && this.reservedGameCardsIndeces[i] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        Power = this.reservedGameCardsIndeces[i] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (this.reservedGameCardsIndeces[i + 1] % 4 == f1[0] % 4 && this.reservedGameCardsIndeces[i + 1] / 4 > f1.Min() / 4)
                    {
                        current = 5;
                        Power = this.reservedGameCardsIndeces[i + 1] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.reservedGameCardsIndeces[i] / 4 < f1.Min() / 4 && this.reservedGameCardsIndeces[i + 1] / 4 < f1.Min())
                    {
                        current = 5;
                        Power = f1.Max() + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (this.reservedGameCardsIndeces[i] % 4 == this.reservedGameCardsIndeces[i + 1] % 4 && this.reservedGameCardsIndeces[i] % 4 == f2[0] % 4)
                    {
                        if (this.reservedGameCardsIndeces[i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (this.reservedGameCardsIndeces[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.reservedGameCardsIndeces[i] / 4 < f2.Max() / 4 && this.reservedGameCardsIndeces[i + 1] / 4 < f2.Max() / 4)
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 4)//different cards in hand
                {
                    if (this.reservedGameCardsIndeces[i] % 4 != this.reservedGameCardsIndeces[i + 1] % 4 && this.reservedGameCardsIndeces[i] % 4 == f2[0] % 4)
                    {
                        if (this.reservedGameCardsIndeces[i] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (this.reservedGameCardsIndeces[i + 1] % 4 != this.reservedGameCardsIndeces[i] % 4 && this.reservedGameCardsIndeces[i + 1] % 4 == f2[0] % 4)
                    {
                        if (this.reservedGameCardsIndeces[i + 1] / 4 > f2.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f2.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f2.Length == 5)
                {
                    if (this.reservedGameCardsIndeces[i] % 4 == f2[0] % 4 && this.reservedGameCardsIndeces[i] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        Power = this.reservedGameCardsIndeces[i] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (this.reservedGameCardsIndeces[i + 1] % 4 == f2[0] % 4 && this.reservedGameCardsIndeces[i + 1] / 4 > f2.Min() / 4)
                    {
                        current = 5;
                        Power = this.reservedGameCardsIndeces[i + 1] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.reservedGameCardsIndeces[i] / 4 < f2.Min() / 4 && this.reservedGameCardsIndeces[i + 1] / 4 < f2.Min())
                    {
                        current = 5;
                        Power = f2.Max() + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (this.reservedGameCardsIndeces[i] % 4 == this.reservedGameCardsIndeces[i + 1] % 4 && this.reservedGameCardsIndeces[i] % 4 == f3[0] % 4)
                    {
                        if (this.reservedGameCardsIndeces[i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (this.reservedGameCardsIndeces[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.reservedGameCardsIndeces[i] / 4 < f3.Max() / 4 && this.reservedGameCardsIndeces[i + 1] / 4 < f3.Max() / 4)
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 4)//different cards in hand
                {
                    if (this.reservedGameCardsIndeces[i] % 4 != this.reservedGameCardsIndeces[i + 1] % 4 && this.reservedGameCardsIndeces[i] % 4 == f3[0] % 4)
                    {
                        if (this.reservedGameCardsIndeces[i] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (this.reservedGameCardsIndeces[i + 1] % 4 != this.reservedGameCardsIndeces[i] % 4 && this.reservedGameCardsIndeces[i + 1] % 4 == f3[0] % 4)
                    {
                        if (this.reservedGameCardsIndeces[i + 1] / 4 > f3.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f3.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f3.Length == 5)
                {
                    if (this.reservedGameCardsIndeces[i] % 4 == f3[0] % 4 && this.reservedGameCardsIndeces[i] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        Power = this.reservedGameCardsIndeces[i] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (this.reservedGameCardsIndeces[i + 1] % 4 == f3[0] % 4 && this.reservedGameCardsIndeces[i + 1] / 4 > f3.Min() / 4)
                    {
                        current = 5;
                        Power = this.reservedGameCardsIndeces[i + 1] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.reservedGameCardsIndeces[i] / 4 < f3.Min() / 4 && this.reservedGameCardsIndeces[i + 1] / 4 < f3.Min())
                    {
                        current = 5;
                        Power = f3.Max() + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (this.reservedGameCardsIndeces[i] % 4 == this.reservedGameCardsIndeces[i + 1] % 4 && this.reservedGameCardsIndeces[i] % 4 == f4[0] % 4)
                    {
                        if (this.reservedGameCardsIndeces[i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        if (this.reservedGameCardsIndeces[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (this.reservedGameCardsIndeces[i] / 4 < f4.Max() / 4 && this.reservedGameCardsIndeces[i + 1] / 4 < f4.Max() / 4)
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 4)//different cards in hand
                {
                    if (this.reservedGameCardsIndeces[i] % 4 != this.reservedGameCardsIndeces[i + 1] % 4 && this.reservedGameCardsIndeces[i] % 4 == f4[0] % 4)
                    {
                        if (this.reservedGameCardsIndeces[i] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                    if (this.reservedGameCardsIndeces[i + 1] % 4 != this.reservedGameCardsIndeces[i] % 4 && this.reservedGameCardsIndeces[i + 1] % 4 == f4[0] % 4)
                    {
                        if (this.reservedGameCardsIndeces[i + 1] / 4 > f4.Max() / 4)
                        {
                            current = 5;
                            Power = this.reservedGameCardsIndeces[i + 1] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            current = 5;
                            Power = f4.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 5 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }
                if (f4.Length == 5)
                {
                    if (this.reservedGameCardsIndeces[i] % 4 == f4[0] % 4 && this.reservedGameCardsIndeces[i] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        Power = this.reservedGameCardsIndeces[i] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    if (this.reservedGameCardsIndeces[i + 1] % 4 == f4[0] % 4 && this.reservedGameCardsIndeces[i + 1] / 4 > f4.Min() / 4)
                    {
                        current = 5;
                        Power = this.reservedGameCardsIndeces[i + 1] + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (this.reservedGameCardsIndeces[i] / 4 < f4.Min() / 4 && this.reservedGameCardsIndeces[i + 1] / 4 < f4.Min())
                    {
                        current = 5;
                        Power = f4.Max() + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }
                //ace
                if (f1.Length > 0)
                {
                    if (this.reservedGameCardsIndeces[i] / 4 == 0 && this.reservedGameCardsIndeces[i] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (this.reservedGameCardsIndeces[i + 1] / 4 == 0 && this.reservedGameCardsIndeces[i + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f2.Length > 0)
                {
                    if (this.reservedGameCardsIndeces[i] / 4 == 0 && this.reservedGameCardsIndeces[i] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (this.reservedGameCardsIndeces[i + 1] / 4 == 0 && this.reservedGameCardsIndeces[i + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f3.Length > 0)
                {
                    if (this.reservedGameCardsIndeces[i] / 4 == 0 && this.reservedGameCardsIndeces[i] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (this.reservedGameCardsIndeces[i + 1] / 4 == 0 && this.reservedGameCardsIndeces[i + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
                if (f4.Length > 0)
                {
                    if (this.reservedGameCardsIndeces[i] / 4 == 0 && this.reservedGameCardsIndeces[i] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                    if (this.reservedGameCardsIndeces[i + 1] / 4 == 0 && this.reservedGameCardsIndeces[i + 1] % 4 == f4[0] % 4 && vf)
                    {
                        current = 5.5;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 5.5 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void rStraight(ref double current, ref double Power, int[] Straight)
        {
            if (current >= -1)
            {
                var op = Straight.Select(o => o / 4).Distinct().ToArray();
                for (int j = 0; j < op.Length - 4; j++)
                {
                    if (op[j] + 4 == op[j + 4])
                    {
                        if (op.Max() - 4 == op[j])
                        {
                            current = 4;
                            Power = op.Max() + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 4 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            current = 4;
                            Power = op[j + 4] + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 4 });
                            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }
                    if (op[j] == 0 && op[j + 1] == 9 && op[j + 2] == 10 && op[j + 3] == 11 && op[j + 4] == 12)
                    {
                        current = 4;
                        Power = 13 + current * 100;
                        Win.Add(new Type() { Power = Power, Current = 4 });
                        sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        private void rThreeOfAKind(ref double current, ref double Power, int[] Straight)
        {
            if (current >= -1)
            {
                for (int j = 0; j <= 12; j++)
                {
                    var fh = Straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            current = 3;
                            Power = 13 * 3 + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 3 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 3;
                            Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 3 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }

        private void rTwoPair(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (this.reservedGameCardsIndeces[i] / 4 != this.reservedGameCardsIndeces[i + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }
                            if (tc - k >= 12)
                            {
                                if (this.reservedGameCardsIndeces[i] / 4 == this.reservedGameCardsIndeces[tc] / 4 && this.reservedGameCardsIndeces[i + 1] / 4 == this.reservedGameCardsIndeces[tc - k] / 4 ||
                                    this.reservedGameCardsIndeces[i + 1] / 4 == this.reservedGameCardsIndeces[tc] / 4 && this.reservedGameCardsIndeces[i] / 4 == this.reservedGameCardsIndeces[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (this.reservedGameCardsIndeces[i] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = 13 * 4 + (this.reservedGameCardsIndeces[i + 1] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (this.reservedGameCardsIndeces[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = 13 * 4 + (this.reservedGameCardsIndeces[i] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (this.reservedGameCardsIndeces[i + 1] / 4 != 0 && this.reservedGameCardsIndeces[i] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (this.reservedGameCardsIndeces[i] / 4) * 2 + (this.reservedGameCardsIndeces[i + 1] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }
                                    msgbox = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void rPairTwoPair(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                bool msgbox1 = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    for (int k = 1; k <= max; k++)
                    {
                        if (tc - k < 12)
                        {
                            max--;
                        }
                        if (tc - k >= 12)
                        {
                            if (this.reservedGameCardsIndeces[tc] / 4 == this.reservedGameCardsIndeces[tc - k] / 4)
                            {
                                if (this.reservedGameCardsIndeces[tc] / 4 != this.reservedGameCardsIndeces[i] / 4 && this.reservedGameCardsIndeces[tc] / 4 != this.reservedGameCardsIndeces[i + 1] / 4 && current == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (this.reservedGameCardsIndeces[i + 1] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = (this.reservedGameCardsIndeces[i] / 4) * 2 + 13 * 4 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (this.reservedGameCardsIndeces[i] / 4 == 0)
                                        {
                                            current = 2;
                                            Power = (this.reservedGameCardsIndeces[i + 1] / 4) * 2 + 13 * 4 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (this.reservedGameCardsIndeces[i + 1] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (this.reservedGameCardsIndeces[tc] / 4) * 2 + (this.reservedGameCardsIndeces[i + 1] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                        if (this.reservedGameCardsIndeces[i] / 4 != 0)
                                        {
                                            current = 2;
                                            Power = (this.reservedGameCardsIndeces[tc] / 4) * 2 + (this.reservedGameCardsIndeces[i] / 4) * 2 + current * 100;
                                            Win.Add(new Type() { Power = Power, Current = 2 });
                                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }
                                    msgbox = true;
                                }
                                if (current == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (this.reservedGameCardsIndeces[i] / 4 > this.reservedGameCardsIndeces[i + 1] / 4)
                                        {
                                            if (this.reservedGameCardsIndeces[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                Power = 13 + this.reservedGameCardsIndeces[i] / 4 + current * 100;
                                                Win.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                Power = this.reservedGameCardsIndeces[tc] / 4 + this.reservedGameCardsIndeces[i] / 4 + current * 100;
                                                Win.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (this.reservedGameCardsIndeces[tc] / 4 == 0)
                                            {
                                                current = 0;
                                                Power = 13 + this.reservedGameCardsIndeces[i + 1] + current * 100;
                                                Win.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                current = 0;
                                                Power = this.reservedGameCardsIndeces[tc] / 4 + this.reservedGameCardsIndeces[i + 1] / 4 + current * 100;
                                                Win.Add(new Type() { Power = Power, Current = 1 });
                                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                    }
                                    msgbox1 = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void rPairFromHand(ref double current, ref double Power)
        {
            if (current >= -1)
            {
                bool msgbox = false;
                if (this.reservedGameCardsIndeces[i] / 4 == this.reservedGameCardsIndeces[i + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (this.reservedGameCardsIndeces[i] / 4 == 0)
                        {
                            current = 1;
                            Power = 13 * 4 + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 1 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            current = 1;
                            Power = (this.reservedGameCardsIndeces[i + 1] / 4) * 4 + current * 100;
                            Win.Add(new Type() { Power = Power, Current = 1 });
                            sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                    msgbox = true;
                }
                for (int tc = 16; tc >= 12; tc--)
                {
                    if (this.reservedGameCardsIndeces[i + 1] / 4 == this.reservedGameCardsIndeces[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (this.reservedGameCardsIndeces[i + 1] / 4 == 0)
                            {
                                current = 1;
                                Power = 13 * 4 + this.reservedGameCardsIndeces[i] / 4 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                Power = (this.reservedGameCardsIndeces[i + 1] / 4) * 4 + this.reservedGameCardsIndeces[i] / 4 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                    if (this.reservedGameCardsIndeces[i] / 4 == this.reservedGameCardsIndeces[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (this.reservedGameCardsIndeces[i] / 4 == 0)
                            {
                                current = 1;
                                Power = 13 * 4 + this.reservedGameCardsIndeces[i + 1] / 4 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                current = 1;
                                Power = (this.reservedGameCardsIndeces[tc] / 4) * 4 + this.reservedGameCardsIndeces[i + 1] / 4 + current * 100;
                                Win.Add(new Type() { Power = Power, Current = 1 });
                                sorted = Win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                }
            }
        }

        private void rHighCard(ref double current, ref double Power)
        {
            if (current == -1)
            {
                if (this.reservedGameCardsIndeces[i] / 4 > this.reservedGameCardsIndeces[i + 1] / 4)
                {
                    current = -1;
                    Power = this.reservedGameCardsIndeces[i] / 4;
                    Win.Add(new Type() { Power = Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    current = -1;
                    Power = this.reservedGameCardsIndeces[i + 1] / 4;
                    Win.Add(new Type() { Power = Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                if (this.reservedGameCardsIndeces[i] / 4 == 0 || this.reservedGameCardsIndeces[i + 1] / 4 == 0)
                {
                    current = -1;
                    Power = 13;
                    Win.Add(new Type() { Power = Power, Current = -1 });
                    sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }

        void Winner(double current, double Power, string currentText, int chips, string lastly)
        {
            if (lastly == " ")
            {
                lastly = "Bot 5";
            }
            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (this.cardsPictureBoxList[j].Visible)
                    this.cardsPictureBoxList[j].Image = this.deskCardsAsImages[j];
            }
            if (current == sorted.Current)
            {
                if (Power == sorted.Power)
                {
                    winners++;
                    CheckWinners.Add(currentText);
                    if (current == -1)
                    {
                        MessageBox.Show(currentText + " High Card ");
                    }
                    if (current == 1 || current == 0)
                    {
                        MessageBox.Show(currentText + " Pair ");
                    }
                    if (current == 2)
                    {
                        MessageBox.Show(currentText + " Two Pair ");
                    }
                    if (current == 3)
                    {
                        MessageBox.Show(currentText + " Three of a Kind ");
                    }
                    if (current == 4)
                    {
                        MessageBox.Show(currentText + " Straight ");
                    }
                    if (current == 5 || current == 5.5)
                    {
                        MessageBox.Show(currentText + " Flush ");
                    }
                    if (current == 6)
                    {
                        MessageBox.Show(currentText + " Full House ");
                    }
                    if (current == 7)
                    {
                        MessageBox.Show(currentText + " Four of a Kind ");
                    }
                    if (current == 8)
                    {
                        MessageBox.Show(currentText + " Straight Flush ");
                    }
                    if (current == 9)
                    {
                        MessageBox.Show(currentText + " Royal Flush ! ");
                    }
                }
            }
            if (currentText == lastly)//lastfixed
            {
                if (winners > 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        this.human.Chips += int.Parse(this.potStatus.Text) / winners;
                        tbChips.Text = this.human.Chips.ToString();
                        //playerPanel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        this.firstBot.Chips += int.Parse(this.potStatus.Text) / winners;
                        tbBotChips1.Text = this.firstBot.Chips.ToString();
                        //bot1Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        this.secondBot.Chips += int.Parse(this.potStatus.Text) / winners;
                        tbBotChips2.Text = this.secondBot.Chips.ToString();
                        //bot2Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        this.thirdBot.Chips += int.Parse(this.potStatus.Text) / winners;
                        tbBotChips3.Text = this.thirdBot.Chips.ToString();
                        //bot3Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        this.fourthBot.Chips += int.Parse(this.potStatus.Text) / winners;
                        tbBotChips4.Text = this.fourthBot.Chips.ToString();
                        //bot4Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        this.fifthBot.Chips += int.Parse(this.potStatus.Text) / winners;
                        tbBotChips5.Text = this.fifthBot.Chips.ToString();
                        //bot5Panel.Visible = true;
                    }
                    //await Finish(1);
                }
                if (winners == 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        this.human.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //playerPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        this.firstBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot1Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        this.secondBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot2Panel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        this.thirdBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot3Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        this.fourthBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot4Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        this.fifthBot.Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot5Panel.Visible = true;
                    }
                }
            }
        }

        async Task CheckRaise(int currentTurn, int raiseTurn)
        {
            if (raising)
            {
                turnCount = 0;
                raising = false;
                raisedTurn = currentTurn;
                changed = true;
            }
            else
            {
                if (turnCount >= maxLeft - 1 || !changed && turnCount == maxLeft)
                {
                    if (currentTurn == raisedTurn - 1 || !changed && turnCount == maxLeft || raisedTurn == 0 && currentTurn == 5)
                    {
                        changed = false;
                        turnCount = 0;
                        this.raise = 0;
                        this.neededChipsToCall = 0;
                        raisedTurn = 123;
                        rounds++;
                        if (!this.human.OutOfChips)
                            this.humanStatus.Text = "";
                        if (!this.firstBot.OutOfChips)
                            this.firstBotStatus.Text = "";
                        if (!this.secondBot.OutOfChips)
                            this.secondBotStatus.Text = "";
                        if (!this.thirdBot.OutOfChips)
                            this.thirdBotStatus.Text = "";
                        if (!this.fourthBot.OutOfChips)
                            this.fourthBotStatus.Text = "";
                        if (!this.fifthBot.OutOfChips)
                            this.fifthBotStatus.Text = "";
                    }
                }
            }
            if (rounds == Flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (this.cardsPictureBoxList[j].Image != this.deskCardsAsImages[j])
                    {
                        this.cardsPictureBoxList[j].Image = this.deskCardsAsImages[j];
                        this.human.Call = 0; this.human.Raise = 0;
                        this.firstBot.Call = 0; this.firstBot.Raise = 0;
                        this.secondBot.Call = 0; this.secondBot.Raise = 0;
                        this.thirdBot.Call = 0; this.thirdBot.Raise = 0;
                        this.fourthBot.Call = 0; this.fourthBot.Raise = 0;
                        this.fifthBot.Call = 0; this.fifthBot.Raise = 0;
                    }
                }
            }
            if (rounds == Turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (this.cardsPictureBoxList[j].Image != this.deskCardsAsImages[j])
                    {
                        this.cardsPictureBoxList[j].Image = this.deskCardsAsImages[j];
                        this.human.Call = 0; this.human.Raise = 0;
                        this.firstBot.Call = 0; this.firstBot.Raise = 0;
                        this.secondBot.Call = 0; this.secondBot.Raise = 0;
                        this.thirdBot.Call = 0; this.thirdBot.Raise = 0;
                        this.fourthBot.Call = 0; this.fourthBot.Raise = 0;
                        this.fifthBot.Call = 0; this.fifthBot.Raise = 0;
                    }
                }
            }
            if (rounds == River)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (this.cardsPictureBoxList[j].Image != this.deskCardsAsImages[j])
                    {
                        this.cardsPictureBoxList[j].Image = this.deskCardsAsImages[j];
                        this.human.Call = 0; this.human.Raise = 0;
                        this.firstBot.Call = 0; this.firstBot.Raise = 0;
                        this.secondBot.Call = 0; this.secondBot.Raise = 0;
                        this.thirdBot.Call = 0; this.thirdBot.Raise = 0;
                        this.fourthBot.Call = 0; this.fourthBot.Raise = 0;
                        this.fifthBot.Call = 0; this.fifthBot.Raise = 0;
                    }
                }
            }
            if (rounds == End && maxLeft == 6)
            {
                string fixedLast = "qwerty";
                if (!this.humanStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, "Player", this.human.Type, this.human.Power, this.human.OutOfChips);
                }
                if (!this.firstBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(2, 3, "Bot 1", this.firstBot.Type, this.firstBot.Power, this.firstBot.OutOfChips);
                }
                if (!this.secondBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(4, 5, "Bot 2", this.thirdBot.Type, this.secondBot.Power, this.secondBot.OutOfChips);
                }
                if (!this.thirdBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(6, 7, "Bot 3", this.thirdBot.Type, this.thirdBot.Power, this.thirdBot.OutOfChips);
                }
                if (!this.fourthBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(8, 9, "Bot 4", this.fourthBot.Type, this.fourthBot.Power, this.fourthBot.OutOfChips);
                }
                if (!this.fifthBotStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(10, 11, "Bot 5", this.fifthBot.Type, this.fourthBot.Power, this.fifthBot.OutOfChips);
                }

                //TODO: COnsider passing the entire player object
                //TODO: COde repetition
                Winner(this.human.Type, this.human.Power, "Player", this.human.Chips, fixedLast);
                Winner(this.firstBot.Type, this.firstBot.Power, "Bot 1", this.firstBot.Chips, fixedLast);
                Winner(this.secondBot.Type, this.secondBot.Power, "Bot 2", this.secondBot.Chips, fixedLast);
                Winner(this.thirdBot.Type, this.thirdBot.Power, "Bot 3", this.thirdBot.Chips, fixedLast);
                Winner(this.fourthBot.Type, this.fourthBot.Power, "Bot 4", this.fourthBot.Chips, fixedLast);
                Winner(this.fifthBot.Type, this.fifthBot.Power, "Bot 5", this.fifthBot.Chips, fixedLast);
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
                    AddChips f2 = new AddChips();
                    f2.ShowDialog();
                    if (f2.a != 0)
                    {
                        this.human.Chips = f2.a;
                        this.firstBot.Chips += f2.a;
                        this.secondBot.Chips += f2.a;
                        this.thirdBot.Chips += f2.a;
                        this.fourthBot.Chips += f2.a;
                        this.fifthBot.Chips += f2.a;
                        this.human.OutOfChips = false;
                        this.human.CanMakeTurn = true;
                        bRaise.Enabled = true;
                        bFold.Enabled = true;
                        bCheck.Enabled = true;
                        bRaise.Text = "raise";
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
                this.playersGameStatus.Clear();
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

                this.chips.Clear();
                this.CheckWinners.Clear();
                this.winners = 0;
                this.Win.Clear();
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

        void FixCall(Label status, int cCall, int cRaise, int options)
        {
            if (rounds != 4)
            {
                if (options == 1)
                {
                    if (status.Text.Contains("raise"))
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
                        bCall.Enabled = false;
                        bCall.Text = "Callisfuckedup";
                    }
                }
            }
        }

        async Task AllIn()
        {
            #region All in
            if (this.human.Chips <= 0 && !intsadded)
            {
                if (this.humanStatus.Text.Contains("raise"))
                {
                    this.chips.Add(this.human.Chips);
                    intsadded = true;
                }
                if (this.humanStatus.Text.Contains("Call"))
                {
                    this.chips.Add(this.human.Chips);
                    intsadded = true;
                }
            }
            intsadded = false;
            if (this.firstBot.Chips <= 0 && !this.firstBot.OutOfChips)
            {
                if (!intsadded)
                {
                    this.chips.Add(this.firstBot.Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (this.secondBot.Chips <= 0 && !this.secondBot.OutOfChips)
            {
                if (!intsadded)
                {
                    this.chips.Add(this.secondBot.Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (this.thirdBot.Chips <= 0 && !this.thirdBot.OutOfChips)
            {
                if (!intsadded)
                {
                    this.chips.Add(this.thirdBot.Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (this.fourthBot.Chips <= 0 && !this.fourthBot.OutOfChips)
            {
                if (!intsadded)
                {
                    this.chips.Add(this.fourthBot.Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (this.fifthBot.Chips <= 0 && !this.fifthBot.OutOfChips)
            {
                if (!intsadded)
                {
                    this.chips.Add(this.fifthBot.Chips);
                    intsadded = true;
                }
            }
            if (this.chips.ToArray().Length == maxLeft)
            {
                await Finish(2);
            }
            else
            {
                this.chips.Clear();
            }
            #endregion
            //TODO: previous name abs
            var leftPlayers = this.playersGameStatus.Count(x => x == false);

            #region LastManStanding
            if (leftPlayers == 1)
            {
                int index = this.playersGameStatus.IndexOf(false);
                if (index == 0)
                {
                    this.human.Chips += int.Parse(this.potStatus.Text);
                    tbChips.Text = this.human.Chips.ToString();
                    this.human.Panel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    this.firstBot.Chips += int.Parse(this.potStatus.Text);
                    tbChips.Text = this.firstBot.Chips.ToString();
                    this.firstBot.Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    this.secondBot.Chips += int.Parse(this.potStatus.Text);
                    tbChips.Text = this.secondBot.Chips.ToString();
                    this.secondBot.Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    this.thirdBot.Chips += int.Parse(this.potStatus.Text);
                    tbChips.Text = this.thirdBot.Chips.ToString();
                    this.thirdBot.Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    this.fourthBot.Chips += int.Parse(this.potStatus.Text);
                    tbChips.Text = this.fourthBot.Chips.ToString();
                    this.fourthBot.Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    this.fifthBot.Chips += int.Parse(this.potStatus.Text);
                    tbChips.Text = this.fifthBot.Chips.ToString();
                    this.fifthBot.Panel.Visible = true;
                    MessageBox.Show("Bot 5 Wins");
                }
                for (int j = 0; j <= 16; j++)
                {
                    this.cardsPictureBoxList[j].Visible = false;
                }
                await Finish(1);
            }
            intsadded = false;
            #endregion

            #region FiveOrLessLeft
            if (leftPlayers < 6 && leftPlayers > 1 && rounds >= End)
            {
                await Finish(2);
            }
            #endregion


        }

        async Task Finish(int n)
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
            this.Flop = 1;
            this.Turn = 2;
            this.River = 3;
            this.End = 4;
            this.maxLeft = 6;
            this.last = 123;
            this.raisedTurn = 1;
            this.playersGameStatus.Clear();
            this.CheckWinners.Clear();
            this.chips.Clear();
            this.Win.Clear();
            this.sorted.Current = 0;
            this.sorted.Power = 0;
            this.potStatus.Text = "0";
            this.t = 60;
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
                if (f2.a != 0)
                {
                    this.human.Chips = f2.a;
                    this.fifthBot.Chips += f2.a;
                    this.secondBot.Chips += f2.a;
                    this.thirdBot.Chips += f2.a;
                    this.fourthBot.Chips += f2.a;
                    this.fifthBot.Chips += f2.a;
                    this.human.OutOfChips = false;
                    this.human.CanMakeTurn = true;
                    this.bRaise.Enabled = true;
                    this.bFold.Enabled = true;
                    this.bCheck.Enabled = true;
                    this.bRaise.Text = "raise";
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

        void FixWinners()
        {
            Win.Clear();
            sorted.Current = 0;
            sorted.Power = 0;
            string fixedLast = "qwerty";
            if (!this.humanStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, "Player", this.human.Type, this.human.Power, this.human.OutOfChips);
            }

            if (!this.firstBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(2, 3, "Bot 1", this.firstBot.Type, this.firstBot.Power, this.firstBot.OutOfChips);
            }

            if (!this.secondBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(4, 5, "Bot 2", this.secondBot.Type, this.secondBot.Power, this.secondBot.OutOfChips);
            }

            if (!this.thirdBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(6, 7, "Bot 3", this.thirdBot.Type, this.thirdBot.Power, this.thirdBot.OutOfChips);
            }

            if (!this.fourthBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(8, 9, "Bot 4", this.fourthBot.Type, this.fourthBot.Power, this.fourthBot.OutOfChips);
            }

            if (!this.fifthBotStatus.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(10, 11, "Bot 5", this.fifthBot.Type, this.fifthBot.Power, this.fifthBot.OutOfChips);
            }

            //TODO: code repetition
            this.Winner(this.human.Type, this.human.Power, "Player", this.human.Chips, fixedLast);
            this.Winner(this.firstBot.Type, this.firstBot.Power, "Bot 1", this.firstBot.Chips, fixedLast);
            this.Winner(this.secondBot.Type, this.secondBot.Power, "Bot 2", this.secondBot.Chips, fixedLast);
            this.Winner(this.thirdBot.Type, this.thirdBot.Power, "Bot 3", this.thirdBot.Chips, fixedLast);
            this.Winner(this.fourthBot.Type, this.fourthBot.Power, "Bot 4", this.fourthBot.Chips, fixedLast);
            this.Winner(this.fifthBot.Type, this.fifthBot.Power, "Bot 5", this.fifthBot.Chips, fixedLast);
        }

        //void AI(int c1, int c2, int sChips, bool sTurn, bool sFTurn,
        //    Label sStatus, int name, double botPower, double botType)
        //{
        //    if (!sFTurn)
        //    {
        //        //TODO: consider switch case here
        //        if (botType == -1)
        //        {
        //            HighCard(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
        //        }
        //        if (botType == 0)
        //        {
        //            PairTable(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
        //        }
        //        if (botType == 1)
        //        {
        //            PairHand(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
        //        }
        //        if (botType == 2)
        //        {
        //            TwoPair(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
        //        }
        //        if (botType == 3)
        //        {
        //            ThreeOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
        //        }
        //        if (botType == 4)
        //        {
        //            Straight(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
        //        }
        //        if (botType == 5 || botType == 5.5)
        //        {
        //            Flush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
        //        }
        //        if (botType == 6)
        //        {
        //            FullHouse(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
        //        }
        //        if (botType == 7)
        //        {
        //            FourOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
        //        }
        //        if (botType == 8 || botType == 9)
        //        {
        //            StraightFlush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
        //        }
        //    }
        //    if (sFTurn)
        //    {
        //        this.cardsPictureBoxList[c1].Visible = false;
        //        this.cardsPictureBoxList[c2].Visible = false;
        //    }
        //}

        void AI(int c1, int c2, Label sStatus, int name, IPlayer player)
        {
            if (!player.OutOfChips)
            {
                if (player.Type == -1)
                {
                    //this.HighCard(pokerPlayer, sStatus);

                    this.handType.HighCard(player, sStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising);
                }

                if (player.Type == 0)
                {
                    //this.PairTable(pokerPlayer, sStatus);

                    this.handType.PairTable(player, sStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising);
                }

                if (player.Type == 1)
                {
                    //this.PairHand(pokerPlayer, sStatus);

                    this.handType.PairHand(player, sStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 2)
                {
                    //this.TwoPair(pokerPlayer, sStatus);

                    this.handType.TwoPair(player, sStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 3)
                {
                    //this.ThreeOfAKind(pokerPlayer, sStatus, name);

                    this.handType.ThreeOfAKind(player, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 4)
                {
                    //this.Straight(pokerPlayer, sStatus, name);

                    this.handType.Straight(player, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 5 || player.Type == 5.5)
                {
                    //this.Flush(pokerPlayer, sStatus, name);

                    this.handType.Flush(player, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 6)
                {
                    //this.FullHouse(pokerPlayer, sStatus, name);

                    this.handType.FullHouse(player, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 7)
                {
                    //this.FourOfAKind(pokerPlayer, sStatus, name);

                    this.handType.FourOfAKind(player, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }

                if (player.Type == 8 || player.Type == 9)
                {
                    //this.StraightFlush(pokerPlayer, sStatus, name);

                    this.handType.StraightFlush(player, sStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                }
            }

            if (player.OutOfChips)
            {
                this.cardsPictureBoxList[c1].Visible = false;
                this.cardsPictureBoxList[c2].Visible = false;
            }
        }

        private void HighCard(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            HP(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 20, 25);
        }

        private void PairTable(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            HP(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower, 16, 25);
        }

        private void PairHand(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(10, 16);
            int rRaise = rPair.Next(10, 13);
            if (botPower <= 199 && botPower >= 140)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 6, rRaise);
            }
            if (botPower <= 139 && botPower >= 128)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 7, rRaise);
            }
            if (botPower < 128 && botPower >= 101)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 9, rRaise);
            }
        }

        private void TwoPair(ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, double botPower)
        {
            Random rPair = new Random();
            int rCall = rPair.Next(6, 11);
            int rRaise = rPair.Next(6, 11);
            if (botPower <= 290 && botPower >= 246)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 3, rRaise);
            }
            if (botPower <= 244 && botPower >= 234)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
            if (botPower < 234 && botPower >= 201)
            {
                PH(ref sChips, ref sTurn, ref sFTurn, sStatus, rCall, 4, rRaise);
            }
        }

        private void ThreeOfAKind(
            ref int botCurrentChips,
            ref bool sTurn,
            ref bool sFTurn,
            Label sStatus,
            int name,
            double botPower)
        {
            Random tk = new Random();
            int tCall = tk.Next(3, 7);
            int tRaise = tk.Next(4, 8);

            if (botPower <= 390 && botPower >= 330)
            {
                //TODO: previously Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
                Smooth(ref botCurrentChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }

            if (botPower <= 327 && botPower >= 321)//10  8
            {
                Smooth(ref botCurrentChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }

            if (botPower < 321 && botPower >= 303)//7 2
            {
                Smooth(ref botCurrentChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }
        }

        private void Straight(
            ref int sChips,
            ref bool sTurn,
            ref bool sFTurn,
            Label sStatus,
            int name,
            double botPower)
        {
            Random str = new Random();
            int sCall = str.Next(3, 6);
            int sRaise = str.Next(3, 8);

            if (botPower <= 480 && botPower >= 410)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }

            if (botPower <= 409 && botPower >= 407)//10  8
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }

            if (botPower < 407 && botPower >= 404)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sCall, sRaise);
            }
        }

        private void Flush(
            ref int sChips,
            ref bool sTurn,
            ref bool sFTurn,
            Label sStatus,
            int name,
            double botPower)
        {
            Random fsh = new Random();
            int fCall = fsh.Next(2, 6);
            int fRaise = fsh.Next(3, 7);
            Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fCall, fRaise);
        }

        private void FullHouse(
            ref int sChips,
            ref bool sTurn,
            ref bool sFTurn,
            Label sStatus,
            int name,
            double botPower)
        {
            Random flh = new Random();
            int fhCall = flh.Next(1, 5);
            int fhRaise = flh.Next(2, 6);
            if (botPower <= 626 && botPower >= 620)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }

            if (botPower < 620 && botPower >= 602)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fhCall, fhRaise);
            }
        }

        private void FourOfAKind(
            ref int sChips,
            ref bool sTurn,
            ref bool sFTurn,
            Label sStatus,
            int name,
            double botPower)
        {
            Random fk = new Random();
            int fkCall = fk.Next(1, 4);
            int fkRaise = fk.Next(2, 5);
            if (botPower <= 752 && botPower >= 704)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, fkCall, fkRaise);
            }
        }

        private void StraightFlush(
            ref int sChips,
            ref bool sTurn,
            ref bool sFTurn,
            Label sStatus,
            int name,
            double botPower)
        {
            Random sf = new Random();
            int sfCall = sf.Next(1, 3);
            int sfRaise = sf.Next(1, 3);
            if (botPower <= 913 && botPower >= 804)
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, sfCall, sfRaise);
            }
        }

        private void Fold(ref bool sTurn, ref bool sFTurn, Label sStatus)
        {
            raising = false;
            sStatus.Text = "Fold";
            sTurn = false;
            sFTurn = true;
        }

        private void PlayerCheck(ref bool botIsOnTurn, Label botStatus)
        {
            botStatus.Text = "Check";
            botIsOnTurn = false;
            raising = false;
        }

        //TODO:TODO:
        private void Call(ref int sChips, ref bool sTurn, Label sStatus)
        {
            raising = false;
            sTurn = false;
            sChips -= this.neededChipsToCall;
            sStatus.Text = "Call " + this.neededChipsToCall;
            this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.neededChipsToCall).ToString();
        }

        private void Raised(ref int sChips, ref bool sTurn, Label sStatus)
        {
            sChips -= Convert.ToInt32(this.raise);
            sStatus.Text = "raise " + this.raise;
            this.potStatus.Text = (int.Parse(this.potStatus.Text) + Convert.ToInt32(this.raise)).ToString();
            this.neededChipsToCall = Convert.ToInt32(this.raise);
            raising = true;
            sTurn = false;
        }

        private static double RoundN(int sChips, int n)
        {
            double a = Math.Round((sChips / n) / 100d, 0) * 100;
            return a;
        }

        private void HP(
            ref int sChips,
            ref bool sTurn,
            ref bool sFTurn,
            Label sStatus,
            double botPower,
            int n,
            int n1)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 4);
            if (this.neededChipsToCall <= 0)
            {
                this.PlayerCheck(ref sTurn, sStatus);
            }

            if (this.neededChipsToCall > 0)
            {
                if (rnd == 1)
                {
                    if (this.neededChipsToCall <= RoundN(sChips, n))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }

                if (rnd == 2)
                {
                    if (this.neededChipsToCall <= RoundN(sChips, n1))
                    {
                        Call(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }

            if (rnd == 3)
            {
                if (this.raise == 0)
                {
                    this.raise = this.neededChipsToCall * 2;
                    Raised(ref sChips, ref sTurn, sStatus);
                }
                else
                {
                    if (this.raise <= RoundN(sChips, n))
                    {
                        this.raise = this.neededChipsToCall * 2;
                        Raised(ref sChips, ref sTurn, sStatus);
                    }
                    else
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }
                }
            }

            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }

        private void PH(
            ref int sChips,
            ref bool sTurn,
            ref bool sFTurn,
            Label sStatus,
            int n,
            int n1,
            int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);

            if (rounds < 2)
            {
                if (this.neededChipsToCall <= 0)
                {
                    this.PlayerCheck(ref sTurn, sStatus);
                }

                if (this.neededChipsToCall > 0)
                {
                    if (this.neededChipsToCall >= RoundN(sChips, n1))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (this.raise > RoundN(sChips, n))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (!sFTurn)
                    {
                        if (this.neededChipsToCall >= RoundN(sChips, n) && this.neededChipsToCall <= RoundN(sChips, n1))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (this.raise <= RoundN(sChips, n) && this.raise >= (RoundN(sChips, n)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }

                        //if (this.raise <= (RoundN(sChips, n)) / 2)
                        //{
                        //    if (this.raise > 0)
                        //    {
                        //        this.raise = RoundN(sChips, n);
                        //        Raised(ref sChips, ref sTurn, sStatus);
                        //    }
                        //    else
                        //    {
                        //        this.raise = this.neededChipsToCall * 2;
                        //        Raised(ref sChips, ref sTurn, sStatus);
                        //    }
                        //}
                    }
                }
            }

            if (rounds >= 2)
            {
                if (this.neededChipsToCall > 0)
                {
                    if (this.neededChipsToCall >= RoundN(sChips, n1 - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (this.raise > RoundN(sChips, n - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (!sFTurn)
                    {
                        if (this.neededChipsToCall >= RoundN(sChips, n - rnd) && this.neededChipsToCall <= RoundN(sChips, n1 - rnd))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (this.raise <= RoundN(sChips, n - rnd) &&
                            this.raise >= (RoundN(sChips, n - rnd)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }

                        //if (this.raise <= (RoundN(sChips, n - rnd)) / 2)
                        //{
                        //    if (this.raise > 0)
                        //    {
                        //        this.raise = RoundN(sChips, n - rnd);
                        //        Raised(ref sChips, ref sTurn, sStatus);
                        //    }
                        //    else
                        //    {
                        //        this.raise = this.neededChipsToCall * 2;
                        //        Raised(ref sChips, ref sTurn, sStatus);
                        //    }
                        //}
                    }
                }

                if (this.neededChipsToCall <= 0)
                {
                    //this.raise = RoundN(sChips, r - rnd);
                    Raised(ref sChips, ref sTurn, sStatus);
                }
            }

            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }

        void Smooth(
            ref int botCurrentChips,
            ref bool botIsOnTurn,
            ref bool botFTurn,
            Label botStatus,
            int name,
            int n,
            int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (this.neededChipsToCall <= 0)
            {
                //neededChipsToCall is none, so the player checks/skips
                this.PlayerCheck(ref botIsOnTurn, botStatus);
            }
            else
            {
                if (this.neededChipsToCall >= RoundN(botCurrentChips, n))
                {
                    if (botCurrentChips > this.neededChipsToCall)
                    {
                        Call(ref botCurrentChips, ref botIsOnTurn, botStatus);
                    }
                    else if (botCurrentChips <= this.neededChipsToCall)
                    {
                        raising = false;
                        botIsOnTurn = false;
                        botCurrentChips = 0;
                        botStatus.Text = "Call " + botCurrentChips;
                        this.potStatus.Text = (int.Parse(this.potStatus.Text) + botCurrentChips).ToString();
                    }
                }
                else
                {
                    if (this.raise > 0)
                    {
                        if (botCurrentChips >= this.raise * 2)
                        {
                            this.raise *= 2;
                            Raised(ref botCurrentChips, ref botIsOnTurn, botStatus);
                        }
                        else
                        {
                            Call(ref botCurrentChips, ref botIsOnTurn, botStatus);
                        }
                    }
                    else
                    {
                        this.raise = this.neededChipsToCall * 2;
                        Raised(ref botCurrentChips, ref botIsOnTurn, botStatus);
                    }
                }
            }

            if (botCurrentChips <= 0)
            {
                botFTurn = true;
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

            if (t > 0)
            {
                t--;
                pbTimer.Value = (t / 6) * 100;
            }
        }

        private void UpdateTick(object sender, object e)
        {
            if (this.human.Chips <= 0)
            {
                tbChips.Text = "Chips : 0";
            }

            if (this.firstBot.Chips <= 0)
            {
                tbBotChips1.Text = "Chips : 0";
            }

            if (this.secondBot.Chips <= 0)
            {
                tbBotChips2.Text = "Chips : 0";
            }

            if (this.thirdBot.Chips <= 0)
            {
                tbBotChips3.Text = "Chips : 0";
            }

            if (this.fourthBot.Chips <= 0)
            {
                tbBotChips4.Text = "Chips : 0";
            }

            if (this.fifthBot.Chips <= 0)
            {
                tbBotChips5.Text = "Chips : 0";
            }

            //TODO: extact in method
            tbChips.Text = "Chips : " + this.human.Chips;
            tbBotChips1.Text = "Chips : " + this.firstBot.Chips;
            tbBotChips2.Text = "Chips : " + this.secondBot.Chips;
            tbBotChips3.Text = "Chips : " + this.thirdBot.Chips;
            tbBotChips4.Text = "Chips : " + this.fourthBot.Chips;
            tbBotChips5.Text = "Chips : " + this.fifthBot.Chips;

            if (this.human.Chips <= 0)
            {
                this.human.CanMakeTurn = false;
                this.human.OutOfChips = true;
                bCall.Enabled = false;
                bRaise.Enabled = false;
                bFold.Enabled = false;
                bCheck.Enabled = false;
            }

            if (up > 0)
            {
                up--;
            }

            if (this.human.Chips >= this.neededChipsToCall)
            {
                bCall.Text = "Call " + this.neededChipsToCall.ToString();
            }
            else
            {
                bCall.Text = "All in";
                bRaise.Enabled = false;
            }

            if (this.neededChipsToCall > 0)
            {
                bCheck.Enabled = false;
            }

            if (this.neededChipsToCall <= 0)
            {
                bCheck.Enabled = true;
                bCall.Text = "Call";
                bCall.Enabled = false;
            }

            if (this.human.Chips <= 0)
            {
                bRaise.Enabled = false;
            }

            int parsedValue;

            if (tbRaise.Text != "" && int.TryParse(tbRaise.Text, out parsedValue))
            {
                if (this.human.Chips <= int.Parse(tbRaise.Text))
                {
                    bRaise.Text = "All in";
                }
                else
                {
                    bRaise.Text = "raise";
                }
            }

            if (this.human.Chips < this.neededChipsToCall)
            {
                bRaise.Enabled = false;
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

                bCheck.Enabled = false;
            }
            await Turns();
        }

        private async void ButtonCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", this.human.Type, this.human.Power, this.human.OutOfChips);
            if (this.human.Chips >= this.neededChipsToCall)
            {
                this.human.Chips -= this.neededChipsToCall;
                tbChips.Text = "Chips : " + this.human.Chips.ToString();
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
                tbChips.Text = "Chips : " + this.human.Chips.ToString();
                this.human.CanMakeTurn = false;
                bFold.Enabled = false;
                this.human.Call = this.human.Chips;
            }

            await Turns();
        }

        private async void ButtonRaise_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", this.human.Type, this.human.Power, this.human.OutOfChips);
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
                            this.humanStatus.Text = "raise " + this.neededChipsToCall.ToString();
                            this.potStatus.Text = (int.Parse(this.potStatus.Text) + this.neededChipsToCall).ToString();
                            bCall.Text = "Call";
                            this.human.Chips -= int.Parse(tbRaise.Text);
                            raising = true;
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
                            raising = true;
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

        //TODO: validate chips are integer
        private void ButtonAddChipsClick(object sender, EventArgs e)
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

            tbChips.Text = "Chips : " + this.human.Chips;
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