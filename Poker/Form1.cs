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

namespace Poker
{

    public partial class Form1 : Form
    {
        #region Variables
        private ProgressBar asd = new ProgressBar();
        private int Nm;
        private Panel playerPanel = new Panel();
        private Panel bot1Panel = new Panel();
        private Panel bot2Panel = new Panel();
        private Panel bot3Panel = new Panel();
        private Panel bot4Panel = new Panel();
        private Panel bot5Panel = new Panel();

        private int call = 500;
        private int foldedPlayers = 5;
        private int Chips = 10000;
        private int bot1Chips = 10000;
        private int bot2Chips = 10000;
        private int bot3Chips = 10000;
        private int bot4Chips = 10000;
        private int bot5Chips = 10000;

        private double type;
        private double rounds;
        private double b1Power;
        private double b2Power;
        private double b3Power;
        private double b4Power;
        private double b5Power;
        private double pPower;
        private double pType = -1;
        private double raise;
        private double b1Type = -1;
        private double b2Type = -1;
        private double b3Type = -1;
        private double b4Type = -1;
        private double b5Type = -1;

        private bool B1turn;
        private bool B2turn;
        private bool B3turn;
        private bool B4turn;
        private bool B5turn;
        private bool B1Fturn;
        private bool B2Fturn;
        private bool B3Fturn;
        private bool B4Fturn;
        private bool B5Fturn;
        private bool pFolded;
        private bool b1Folded;
        private bool b2Folded;
        private bool b3Folded;
        private bool b4Folded;
        private bool b5Folded;
        private bool intsadded;
        private bool changed;

        private int pCall = 0;
        private int b1Call;
        private int b2Call;
        private int b3Call;
        private int b4Call;
        private int b5Call;
        private int pRaise;
        private int b1Raise;
        private int b2Raise;
        private int b3Raise;
        private int b4Raise;
        private int b5Raise;

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

        //TODO: Check if name is proper, previous name - bools
        private List<bool?> playersGameStatus = new List<bool?>();
        private List<Type> Win = new List<Type>();
        private List<string> CheckWinners = new List<string>();
        //TODO: Check if name is proper, previous name - chips
        private List<int> chips = new List<int>();

        private bool PFturn = false;
        private bool Pturn = true;
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

        public Form1()
        {
            //playersGameStatus.Add(PFturn); playersGameStatus.Add(B1Fturn); playersGameStatus.Add(B2Fturn); playersGameStatus.Add(B3Fturn); playersGameStatus.Add(B4Fturn); playersGameStatus.Add(B5Fturn);
            this.call = this.bigBlindValue;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Updates.Start();
            this.InitializeComponent();
            this.width = this.Width;
            this.height = this.Height;
            this.Shuffle();
            this.tbPot.Enabled = false;
            this.tbChips.Enabled = false;
            this.tbBotChips1.Enabled = false;
            this.tbBotChips2.Enabled = false;
            this.tbBotChips3.Enabled = false;
            this.tbBotChips4.Enabled = false;
            this.tbBotChips5.Enabled = false;
            this.tbChips.Text = "Chips : " + Chips.ToString();
            this.tbBotChips1.Text = "Chips : " + bot1Chips;
            this.tbBotChips2.Text = "Chips : " + bot2Chips;
            this.tbBotChips3.Text = "Chips : " + bot3Chips;
            this.tbBotChips4.Text = "Chips : " + bot4Chips;
            this.tbBotChips5.Text = "Chips : " + bot5Chips;
            this.timer.Interval = (1 * 1 * 1000);
            this.timer.Tick += this.TimerTick;
            this.Updates.Interval = (1 * 1 * 100);
            this.Updates.Tick += this.UpdateTick;
            this.tbBigBlind.Visible = true;
            this.tbSmallBlind.Visible = true;
            this.bBB.Visible = true;
            this.bSB.Visible = true;
            this.tbBigBlind.Visible = true;
            this.tbSmallBlind.Visible = true;
            this.bBB.Visible = true;
            this.bSB.Visible = true;
            this.tbBigBlind.Visible = false;
            this.tbSmallBlind.Visible = false;
            this.bBB.Visible = false;
            this.bSB.Visible = false;
            this.tbRaise.Text = (this.bigBlindValue * 2).ToString();
        }

        async Task Shuffle()
        {
            this.playersGameStatus.Add(this.PFturn);
            this.playersGameStatus.Add(this.B1Fturn);
            this.playersGameStatus.Add(this.B2Fturn);
            this.playersGameStatus.Add(this.B3Fturn);
            this.playersGameStatus.Add(this.B4Fturn);
            this.playersGameStatus.Add(this.B5Fturn);
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
                var partsToRemove = new [] { "..\\..\\Resources\\Assets\\Cards\\", ".png" };
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
                    this.Controls.Add(this.playerPanel);
                    this.playerPanel.Location = new Point(this.cardsPictureBoxList[0].Left - 10, this.cardsPictureBoxList[0].Top - 10);
                    this.playerPanel.BackColor = Color.DarkBlue;
                    this.playerPanel.Height = 150;
                    this.playerPanel.Width = 180;
                    this.playerPanel.Visible = false;
                }

