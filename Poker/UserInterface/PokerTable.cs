using System;
namespace Poker.UserInterface
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Forms;
    using Poker.Data;
    using Poker.Interfaces;
    using Poker.Models;
    using Poker.Models.Players;
    using Poker.Utility;
    using Type = Poker.Type;

    public partial class PokerTable : Form
    {
        #region Variables

        private readonly IAssertHandType assertHandType;
        private readonly IHandType handType;
        private readonly IPlayer human;
        // TODO: put all bots in one list!
        private readonly IList<IBot> bots;
        //private readonly IBot firstBot;
        //private readonly IBot secondBot;
        //private readonly IBot thirdBot;
        //private readonly IBot fourthBot;
        //private readonly IBot fifthBot;
        private readonly IDatabase pokerDatabase;
        private readonly Image[] deckImages;
        private readonly PictureBox[] cardsPictureBoxList;
        private readonly Timer timer = new Timer();
        private readonly Timer updates = new Timer();
        private string[] cardsImageLocation;
        private int[] reservedGameCardsIndeces;

        //private ProgressBar asd = new ProgressBar();
        //private int nm;
        private int neededChipsToCall;
        private int foldedBotsCount;
        private double type;
        private int rounds;
        private int raise;
        private bool chipsAreAdded;
        private bool changed;
        private int height;
        private int width;
        private int winners;

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

        private int secondsToMakeTurn;
        private int cardNumber;
        private int bigBlindValue;
        private int smallBlindValue;
        private int up = 10000000;
        private int turnCount;
        #endregion

        public PokerTable()
        {
            this.handType = new HandType();
            this.assertHandType = new AssertHandType();
            this.pokerDatabase = new PokerDatabase();
            this.human = new Human("Player");
            this.bots = new List<IBot>(5);
            this.cardsImageLocation = Directory.GetFiles(
                "..\\..\\Resources\\Assets\\Cards",
                "*.png",
                SearchOption.TopDirectoryOnly);
            this.reservedGameCardsIndeces = new int[Constants.NeededCardsFromDeck];
            this.deckImages = new Image[Constants.CardsInADeck];
            this.cardsPictureBoxList = new PictureBox[Constants.CardsInADeck];
            //pokerDatabasePlayersGameStatus.Add(humanOutOfChips); pokerDatabasePlayersGameStatus.Add(firstBotOutOfChips); pokerDatabasePlayersGameStatus.Add(secondBotOutOfChips); pokerDatabasePlayersGameStatus.Add(thirdBotOutOfChips); pokerDatabasePlayersGameStatus.Add(fourthBotOutOfChips); pokerDatabasePlayersGameStatus.Add(fifthBotOutOfChips);
            this.bigBlindValue = Constants.MinBigBlindValue;
            this.smallBlindValue = Constants.MinSmallBlindValue;
            this.neededChipsToCall = this.bigBlindValue;
            this.secondsToMakeTurn = Constants.DefaultSecondsToMakeTurn;
            this.foldedBotsCount = Constants.DefaultBotsCount;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.updates.Start();
            this.bots = new List<IBot>();
            this.PopulateBots();
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
            this.PopulateBotChips();
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

        private void PopulateBots()
        {
            bots.Add(new Bot("Bot 1", 2, 420, 15, AnchorStyles.Bottom, AnchorStyles.Left));
            bots.Add(new Bot("Bot 2", 4, 65, 75, AnchorStyles.Top, AnchorStyles.Left));
            bots.Add(new Bot("Bot 3", 6, 25, 590, AnchorStyles.Top, 0));
            bots.Add(new Bot("Bot 4", 8, 65, 1115, AnchorStyles.Top, AnchorStyles.Right));
            bots.Add(new Bot("Bot 5", 10, 420, 1160, AnchorStyles.Bottom, AnchorStyles.Right));
        }

        private void PopulateBotChips()
        {
            for (int bot = 0; bot < this.bots.Count; bot++)
            {
                switch (bot)
                {
                    case 0:
                        this.txtBoxFirstBotChips.Text = "Chips : " + this.bots[bot].Chips;
                        break;
                    case 1:
                        this.txtBoxSecondBotChips.Text = "Chips : " + this.bots[bot].Chips;
                        break;
                    case 2:
                        this.txtBoxThirdBotChips.Text = "Chips : " + this.bots[bot].Chips;
                        break;
                    case 3:
                        this.txtBoxFourthBotChips.Text = "Chips : " + this.bots[bot].Chips;
                        break;
                    case 4:
                        this.txtBoxFifthBotChips.Text = "Chips : " + this.bots[bot].Chips;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException("No such bot found!");
                }
            }
        }

        private async Task Shuffle()
        {
            this.pokerDatabase.PlayersGameStatus.Add(this.human.OutOfChips);
            foreach (var bot in this.bots)
            {
                this.pokerDatabase.PlayersGameStatus.Add(bot.OutOfChips);
            }
            //this.pokerDatabase.PlayersGameStatus.Add(this.bots[0].OutOfChips);
            //this.pokerDatabase.PlayersGameStatus.Add(this.bots[1].OutOfChips);
            //this.pokerDatabase.PlayersGameStatus.Add(this.bots[2].OutOfChips);
            //this.pokerDatabase.PlayersGameStatus.Add(this.bots[3].OutOfChips);
            //this.pokerDatabase.PlayersGameStatus.Add(this.bots[4].OutOfChips);
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
            for (int cardLocationIndex = Constants.CardsInADeck; cardLocationIndex > 0; cardLocationIndex--)
            {
                //Swaps two cards locations from the desk, taking one random and replacing it with the 
                //card location from the loop index
                int randomCardIndex = randomCardLocation.Next(cardLocationIndex);
                string oldCardLocation = this.cardsImageLocation[randomCardIndex];
                this.cardsImageLocation[randomCardIndex] = this.cardsImageLocation[cardLocationIndex - 1];
                this.cardsImageLocation[cardLocationIndex - 1] = oldCardLocation;
            }

            for (this.cardNumber = 0; this.cardNumber < Constants.NeededCardsFromDeck; this.cardNumber++)
            {
                //PERFORMANCE: Unnecessary loop for removing parts of the image location
                this.deckImages[cardNumber] = Image.FromFile(this.cardsImageLocation[cardNumber]);

                int lastSlashIndex = this.cardsImageLocation[this.cardNumber].LastIndexOf("\\");
                int lastDotIndex = this.cardsImageLocation[this.cardNumber].LastIndexOf(".");
                this.cardsImageLocation[this.cardNumber] = this.cardsImageLocation[this.cardNumber]
                    .Substring(lastSlashIndex + 1, lastDotIndex - lastSlashIndex - 1);

                this.reservedGameCardsIndeces[this.cardNumber] = int.Parse(this.cardsImageLocation[this.cardNumber]) - 1;
                this.cardsPictureBoxList[this.cardNumber] = new PictureBox
                {
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Height = 130,
                    Width = 80
                };

                this.Controls.Add(this.cardsPictureBoxList[this.cardNumber]);
                this.cardsPictureBoxList[this.cardNumber].Name = "pb" + this.cardNumber.ToString();
                await Task.Delay(200);

                //TODO: Should this region be in the for loop at all?
                #region Throwing Cards
                if (this.cardNumber < 2)
                {
                    if (this.cardsPictureBoxList[0].Tag != null)
                    {
                        this.cardsPictureBoxList[1].Tag = this.reservedGameCardsIndeces[1];
                    }

                    this.cardsPictureBoxList[0].Tag = this.reservedGameCardsIndeces[0];
                    this.cardsPictureBoxList[this.cardNumber].Image = this.deckImages[this.cardNumber];
                    this.cardsPictureBoxList[this.cardNumber].Anchor = (AnchorStyles.Bottom);
                    //cardsPictureBoxList[i].Dock = DockStyle.Top;
                    this.cardsPictureBoxList[this.cardNumber].Location = new Point(horizontal, vertical);
                    horizontal += this.cardsPictureBoxList[this.cardNumber].Width;
                    this.Controls.Add(this.human.Panel);
                    this.human.InitializePanel(new Point(
                        this.cardsPictureBoxList[0].Left - 10,
                        this.cardsPictureBoxList[0].Top - 10));
                }
                
                // solved the problem with the code repetition(still need to check the code for quality!)
                foreach (var bot in this.bots)
                {
                    this.DealCardsForBots(bot, this.cardNumber, backImage, ref check, ref horizontal, ref vertical);
                }

                if (this.cardNumber >= 12)
                {
                    this.cardsPictureBoxList[12].Tag = this.reservedGameCardsIndeces[12];
                    if (this.cardNumber > 12)
                    {
                        this.cardsPictureBoxList[13].Tag = this.reservedGameCardsIndeces[13];
                    }

                    if (this.cardNumber > 13)
                    {
                        this.cardsPictureBoxList[14].Tag = this.reservedGameCardsIndeces[14];
                    }

                    if (this.cardNumber > 14)
                    {
                        this.cardsPictureBoxList[15].Tag = this.reservedGameCardsIndeces[15];
                    }

                    if (this.cardNumber > 15)
                    {
                        this.cardsPictureBoxList[16].Tag = this.reservedGameCardsIndeces[16];

                    }

                    if (!check)
                    {
                        horizontal = 410;
                        vertical = 265;
                    }

                    check = true;
                    if (this.cardsPictureBoxList[this.cardNumber] != null)
                    {
                        this.cardsPictureBoxList[this.cardNumber].Anchor = AnchorStyles.None;
                        this.cardsPictureBoxList[this.cardNumber].Image = backImage;
                        //cardsPictureBoxList[i].Image = deckImages[i];
                        this.cardsPictureBoxList[this.cardNumber].Location = new Point(horizontal, vertical);
                        horizontal += 110;
                    }
                }

                #endregion
                
                #region CheckForDefeatedBots
                foreach (var bot in this.bots)
                {
                    this.CheckForDefeatedBot(bot, this.cardNumber);
                }

                if (this.cardNumber == 16)
                {
                    if (!this.restart)
                    {
                        this.MaximizeBox = true;
                        this.MinimizeBox = true;
                    }
                    this.timer.Start();
                }
                #endregion
            }

            //TODO: GameOver state
            #region GameOverState?
            if (this.foldedBotsCount == 5)
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
                this.foldedBotsCount = 5;
            }

            if (this.cardNumber == 17)
            {
                this.buttonRaise.Enabled = true;
                this.buttonCall.Enabled = true;
                this.buttonRaise.Enabled = true;
                this.buttonRaise.Enabled = true;
                this.buttonFold.Enabled = true;
            }
            #endregion
        }

        private void DealCardsForBots(IBot bot, int cardNumber, Bitmap backImage, 
            ref bool check, ref int horizontal, ref int vertical)
        {
            if (bot.Chips > 0)
            {
                this.foldedBotsCount--;
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
                    //cardsPictureBoxList[i].Image = deckImages[i];
                    this.cardsPictureBoxList[cardNumber].Location = new Point(horizontal, vertical);
                    horizontal += this.cardsPictureBoxList[cardNumber].Width;
                    this.cardsPictureBoxList[cardNumber].Visible = true;
                    this.Controls.Add(bot.Panel);
                    bot.InitializePanel(new Point(
                        this.cardsPictureBoxList[bot.StartCard].Left - 10,
                        this.cardsPictureBoxList[bot.StartCard].Top - 10));

                    if (cardNumber == bot.StartCard + 1)
                    {
                        check = false;
                    }
                }
            }
        }

        private void CheckForDefeatedBot(IBot bot, int cardNumber)
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
            // TODO: avoid code repetition in this region
            if (!this.human.OutOfChips)
            {
                if (this.human.CanMakeTurn)
                {
                    this.FixCall(this.humanStatus, this.human.Call, this.human.Raise, 1);
                    //MessageBox.Show("Player's turn");
                    this.pbTimer.Visible = true;
                    this.pbTimer.Value = 1000;
                    this.secondsToMakeTurn = 60;
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
                await this.AllIn();
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

                await this.CheckRaise(0, 0);
                pbTimer.Visible = false;
                this.buttonRaise.Enabled = false;
                this.buttonCall.Enabled = false;
                this.buttonRaise.Enabled = false;
                this.buttonRaise.Enabled = false;
                this.buttonFold.Enabled = false;
                this.timer.Stop();
                this.bots[0].CanMakeTurn = true;
                this.ProceedWithBotsTurns();
                #endregion

                await this.AllIn();
                if (!this.restart)
                {
                    await this.Turns();
                }
                this.restart = false;
            }
        }

        // TODO: in progress, create a method to proceed the bots' turns
        private async void ProceedWithBotsTurns()
        {
            for (int botNumber = 0; botNumber < this.bots.Count; botNumber++)
            {
                if (!this.bots[botNumber].OutOfChips)
                {
                    if (this.bots[botNumber].CanMakeTurn)
                    {
                        this.FixCall(this.bots[botNumber].Status, this.bots[botNumber].Call, this.bots[botNumber].Raise, 1);
                        this.FixCall(this.bots[botNumber].Status, this.bots[botNumber].Call, this.bots[botNumber].Raise, 2);
                        this.Rules(this.bots[botNumber].StartCard, this.bots[botNumber].StartCard + 1, "Bot " + (botNumber + 1), this.bots[botNumber]);
                        MessageBox.Show("Bot "+ (botNumber + 1) +"'s turn");
                        this.AI(this.bots[botNumber].StartCard, this.bots[botNumber].StartCard + 1, this.bots[botNumber].Status, botNumber, this.bots[botNumber]);
                        this.turnCount++;
                        this.last = botNumber + 1;
                        this.bots[botNumber].CanMakeTurn = false;
                        if (botNumber < this.bots.Count - 1)
                        {
                            this.bots[botNumber].CanMakeTurn = true;
                        }
                    }
                }

                if (this.bots[botNumber].OutOfChips && !this.bots[botNumber].Folded)
                {
                    this.pokerDatabase.PlayersGameStatus.RemoveAt(botNumber + 1);
                    this.pokerDatabase.PlayersGameStatus.Insert(botNumber + 1, null);
                    this.maxLeft--;
                    this.bots[botNumber].Folded = true;
                }

                if (this.bots[botNumber].OutOfChips || !this.bots[botNumber].CanMakeTurn)
                {
                    await this.CheckRaise(botNumber + 1, botNumber + 1);
                    if (botNumber < this.bots.Count - 1)
                    {
                        this.bots[botNumber + 1].CanMakeTurn = true;
                    }
                    else
                    {
                        this.human.CanMakeTurn = true;
                    }
                }
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
                        this.assertHandType.rPairFromHand(player, index, this.pokerDatabase.Win, ref this.sorted, ref this.reservedGameCardsIndeces);

                        this.assertHandType.rPairTwoPair(player, index, this.pokerDatabase.Win, ref this.sorted, ref this.reservedGameCardsIndeces);

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
                    this.cardsPictureBoxList[j].Image = this.deckImages[j];
                }
            }

            if (player.Type == this.sorted.Current)
            {
                if (player.Power == this.sorted.Power)
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
                // TODO: avoid code repetition!
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
                        this.bots[0].Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.txtBoxFirstBotChips.Text = this.bots[0].Chips.ToString();
                        //bot1Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 2"))
                    {
                        this.bots[1].Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.txtBoxSecondBotChips.Text = this.bots[1].Chips.ToString();
                        //bot2Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 3"))
                    {
                        this.bots[2].Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.txtBoxThirdBotChips.Text = this.bots[2].Chips.ToString();
                        //bot3Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 4"))
                    {
                        this.bots[3].Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.txtBoxFourthBotChips.Text = this.bots[3].Chips.ToString();
                        //bot4Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 5"))
                    {
                        this.bots[4].Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.txtBoxFifthBotChips.Text = this.bots[4].Chips.ToString();
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
                        this.bots[0].Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot1Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 2"))
                    {
                        this.bots[1].Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot2Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 3"))
                    {
                        this.bots[2].Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot3Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 4"))
                    {
                        this.bots[3].Chips += int.Parse(this.potStatus.Text);
                        //await Finish(1);
                        //bot4Panel.Visible = true;
                    }

                    if (this.pokerDatabase.CheckWinners.Contains("Bot 5"))
                    {
                        this.bots[4].Chips += int.Parse(this.potStatus.Text);
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
                            this.humanStatus.Text = string.Empty;
                        }
                        // TODO: avoid code repetition!
                        if (!this.bots[0].OutOfChips)
                        {
                            this.bots[0].Status.Text = string.Empty;
                        }

                        if (!this.bots[1].OutOfChips)
                        {
                            this.bots[1].Status.Text = string.Empty;
                        }

                        if (!this.bots[2].OutOfChips)
                        {
                            this.bots[2].Status.Text = string.Empty;
                        }

                        if (!this.bots[3].OutOfChips)
                        {
                            this.bots[3].Status.Text = string.Empty;
                        }

                        if (!this.bots[4].OutOfChips)
                        {
                            this.bots[4].Status.Text = string.Empty;
                        }

                    }
                }
            }

            if (this.rounds == this.flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (this.cardsPictureBoxList[j].Image != this.deckImages[j])
                    {
                        this.ResetPlayersStats(j);
                    }
                }
            }

            if (this.rounds == this.turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (this.cardsPictureBoxList[j].Image != this.deckImages[j])
                    {
                        this.ResetPlayersStats(j);
                    }
                }
            }

            if (this.rounds == this.river)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (this.cardsPictureBoxList[j].Image != this.deckImages[j])
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

                if (!this.bots[0].Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 1";
                    this.Rules(2, 3, "Bot 1", this.bots[0]);
                }

                if (!this.bots[1].Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 2";
                    this.Rules(4, 5, "Bot 2", this.bots[1]);
                }

                if (!this.bots[2].Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 3";
                    this.Rules(6, 7, "Bot 3", this.bots[2]);
                }

                if (!this.bots[3].Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 4";
                    this.Rules(8, 9, "Bot 4", this.bots[3]);
                }

                if (!this.bots[4].Status.Text.Contains("Fold"))
                {
                    fixedLast = "Bot 5";
                    this.Rules(10, 11, "Bot 5", this.bots[4]);
                }

                //TODO: COnsider passing the entire player object
                //TODO: COde repetition
                this.Winner(this.human, fixedLast);
                this.Winner(this.bots[0], fixedLast);
                this.Winner(this.bots[1], fixedLast);
                this.Winner(this.bots[2], fixedLast);
                this.Winner(this.bots[3], fixedLast);
                this.Winner(this.bots[4], fixedLast);

                this.restart = true;

                this.human.CanMakeTurn = true;
                this.human.OutOfChips = false;
                this.bots[0].OutOfChips = false;
                this.bots[1].OutOfChips = false;
                this.bots[2].OutOfChips = false;
                this.bots[3].OutOfChips = false;
                this.bots[4].OutOfChips = false;
                if (this.human.Chips <= 0)
                {
                    AddChips chipAdder = new AddChips();
                    chipAdder.ShowDialog();
                    if (chipAdder.chipsAmount != 0)
                    {
                        this.human.Chips = chipAdder.chipsAmount;
                        this.bots[0].Chips += chipAdder.chipsAmount;
                        this.bots[1].Chips += chipAdder.chipsAmount;
                        this.bots[2].Chips += chipAdder.chipsAmount;
                        this.bots[3].Chips += chipAdder.chipsAmount;
                        this.bots[4].Chips += chipAdder.chipsAmount;
                        this.human.OutOfChips = false;
                        this.human.CanMakeTurn = true;
                        this.buttonRaise.Enabled = true;
                        this.buttonFold.Enabled = true;
                        this.buttonCheck.Enabled = true;
                        this.buttonRaise.Text = "Raise";
                    }
                }

                this.human.Panel.Visible = false;
                this.bots[0].Panel.Visible = false;
                this.bots[1].Panel.Visible = false;
                this.bots[2].Panel.Visible = false;
                this.bots[3].Panel.Visible = false;
                this.bots[4].Panel.Visible = false;
                this.human.Call = 0;

                this.human.Raise = 0;
                this.bots[0].Call = 0;
                this.bots[0].Raise = 0;
                this.bots[1].Call = 0;
                this.bots[1].Raise = 0;
                this.bots[2].Call = 0;
                this.bots[2].Raise = 0;
                this.bots[3].Call = 0;
                this.bots[3].Raise = 0;
                this.bots[4].Call = 0;
                this.bots[4].Raise = 0;
                this.last = 0;
                this.neededChipsToCall = this.bigBlindValue;
                this.raise = 0;
                this.cardsImageLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                this.pokerDatabase.PlayersGameStatus.Clear();
                this.rounds = 0;
                this.type = 0;

                this.human.Power = 0;
                this.bots[0].Power = 0;
                this.bots[1].Power = 0;
                this.bots[2].Power = 0;
                this.bots[3].Power = 0;
                this.bots[4].Power = 0;

                this.bots[0].Type = -1;
                this.bots[1].Type = -1;
                this.bots[2].Type = -1;
                this.bots[3].Type = -1;
                this.bots[4].Type = -1;
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
                await this.Shuffle();
                await this.Turns();
            }
        }

        private void ResetPlayersStats(int j)
        {
            this.cardsPictureBoxList[j].Image = this.deckImages[j];
            this.human.Call = 0;
            this.human.Raise = 0;
            this.bots[0].Call = 0;
            this.bots[0].Raise = 0;
            this.bots[1].Call = 0;
            this.bots[1].Raise = 0;
            this.bots[2].Call = 0;
            this.bots[2].Raise = 0;
            this.bots[3].Call = 0;
            this.bots[3].Raise = 0;
            this.bots[4].Call = 0;
            this.bots[4].Raise = 0;
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

            if (this.bots[0].Chips <= 0 && !this.bots[0].OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.pokerDatabase.Chips.Add(this.bots[0].Chips);
                    this.chipsAreAdded = true;
                }

                this.chipsAreAdded = false;
            }

            if (this.bots[1].Chips <= 0 && !this.bots[1].OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.pokerDatabase.Chips.Add(this.bots[1].Chips);
                    this.chipsAreAdded = true;
                }

                this.chipsAreAdded = false;
            }

            if (this.bots[2].Chips <= 0 && !this.bots[2].OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.pokerDatabase.Chips.Add(this.bots[2].Chips);
                    this.chipsAreAdded = true;
                }

                this.chipsAreAdded = false;
            }

            if (this.bots[3].Chips <= 0 && !this.bots[3].OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.pokerDatabase.Chips.Add(this.bots[3].Chips);
                    this.chipsAreAdded = true;
                }

                this.chipsAreAdded = false;
            }

            if (this.bots[4].Chips <= 0 && !this.bots[4].OutOfChips)
            {
                if (!this.chipsAreAdded)
                {
                    this.pokerDatabase.Chips.Add(this.bots[4].Chips);
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
                    this.bots[0].Chips += int.Parse(this.potStatus.Text);
                    this.txtBoxHumanChips.Text = this.bots[0].Chips.ToString();
                    this.bots[0].Panel.Visible = true;
                    MessageBox.Show("Bot 1 Wins");
                }

                if (index == 2)
                {
                    this.bots[1].Chips += int.Parse(this.potStatus.Text);
                    this.txtBoxHumanChips.Text = this.bots[1].Chips.ToString();
                    this.bots[1].Panel.Visible = true;
                    MessageBox.Show("Bot 2 Wins");
                }

                if (index == 3)
                {
                    this.bots[2].Chips += int.Parse(this.potStatus.Text);
                    this.txtBoxHumanChips.Text = this.bots[2].Chips.ToString();
                    this.bots[2].Panel.Visible = true;
                    MessageBox.Show("Bot 3 Wins");
                }

                if (index == 4)
                {
                    this.bots[3].Chips += int.Parse(this.potStatus.Text);
                    this.txtBoxHumanChips.Text = this.bots[3].Chips.ToString();
                    this.bots[3].Panel.Visible = true;
                    MessageBox.Show("Bot 4 Wins");
                }

                if (index == 5)
                {
                    this.bots[4].Chips += int.Parse(this.potStatus.Text);
                    this.txtBoxHumanChips.Text = this.bots[4].Chips.ToString();
                    this.bots[4].Panel.Visible = true;
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
                this.FixWinners();
            }

            this.human.Panel.Visible = false;
            this.bots[0].Panel.Visible = false;
            this.bots[1].Panel.Visible = false;
            this.bots[2].Panel.Visible = false;
            this.bots[3].Panel.Visible = false;
            this.bots[4].Panel.Visible = false;

            this.neededChipsToCall = this.bigBlindValue;
            this.raise = 0;
            this.foldedBotsCount = 5;
            this.type = 0;
            this.rounds = 0;

            this.bots[0].Power = 0;
            this.bots[1].Power = 0;
            this.bots[2].Power = 0;
            this.bots[3].Power = 0;
            this.bots[4].Power = 0;
            this.human.Power = 0;

            this.raise = 0;

            this.human.Type = -1;
            this.bots[0].Type = -1;
            this.bots[1].Type = -1;
            this.bots[2].Type = -1;
            this.bots[3].Type = -1;
            this.bots[4].Type = -1;

            this.bots[0].CanMakeTurn = false;
            this.bots[1].CanMakeTurn = false;
            this.bots[2].CanMakeTurn = false;
            this.bots[3].CanMakeTurn = false;
            this.bots[4].CanMakeTurn = false;
            this.bots[0].OutOfChips = false;
            this.bots[1].OutOfChips = false;
            this.bots[2].OutOfChips = false;
            this.bots[3].OutOfChips = false;
            this.bots[4].OutOfChips = false;
            this.human.Folded = false;
            this.bots[0].Folded = false;
            this.bots[1].Folded = false;
            this.bots[2].Folded = false;
            this.bots[3].Folded = false;
            this.bots[4].Folded = false;
            this.human.OutOfChips = false;
            this.human.CanMakeTurn = true;
            this.restart = false;
            this.raising = false;
            this.human.Call = 0;
            this.bots[0].Call = 0;
            this.bots[1].Call = 0;
            this.bots[2].Call = 0;
            this.bots[3].Call = 0;
            this.bots[4].Call = 0;
            this.human.Raise = 0;
            this.bots[0].Raise = 0;
            this.bots[1].Raise = 0;
            this.bots[2].Raise = 0;
            this.bots[3].Raise = 0;
            this.bots[4].Raise = 0;
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
            this.secondsToMakeTurn = 60;
            this.up = 10000000;
            this.turnCount = 0;
            this.humanStatus.Text = string.Empty;
            this.bots[0].Status.Text = string.Empty;
            this.bots[1].Status.Text = string.Empty;
            this.bots[2].Status.Text = string.Empty;
            this.bots[3].Status.Text = string.Empty;
            this.bots[4].Status.Text = string.Empty;

            if (this.human.Chips <= 0)
            {
                AddChips f2 = new AddChips();
                f2.ShowDialog();
                if (f2.chipsAmount != 0)
                {
                    this.human.Chips = f2.chipsAmount;
                    this.bots[4].Chips += f2.chipsAmount;
                    this.bots[1].Chips += f2.chipsAmount;
                    this.bots[2].Chips += f2.chipsAmount;
                    this.bots[3].Chips += f2.chipsAmount;
                    this.bots[4].Chips += f2.chipsAmount;
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

            await this.Shuffle();
            //await Turns();
        }

        private void FixWinners()
        {
            this.pokerDatabase.Win.Clear();
            this.sorted.Current = 0;
            this.sorted.Power = 0;
            string fixedLast = "qwerty";
            if (!this.humanStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                this.Rules(0, 1, "Player", this.human);
            }

            if (!this.bots[0].Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 1";
                this.Rules(2, 3, "Bot 1", this.bots[0]);
            }

            if (!this.bots[1].Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 2";
                this.Rules(4, 5, "Bot 2", this.bots[1]);
            }

            if (!this.bots[2].Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 3";
                this.Rules(6, 7, "Bot 3", this.bots[2]);
            }

            if (!this.bots[3].Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 4";
                this.Rules(8, 9, "Bot 4", this.bots[3]);
            }

            if (!this.bots[4].Status.Text.Contains("Fold"))
            {
                fixedLast = "Bot 5";
                this.Rules(10, 11, "Bot 5", this.bots[4]);
            }

            //TODO: code repetition
            this.Winner(this.human, fixedLast);
            this.Winner(this.bots[0], fixedLast);
            this.Winner(this.bots[1], fixedLast);
            this.Winner(this.bots[2], fixedLast);
            this.Winner(this.bots[3], fixedLast);
            this.Winner(this.bots[4], fixedLast);
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

            if (this.secondsToMakeTurn > 0)
            {
                this.secondsToMakeTurn--;
                pbTimer.Value = (this.secondsToMakeTurn / 6) * 100;
            }
        }

        private void UpdateTick(object sender, object e)
        {
            if (this.human.Chips <= 0)
            {
                this.txtBoxHumanChips.Text = "Chips : 0";
            }

            if (this.bots[0].Chips <= 0)
            {
                this.txtBoxFirstBotChips.Text = "Chips : 0";
            }

            if (this.bots[1].Chips <= 0)
            {
                this.txtBoxSecondBotChips.Text = "Chips : 0";
            }

            if (this.bots[2].Chips <= 0)
            {
                this.txtBoxThirdBotChips.Text = "Chips : 0";
            }

            if (this.bots[3].Chips <= 0)
            {
                this.txtBoxFourthBotChips.Text = "Chips : 0";
            }

            if (this.bots[4].Chips <= 0)
            {
                this.txtBoxFifthBotChips.Text = "Chips : 0";
            }

            //TODO: extact in method
            this.txtBoxHumanChips.Text = "Chips : " + this.human.Chips;
            this.txtBoxFirstBotChips.Text = "Chips : " + this.bots[0].Chips;
            this.txtBoxSecondBotChips.Text = "Chips : " + this.bots[1].Chips;
            this.txtBoxThirdBotChips.Text = "Chips : " + this.bots[2].Chips;
            this.txtBoxFourthBotChips.Text = "Chips : " + this.bots[3].Chips;
            this.txtBoxFifthBotChips.Text = "Chips : " + this.bots[4].Chips;

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
            await this.Turns();
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
            await this.Turns();
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
                            this.last = 0;
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
                            this.last = 0;
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
            await this.Turns();
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
                this.bots[0].Chips += int.Parse(tbAdd.Text);
                this.bots[1].Chips += int.Parse(tbAdd.Text);
                this.bots[2].Chips += int.Parse(tbAdd.Text);
                this.bots[3].Chips += int.Parse(tbAdd.Text);
                this.bots[4].Chips += int.Parse(tbAdd.Text);
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