                if (bot1Chips > 0)
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
                        this.Controls.Add(this.bot1Panel);
                        this.bot1Panel.Location = new Point(this.cardsPictureBoxList[2].Left - 10, this.cardsPictureBoxList[2].Top - 10);
                        this.bot1Panel.BackColor = Color.DarkBlue;
                        this.bot1Panel.Height = 150;
                        this.bot1Panel.Width = 180;
                        this.bot1Panel.Visible = false;
                        if (i == 3)
                        {
                            check = false;
                        }
                    }
                }

                if (bot2Chips > 0)
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
                        this.Controls.Add(this.bot2Panel);
                        this.bot2Panel.Location = new Point(this.cardsPictureBoxList[4].Left - 10, this.cardsPictureBoxList[4].Top - 10);
                        this.bot2Panel.BackColor = Color.DarkBlue;
                        this.bot2Panel.Height = 150;
                        this.bot2Panel.Width = 180;
                        this.bot2Panel.Visible = false;
                        if (i == 5)
                        {
                            check = false;
                        }
                    }
                }

                if (bot3Chips > 0)
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
                        this.Controls.Add(this.bot3Panel);
                        this.bot3Panel.Location = new Point(this.cardsPictureBoxList[6].Left - 10, this.cardsPictureBoxList[6].Top - 10);
                        this.bot3Panel.BackColor = Color.DarkBlue;
                        this.bot3Panel.Height = 150;
                        this.bot3Panel.Width = 180;
                        this.bot3Panel.Visible = false;
                        if (i == 7)
                        {
                            check = false;
                        }
                    }
                }

                if (bot4Chips > 0)
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
                        this.Controls.Add(this.bot4Panel);
                        this.bot4Panel.Location = new Point(this.cardsPictureBoxList[8].Left - 10, this.cardsPictureBoxList[8].Top - 10);
                        this.bot4Panel.BackColor = Color.DarkBlue;
                        this.bot4Panel.Height = 150;
                        this.bot4Panel.Width = 180;
                        this.bot4Panel.Visible = false;
                        if (i == 9)
                        {
                            check = false;
                        }
                    }
                }

                if (bot5Chips > 0)
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
                        this.Controls.Add(this.bot5Panel);
                        this.bot5Panel.Location = new Point(this.cardsPictureBoxList[10].Left - 10, this.cardsPictureBoxList[10].Top - 10);
                        this.bot5Panel.BackColor = Color.DarkBlue;
                        this.bot5Panel.Height = 150;
                        this.bot5Panel.Width = 180;
                        this.bot5Panel.Visible = false;
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
                if (bot1Chips <= 0)
                {
                    B1Fturn = true;
                    this.cardsPictureBoxList[2].Visible = false;
                    this.cardsPictureBoxList[3].Visible = false;
                }
                else
                {
                    B1Fturn = false;
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

                if (bot2Chips <= 0)
                {
                    B2Fturn = true;
                    this.cardsPictureBoxList[4].Visible = false;
                    this.cardsPictureBoxList[5].Visible = false;
                }
                else
                {
                    B2Fturn = false;
                    if (i == 5)
                    {
                        if (this.cardsPictureBoxList[5] != null)
                        {
                            this.cardsPictureBoxList[4].Visible = true;
                            this.cardsPictureBoxList[5].Visible = true;
                        }
                    }
                }

                if (bot3Chips <= 0)
                {
                    B3Fturn = true;
                    this.cardsPictureBoxList[6].Visible = false;
                    this.cardsPictureBoxList[7].Visible = false;
                }
                else
                {
                    B3Fturn = false;
                    if (i == 7)
                    {
                        if (this.cardsPictureBoxList[7] != null)
                        {
                            this.cardsPictureBoxList[6].Visible = true;
                            this.cardsPictureBoxList[7].Visible = true;
                        }
                    }
                }

                if (bot4Chips <= 0)
                {
                    B4Fturn = true;
                    this.cardsPictureBoxList[8].Visible = false;
                    this.cardsPictureBoxList[9].Visible = false;
                }
                else
                {
                    B4Fturn = false;
                    if (i == 9)
                    {
                        if (this.cardsPictureBoxList[9] != null)
                        {
                            this.cardsPictureBoxList[8].Visible = true;
                            this.cardsPictureBoxList[9].Visible = true;
                        }
                    }
                }

                if (bot5Chips <= 0)
                {
                    B5Fturn = true;
                    this.cardsPictureBoxList[10].Visible = false;
                    this.cardsPictureBoxList[11].Visible = false;
                }
                else
                {
                    B5Fturn = false;
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
            if (!PFturn)
            {
                if (Pturn)
                {
                    FixCall(pStatus, ref pCall, ref pRaise, 1);
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
                    FixCall(pStatus, ref pCall, ref pRaise, 2);
                }
            }

            if (PFturn || !Pturn)
            {
                await AllIn();
                if (PFturn && !pFolded)
                {
                    if (bCall.Text.Contains("All in") == false || bRaise.Text.Contains("All in") == false)
                    {
                        this.playersGameStatus.RemoveAt(0);
                        this.playersGameStatus.Insert(0, null);
                        maxLeft--;
                        pFolded = true;
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
                B1turn = true;
                if (!B1Fturn)
                {
                    if (B1turn)
                    {
                        FixCall(b1Status, ref b1Call, ref b1Raise, 1);
                        FixCall(b1Status, ref b1Call, ref b1Raise, 2);
                        Rules(2, 3, "Bot 1", ref b1Type, ref b1Power, B1Fturn);
                        MessageBox.Show("Bot 1's Turn");
                        AI(2, 3, ref bot1Chips, ref B1turn, ref B1Fturn, b1Status, 0, b1Power, b1Type);
                        turnCount++;
                        last = 1;
                        B1turn = false;
                        B2turn = true;
                    }
                }

                if (B1Fturn && !b1Folded)
                {
                    this.playersGameStatus.RemoveAt(1);
                    this.playersGameStatus.Insert(1, null);
                    maxLeft--;
                    b1Folded = true;
                }

                if (B1Fturn || !B1turn)
                {
                    await CheckRaise(1, 1);
                    B2turn = true;
                }

                if (!B2Fturn)
                {
                    if (B2turn)
                    {
                        FixCall(b2Status, ref b2Call, ref b2Raise, 1);
                        FixCall(b2Status, ref b2Call, ref b2Raise, 2);
                        Rules(4, 5, "Bot 2", ref b2Type, ref b2Power, B2Fturn);
                        MessageBox.Show("Bot 2's Turn");
                        AI(4, 5, ref bot2Chips, ref B2turn, ref B2Fturn, b2Status, 1, b2Power, b2Type);
                        turnCount++;
                        last = 2;
                        B2turn = false;
                        B3turn = true;
                    }
                }

                if (B2Fturn && !b2Folded)
                {
                    this.playersGameStatus.RemoveAt(2);
                    this.playersGameStatus.Insert(2, null);
                    maxLeft--;
                    b2Folded = true;
                }

                if (B2Fturn || !B2turn)
                {
                    await CheckRaise(2, 2);
                    B3turn = true;
                }

                if (!B3Fturn)
                {
                    if (B3turn)
                    {
                        FixCall(b3Status, ref b3Call, ref b3Raise, 1);
                        FixCall(b3Status, ref b3Call, ref b3Raise, 2);
                        Rules(6, 7, "Bot 3", ref b3Type, ref b3Power, B3Fturn);
                        MessageBox.Show("Bot 3's Turn");
                        AI(6, 7, ref bot3Chips, ref B3turn, ref B3Fturn, b3Status, 2, b3Power, b3Type);
                        turnCount++;
                        last = 3;
                        B3turn = false;
                        B4turn = true;
                    }
                }

                if (B3Fturn && !b3Folded)
                {
                    this.playersGameStatus.RemoveAt(3);
                    this.playersGameStatus.Insert(3, null);
                    maxLeft--;
                    b3Folded = true;
                }

                if (B3Fturn || !B3turn)
                {
                    await CheckRaise(3, 3);
                    B4turn = true;
                }

                if (!B4Fturn)
                {
                    if (B4turn)
                    {
                        FixCall(b4Status, ref b4Call, ref b4Raise, 1);
                        FixCall(b4Status, ref b4Call, ref b4Raise, 2);
                        Rules(8, 9, "Bot 4", ref b4Type, ref b4Power, B4Fturn);
                        MessageBox.Show("Bot 4's Turn");
                        AI(8, 9, ref bot4Chips, ref B4turn, ref B4Fturn, b4Status, 3, b4Power, b4Type);
                        turnCount++;
                        last = 4;
                        B4turn = false;
                        B5turn = true;
                    }
                }

                if (B4Fturn && !b4Folded)
                {
                    this.playersGameStatus.RemoveAt(4);
                    this.playersGameStatus.Insert(4, null);
                    maxLeft--;
                    b4Folded = true;
                }

                if (B4Fturn || !B4turn)
                {
                    await CheckRaise(4, 4);
                    B5turn = true;
                }

                if (!B5Fturn)
                {
                    if (B5turn)
                    {
                        FixCall(b5Status, ref b5Call, ref b5Raise, 1);
                        FixCall(b5Status, ref b5Call, ref b5Raise, 2);
                        Rules(10, 11, "Bot 5", ref b5Type, ref b5Power, B5Fturn);
                        MessageBox.Show("Bot 5's Turn");
                        AI(10, 11, ref bot5Chips, ref B5turn, ref B5Fturn, b5Status, 4, b5Power, b5Type);
                        turnCount++;
                        last = 5;
                        B5turn = false;
                    }
                }

                if (B5Fturn && !b5Folded)
                {
                    this.playersGameStatus.RemoveAt(5);
                    this.playersGameStatus.Insert(5, null);
                    maxLeft--;
                    b5Folded = true;
                }

                if (B5Fturn || !B5turn)
                {
                    await CheckRaise(5, 5);
                    Pturn = true;
                }

                if (PFturn && !pFolded)
                {
                    if (bCall.Text.Contains("All in") == false || bRaise.Text.Contains("All in") == false)
                    {
                        this.playersGameStatus.RemoveAt(0);
                        this.playersGameStatus.Insert(0, null);
                        maxLeft--;
                        pFolded = true;
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
            ref double current,
            ref double Power,
            bool foldedTurn)
        {
            if (cardOne == 0 && cardTwo == 1)
            {
            }

            if (!foldedTurn || cardOne == 0 && cardTwo == 1 && pStatus.Text.Contains("Fold") == false)
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
                        Chips += int.Parse(tbPot.Text) / winners;
                        tbChips.Text = Chips.ToString();
                        //playerPanel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        bot1Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips1.Text = bot1Chips.ToString();
                        //bot1Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        bot2Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips2.Text = bot2Chips.ToString();
                        //bot2Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        bot3Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips3.Text = bot3Chips.ToString();
                        //bot3Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        bot4Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips4.Text = bot4Chips.ToString();
                        //bot4Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        bot5Chips += int.Parse(tbPot.Text) / winners;
                        tbBotChips5.Text = bot5Chips.ToString();
                        //bot5Panel.Visible = true;
                    }
                    //await Finish(1);
                }
                if (winners == 1)
                {
                    if (CheckWinners.Contains("Player"))
                    {
                        Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //playerPanel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 1"))
                    {
                        bot1Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //bot1Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 2"))
                    {
                        bot2Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //bot2Panel.Visible = true;

                    }
                    if (CheckWinners.Contains("Bot 3"))
                    {
                        bot3Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //bot3Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 4"))
                    {
                        bot4Chips += int.Parse(tbPot.Text);
                        //await Finish(1);
                        //bot4Panel.Visible = true;
                    }
                    if (CheckWinners.Contains("Bot 5"))
                    {
                        bot5Chips += int.Parse(tbPot.Text);
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
                        call = 0;
                        raisedTurn = 123;
                        rounds++;
                        if (!PFturn)
                            pStatus.Text = "";
                        if (!B1Fturn)
                            b1Status.Text = "";
                        if (!B2Fturn)
                            b2Status.Text = "";
                        if (!B3Fturn)
                            b3Status.Text = "";
                        if (!B4Fturn)
                            b4Status.Text = "";
                        if (!B5Fturn)
                            b5Status.Text = "";
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
                        pCall = 0; pRaise = 0;
                        b1Call = 0; b1Raise = 0;
                        b2Call = 0; b2Raise = 0;
                        b3Call = 0; b3Raise = 0;
                        b4Call = 0; b4Raise = 0;
                        b5Call = 0; b5Raise = 0;
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
                        pCall = 0; pRaise = 0;
                        b1Call = 0; b1Raise = 0;
                        b2Call = 0; b2Raise = 0;
                        b3Call = 0; b3Raise = 0;
                        b4Call = 0; b4Raise = 0;
                        b5Call = 0; b5Raise = 0;
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
                        pCall = 0; pRaise = 0;
                        b1Call = 0; b1Raise = 0;
                        b2Call = 0; b2Raise = 0;
                        b3Call = 0; b3Raise = 0;
                        b4Call = 0; b4Raise = 0;
                        b5Call = 0; b5Raise = 0;
                    }
                }
            }
            if (rounds == End && maxLeft == 6)
            {
                string fixedLast = "qwerty";
                if (!pStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    Rules(0, 1, "Player", ref pType, ref pPower, PFturn);
                }
                if (!b1Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    Rules(2, 3, "Bot 1", ref b1Type, ref b1Power, B1Fturn);
                }
                if (!b2Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    Rules(4, 5, "Bot 2", ref b2Type, ref b2Power, B2Fturn);
                }
                if (!b3Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    Rules(6, 7, "Bot 3", ref b3Type, ref b3Power, B3Fturn);
                }
                if (!b4Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    Rules(8, 9, "Bot 4", ref b4Type, ref b4Power, B4Fturn);
                }
                if (!b5Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    Rules(10, 11, "Bot 5", ref b5Type, ref b5Power, B5Fturn);
                }
                Winner(pType, pPower, "Player", Chips, fixedLast);
                Winner(b1Type, b1Power, "Bot 1", bot1Chips, fixedLast);
                Winner(b2Type, b2Power, "Bot 2", bot2Chips, fixedLast);
                Winner(b3Type, b3Power, "Bot 3", bot3Chips, fixedLast);
                Winner(b4Type, b4Power, "Bot 4", bot4Chips, fixedLast);
                Winner(b5Type, b5Power, "Bot 5", bot5Chips, fixedLast);
                restart = true;
                Pturn = true;
                PFturn = false;
                B1Fturn = false;
                B2Fturn = false;
                B3Fturn = false;
                B4Fturn = false;
                B5Fturn = false;
                if (Chips <= 0)
                {
                    AddChips f2 = new AddChips();
                    f2.ShowDialog();
                    if (f2.a != 0)
                    {
                        Chips = f2.a;
                        bot1Chips += f2.a;
                        bot2Chips += f2.a;
                        bot3Chips += f2.a;
                        bot4Chips += f2.a;
                        bot5Chips += f2.a;
                        PFturn = false;
                        Pturn = true;
                        bRaise.Enabled = true;
                        bFold.Enabled = true;
                        bCheck.Enabled = true;
                        bRaise.Text = "raise";
                    }
                }
                this.playerPanel.Visible = false;
                this.bot1Panel.Visible = false;
                this.bot2Panel.Visible = false;
                this.bot3Panel.Visible = false;
                this.bot4Panel.Visible = false;
                this.bot5Panel.Visible = false;
                this.pCall = 0;
                this.pRaise = 0;
                this.b1Call = 0;
                this.b1Raise = 0;
                this.b2Call = 0;
                this.b2Raise = 0;
                this.b3Call = 0;
                this.b3Raise = 0;
                this.b4Call = 0;
                this.b4Raise = 0;
                this.b5Call = 0;
                this.b5Raise = 0;
                this.last = 0;
                this.call = this.bigBlindValue;
                this.raise = 0;
                this.cardsImageLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                this.playersGameStatus.Clear();
                this.rounds = 0;
                this.pPower = 0; pType = -1;
                this.type = 0;
                this.b1Power = 0;
                this.b2Power = 0;
                this.b3Power = 0;
                this.b4Power = 0;
                this.b5Power = 0;
                this.b1Type = -1;
                this.b2Type = -1;
                this.b3Type = -1;
                this.b4Type = -1;
                this.b5Type = -1;
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
                tbPot.Text = "0";
                pStatus.Text = "";
                await Shuffle();
                await Turns();
            }
        }

        void FixCall(Label status, ref int cCall, ref int cRaise, int options)
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
                    if (status.Text.Contains("Check"))
                    {
                        cRaise = 0;
                        cCall = 0;
                    }
                }
                if (options == 2)
                {
                    if (cRaise != this.raise && cRaise <= this.raise)
                    {
                        call = Convert.ToInt32(this.raise) - cRaise;
                    }
                    if (cCall != call || cCall <= call)
                    {
                        call = call - cCall;
                    }
                    if (cRaise == this.raise && this.raise > 0)
                    {
                        call = 0;
                        bCall.Enabled = false;
                        bCall.Text = "Callisfuckedup";
                    }
                }
            }
        }

        async Task AllIn()
        {
            #region All in
            if (Chips <= 0 && !intsadded)
            {
                if (pStatus.Text.Contains("raise"))
                {
                    this.chips.Add(Chips);
                    intsadded = true;
                }
                if (pStatus.Text.Contains("Call"))
                {
                    this.chips.Add(Chips);
                    intsadded = true;
                }
            }
            intsadded = false;
            if (bot1Chips <= 0 && !B1Fturn)
            {
                if (!intsadded)
                {
                    this.chips.Add(bot1Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot2Chips <= 0 && !B2Fturn)
            {
                if (!intsadded)
                {
                    this.chips.Add(bot2Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot3Chips <= 0 && !B3Fturn)
            {
                if (!intsadded)
                {
                    this.chips.Add(bot3Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot4Chips <= 0 && !B4Fturn)
            {
                if (!intsadded)
                {
                    this.chips.Add(bot4Chips);
                    intsadded = true;
                }
                intsadded = false;
            }
            if (bot5Chips <= 0 && !B5Fturn)
            {
                if (!intsadded)
                {
                    this.chips.Add(bot5Chips);
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
                    Chips += int.Parse(tbPot.Text);
                    tbChips.Text = Chips.ToString();
                    this.playerPanel.Visible = true;
                    MessageBox.Show("Player Wins");
                }
                if (index == 1)
                {
                    bot1Chips += int.Parse(tbPot.Text);
                    tbChips.Text = bot1Chips.ToString();
                    this.bot1Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }
                if (index == 2)
                {
                    bot2Chips += int.Parse(tbPot.Text);
                    tbChips.Text = bot2Chips.ToString();
                    this.bot2Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }
                if (index == 3)
                {
                    bot3Chips += int.Parse(tbPot.Text);
                    tbChips.Text = bot3Chips.ToString();
                    this.bot3Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }
                if (index == 4)
                {
                    bot4Chips += int.Parse(tbPot.Text);
                    tbChips.Text = bot4Chips.ToString();
                    this.bot4Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }
                if (index == 5)
                {
                    bot5Chips += int.Parse(tbPot.Text);
                    tbChips.Text = bot5Chips.ToString();
                    this.bot5Panel.Visible = true;
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
            this.playerPanel.Visible = false;
            this.bot1Panel.Visible = false;
            this.bot2Panel.Visible = false;
            this.bot3Panel.Visible = false;
            this.bot4Panel.Visible = false;
            this.bot5Panel.Visible = false;
            this.call = this.bigBlindValue; this.raise = 0;
            this.foldedPlayers = 5;
            this.type = 0;
            this.rounds = 0;
            this.b1Power = 0;
            this.b2Power = 0;
            this.b3Power = 0;
            this.b4Power = 0;
            this.b5Power = 0;
            this.pPower = 0;
            this.pType = -1;
            this.raise = 0;
            this.b1Type = -1;
            this.b2Type = -1;
            this.b3Type = -1;
            this.b4Type = -1;
            this.b5Type = -1;
            this.B1turn = false;
            this.B2turn = false;
            this.B3turn = false;
            this.B4turn = false;
            this.B5turn = false;
            this.B1Fturn = false;
            this.B2Fturn = false;
            this.B3Fturn = false;
            this.B4Fturn = false;
            this.B5Fturn = false;
            this.pFolded = false;
            this.b1Folded = false;
            this.b2Folded = false;
            this.b3Folded = false;
            this.b4Folded = false;
            this.b5Folded = false;
            this.PFturn = false;
            this.Pturn = true;
            this.restart = false;
            this.raising = false;
            this.pCall = 0;
            this.b1Call = 0;
            this.b2Call = 0;
            this.b3Call = 0;
            this.b4Call = 0;
            this.b5Call = 0;
            this.pRaise = 0;
            this.b1Raise = 0;
            this.b2Raise = 0;
            this.b3Raise = 0;
            this.b4Raise = 0;
            this.b5Raise = 0;
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
            this.tbPot.Text = "0";
            this.t = 60;
            this.up = 10000000;
            this.turnCount = 0;
            this.pStatus.Text = string.Empty;
            this.b1Status.Text = string.Empty;
            this.b2Status.Text = string.Empty;
            this.b3Status.Text = string.Empty;
            this.b4Status.Text = string.Empty;
            this.b5Status.Text = string.Empty;

            if (Chips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.a != 0)
                {
                    this.Chips = f2.a;
                    this.bot1Chips += f2.a;
                    this.bot2Chips += f2.a;
                    this.bot3Chips += f2.a;
                    this.bot4Chips += f2.a;
                    this.bot5Chips += f2.a;
                    this.PFturn = false;
                    this.Pturn = true;
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
            if (!pStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                Rules(0, 1, "Player", ref pType, ref pPower, PFturn);
            }

            if (!b1Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                Rules(2, 3, "Bot 1", ref b1Type, ref b1Power, B1Fturn);
            }

            if (!b2Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                Rules(4, 5, "Bot 2", ref b2Type, ref b2Power, B2Fturn);
            }

            if (!b3Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                Rules(6, 7, "Bot 3", ref b3Type, ref b3Power, B3Fturn);
            }

            if (!b4Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                Rules(8, 9, "Bot 4", ref b4Type, ref b4Power, B4Fturn);
            }

            if (!b5Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                Rules(10, 11, "Bot 5", ref b5Type, ref b5Power, B5Fturn);
            }

            this.Winner(this.pType, this.pPower, "Player", this.Chips, fixedLast);
            this.Winner(this.b1Type, this.b1Power, "Bot 1", this.bot1Chips, fixedLast);
            this.Winner(this.b2Type, this.b2Power, "Bot 2", this.bot2Chips, fixedLast);
            this.Winner(this.b3Type, this.b3Power, "Bot 3", this.bot3Chips, fixedLast);
            this.Winner(this.b4Type, this.b4Power, "Bot 4", this.bot4Chips, fixedLast);
            this.Winner(this.b5Type, this.b5Power, "Bot 5", this.bot5Chips, fixedLast);
        }

        void AI(int c1, int c2, ref int sChips, ref bool sTurn, ref bool sFTurn, Label sStatus, int name, double botPower, double botCurrent)
        {
            if (!sFTurn)
            {
                //TODO: consider switch case here
                if (botCurrent == -1)
                {
                    HighCard(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 0)
                {
                    PairTable(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 1)
                {
                    PairHand(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 2)
                {
                    TwoPair(ref sChips, ref sTurn, ref sFTurn, sStatus, botPower);
                }
                if (botCurrent == 3)
                {
                    ThreeOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 4)
                {
                    Straight(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 5 || botCurrent == 5.5)
                {
                    Flush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 6)
                {
                    FullHouse(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 7)
                {
                    FourOfAKind(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
                if (botCurrent == 8 || botCurrent == 9)
                {
                    StraightFlush(ref sChips, ref sTurn, ref sFTurn, sStatus, name, botPower);
                }
            }
            if (sFTurn)
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
            ref int sChips,
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
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }

            if (botPower <= 327 && botPower >= 321)//10  8
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
            }

            if (botPower < 321 && botPower >= 303)//7 2
            {
                Smooth(ref sChips, ref sTurn, ref sFTurn, sStatus, name, tCall, tRaise);
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

        private void Check(ref bool cTurn, Label cStatus)
        {
            cStatus.Text = "Check";
            cTurn = false;
            raising = false;
        }

        private void Call(ref int sChips, ref bool sTurn, Label sStatus)
        {
            raising = false;
            sTurn = false;
            sChips -= call;
            sStatus.Text = "Call " + call;
            tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
        }

        private void Raised(ref int sChips, ref bool sTurn, Label sStatus)
        {
            sChips -= Convert.ToInt32(this.raise);
            sStatus.Text = "raise " + this.raise;
            tbPot.Text = (int.Parse(tbPot.Text) + Convert.ToInt32(this.raise)).ToString();
            call = Convert.ToInt32(this.raise);
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
            if (call <= 0)
            {
                Check(ref sTurn, sStatus);
            }

            if (call > 0)
            {
                if (rnd == 1)
                {
                    if (call <= RoundN(sChips, n))
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
                    if (call <= RoundN(sChips, n1))
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
                    this.raise = call * 2;
                    Raised(ref sChips, ref sTurn, sStatus);
                }
                else
                {
                    if (this.raise <= RoundN(sChips, n))
                    {
                        this.raise = call * 2;
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
                if (call <= 0)
                {
                    Check(ref sTurn, sStatus);
                }

                if (call > 0)
                {
                    if (call >= RoundN(sChips, n1))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (this.raise > RoundN(sChips, n))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n) && call <= RoundN(sChips, n1))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (this.raise <= RoundN(sChips, n) && this.raise >= (RoundN(sChips, n)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (this.raise <= (RoundN(sChips, n)) / 2)
                        {
                            if (this.raise > 0)
                            {
                                this.raise = RoundN(sChips, n);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                this.raise = call * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }
                    }
                }
            }

            if (rounds >= 2)
            {
                if (call > 0)
                {
                    if (call >= RoundN(sChips, n1 - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (this.raise > RoundN(sChips, n - rnd))
                    {
                        Fold(ref sTurn, ref sFTurn, sStatus);
                    }

                    if (!sFTurn)
                    {
                        if (call >= RoundN(sChips, n - rnd) && call <= RoundN(sChips, n1 - rnd))
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (this.raise <= RoundN(sChips, n - rnd) &&
                            this.raise >= (RoundN(sChips, n - rnd)) / 2)
                        {
                            Call(ref sChips, ref sTurn, sStatus);
                        }

                        if (this.raise <= (RoundN(sChips, n - rnd)) / 2)
                        {
                            if (this.raise > 0)
                            {
                                this.raise = RoundN(sChips, n - rnd);
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                            else
                            {
                                this.raise = call * 2;
                                Raised(ref sChips, ref sTurn, sStatus);
                            }
                        }
                    }
                }

                if (call <= 0)
                {
                    this.raise = RoundN(sChips, r - rnd);
                    Raised(ref sChips, ref sTurn, sStatus);
                }
            }

            if (sChips <= 0)
            {
                sFTurn = true;
            }
        }

        void Smooth(
            ref int botChips, 
            ref bool botTurn, 
            ref bool botFTurn,
            Label botStatus, 
            int name,
            int n, 
            int r)
        {
            Random rand = new Random();
            int rnd = rand.Next(1, 3);
            if (call <= 0)
            {
                Check(ref botTurn, botStatus);
            }
            else
            {
                if (call >= RoundN(botChips, n))
                {
                    if (botChips > call)
                    {
                        Call(ref botChips, ref botTurn, botStatus);
                    }
                    else if (botChips <= call)
                    {
                        raising = false;
                        botTurn = false;
                        botChips = 0;
                        botStatus.Text = "Call " + botChips;
                        tbPot.Text = (int.Parse(tbPot.Text) + botChips).ToString();
                    }
                }
                else
                {
                    if (this.raise > 0)
                    {
                        if (botChips >= this.raise * 2)
                        {
                            this.raise *= 2;
                            Raised(ref botChips, ref botTurn, botStatus);
                        }
                        else
                        {
                            Call(ref botChips, ref botTurn, botStatus);
                        }
                    }
                    else
                    {
                        this.raise = call * 2;
                        Raised(ref botChips, ref botTurn, botStatus);
                    }
                }
            }

            if (botChips <= 0)
            {
                botFTurn = true;
            }
        }

        #region UI
        private async void TimerTick(object sender, object e)
        {
            if (pbTimer.Value <= 0)
            {
                PFturn = true;
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
            if (Chips <= 0)
            {
                tbChips.Text = "Chips : 0";
            }

            if (bot1Chips <= 0)
            {
                tbBotChips1.Text = "Chips : 0";
            }

            if (bot2Chips <= 0)
            {
                tbBotChips2.Text = "Chips : 0";
            }

            if (bot3Chips <= 0)
            {
                tbBotChips3.Text = "Chips : 0";
            }

            if (bot4Chips <= 0)
            {
                tbBotChips4.Text = "Chips : 0";
            }

            if (bot5Chips <= 0)
            {
                tbBotChips5.Text = "Chips : 0";
            }

            tbChips.Text = "Chips : " + Chips;
            tbBotChips1.Text = "Chips : " + bot1Chips;
            tbBotChips2.Text = "Chips : " + bot2Chips;
            tbBotChips3.Text = "Chips : " + bot3Chips;
            tbBotChips4.Text = "Chips : " + bot4Chips;
            tbBotChips5.Text = "Chips : " + bot5Chips;

            if (Chips <= 0)
            {
                Pturn = false;
                PFturn = true;
                bCall.Enabled = false;
                bRaise.Enabled = false;
                bFold.Enabled = false;
                bCheck.Enabled = false;
            }

            if (up > 0)
            {
                up--;
            }

            if (Chips >= call)
            {
                bCall.Text = "Call " + call.ToString();
            }
            else
            {
                bCall.Text = "All in";
                bRaise.Enabled = false;
            }

            if (call > 0)
            {
                bCheck.Enabled = false;
            }

            if (call <= 0)
            {
                bCheck.Enabled = true;
                bCall.Text = "Call";
                bCall.Enabled = false;
            }

            if (Chips <= 0)
            {
                bRaise.Enabled = false;
            }

            int parsedValue;

            if (tbRaise.Text != "" && int.TryParse(tbRaise.Text, out parsedValue))
            {
                if (Chips <= int.Parse(tbRaise.Text))
                {
                    bRaise.Text = "All in";
                }
                else
                {
                    bRaise.Text = "raise";
                }
            }

            if (Chips < call)
            {
                bRaise.Enabled = false;
            }
        }

        private async void bFold_Click(object sender, EventArgs e)
        {
            pStatus.Text = "Fold";
            Pturn = false;
            PFturn = true;
            await Turns();
        }

        private async void bCheck_Click(object sender, EventArgs e)
        {
            if (call <= 0)
            {
                Pturn = false;
                pStatus.Text = "Check";
            }
            else
            {
                //pStatus.Text = "All in " + Chips;

                bCheck.Enabled = false;
            }
            await Turns();
        }

        private async void bCall_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref pType, ref pPower, PFturn);
            if (Chips >= call)
            {
                Chips -= call;
                tbChips.Text = "Chips : " + Chips.ToString();
                if (tbPot.Text != "")
                {
                    tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
                }
                else
                {
                    tbPot.Text = call.ToString();
                }
                Pturn = false;
                pStatus.Text = "Call " + call;
                pCall = call;
            }
            else if (Chips <= call && call > 0)
            {
                tbPot.Text = (int.Parse(tbPot.Text) + Chips).ToString();
                pStatus.Text = "All in " + Chips;
                Chips = 0;
                tbChips.Text = "Chips : " + Chips.ToString();
                Pturn = false;
                bFold.Enabled = false;
                pCall = Chips;
            }

            await Turns();
        }

        private async void bRaise_Click(object sender, EventArgs e)
        {
            Rules(0, 1, "Player", ref pType, ref pPower, PFturn);
            int parsedValue;
            if (tbRaise.Text != "" && int.TryParse(tbRaise.Text, out parsedValue))
            {
                if (Chips > call)
                {
                    if (this.raise * 2 > int.Parse(tbRaise.Text))
                    {
                        tbRaise.Text = (this.raise * 2).ToString();
                        MessageBox.Show("You must raise atleast twice as the current raise !");
                        return;
                    }
                    else
                    {
                        if (Chips >= int.Parse(tbRaise.Text))
                        {
                            call = int.Parse(tbRaise.Text);
                            this.raise = int.Parse(tbRaise.Text);
                            pStatus.Text = "raise " + call.ToString();
                            tbPot.Text = (int.Parse(tbPot.Text) + call).ToString();
                            bCall.Text = "Call";
                            Chips -= int.Parse(tbRaise.Text);
                            raising = true;
                            last = 0;
                            pRaise = Convert.ToInt32(this.raise);
                        }
                        else
                        {
                            call = Chips;
                            this.raise = Chips;
                            tbPot.Text = (int.Parse(tbPot.Text) + Chips).ToString();
                            pStatus.Text = "raise " + call.ToString();
                            Chips = 0;
                            raising = true;
                            last = 0;
                            pRaise = Convert.ToInt32(this.raise);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("This is a number only field");
                return;
            }

            Pturn = false;
            await Turns();
        }

        //TODO: validate chips are integer
        private void bAddChipsClick(object sender, EventArgs e)
        {
            if (tbAdd.Text == "")
            {

            }
            else
            {
                Chips += int.Parse(tbAdd.Text);
                bot1Chips += int.Parse(tbAdd.Text);
                bot2Chips += int.Parse(tbAdd.Text);
                bot3Chips += int.Parse(tbAdd.Text);
                bot4Chips += int.Parse(tbAdd.Text);
                bot5Chips += int.Parse(tbAdd.Text);
            }

            tbChips.Text = "Chips : " + Chips;
        }

        private void bOptions_Click(object sender, EventArgs e)
        {
            this.tbBigBlind.Text = this.bigBlindValue.ToString();
            this.tbSmallBlind.Text = this.smallBlindValue.ToString();

            if (this.tbBigBlind.Visible == false)
            {
                this.tbBigBlind.Visible = true;
                this.tbSmallBlind.Visible = true;
                bBB.Visible = true;
                bSB.Visible = true;
            }
            else
            {
                this.tbBigBlind.Visible = false;
                this.tbSmallBlind.Visible = false;
                bBB.Visible = false;
                bSB.Visible = false;
            }
        }

        //TODO: Small and big blind have similar logic, extract in different method
        private void bSmallBlindClick(object sender, EventArgs e)
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

        private void bBigBlindClick(object sender, EventArgs e)
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