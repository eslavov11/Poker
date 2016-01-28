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
    using Poker.Enums;
    using Poker.Interfaces;
    using Poker.Models;
    using Poker.Models.Players;
    using Poker.Utility;
    using Type = Poker.Models.Type;

    public partial class PokerTable : Form
    {
        #region Variables
        private readonly IAssertHandType assertHandType;
        private readonly IHandType handType;
        private readonly IPlayer human;
        private readonly IList<IBot> bots;
        private readonly IDatabase pokerDatabase;
        private readonly Image[] deckImages;
        private readonly PictureBox[] cardsPictureBoxList;
        private readonly Timer timer = new Timer();
        private readonly Timer updates = new Timer();
        private string[] cardsImageLocation;
        private int[] reservedGameCardsIndeces;
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
            this.InitializePlayersDisplay();
            this.width = this.Width;
            this.height = this.Height;
            this.Shuffle();
            this.potStatus.Enabled = false;
            this.txtBoxHumanChips.Enabled = false;
            this.txtBoxHumanChips.Text = "Chips : " + this.human.Chips;
            this.PopulateBotChips();
            this.timer.Interval = 1 * 1 * 1000;
            this.timer.Tick += this.TimerTick;
            this.updates.Interval = 1 * 1 * 100;
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

        private void InitializePlayersDisplay()
        {
            int tabIndex = 13;
            int locationIndex = 0;
            int[] locationIndicesHorizontal = new int[] { 181, 276, 755, 970, 1012 };
            int[] locationIndicesVertical = new int[] { 553, 81, 81, 81, 553 };
            int[] sizesHorizontal = new int[] { 142, 133, 125, 123, 152 };
            int[] sizesVertical = new int[] { 23, 23, 23, 23, 23 };
            foreach (var bot in this.bots)
            {
                bot.TextBoxBotChips.Anchor = bot.GetAnchorStyles();
                bot.TextBoxBotChips.Font = new Font(
                    "Microsoft Sans Serif",
                    10F,
                    FontStyle.Regular,
                    GraphicsUnit.Point,
                    (byte)204);
                bot.TextBoxBotChips.Location = new Point(
                    locationIndicesHorizontal[locationIndex],
                    locationIndicesVertical[locationIndex]);
                bot.TextBoxBotChips.Name = bot.Name + " Chips";
                bot.TextBoxBotChips.Size = new Size(
                    sizesHorizontal[locationIndex],
                    sizesVertical[locationIndex]);
                bot.TextBoxBotChips.TabIndex = tabIndex;
                tabIndex++;
                locationIndex++;
                bot.TextBoxBotChips.Text = "Chips : 0";
            }
        }

        private void PopulateBots()
        {
            this.bots.Add(new Bot("Bot 1", 2, 420, 15, AnchorStyles.Bottom, AnchorStyles.Left));
            this.bots.Add(new Bot("Bot 2", 4, 65, 75, AnchorStyles.Top, AnchorStyles.Left));
            this.bots.Add(new Bot("Bot 3", 6, 25, 590, AnchorStyles.Top, 0));
            this.bots.Add(new Bot("Bot 4", 8, 65, 1115, AnchorStyles.Top, AnchorStyles.Right));
            this.bots.Add(new Bot("Bot 5", 10, 420, 1160, AnchorStyles.Bottom, AnchorStyles.Right));
        }

        private void PopulateBotChips()
        {
            foreach (var bot in this.bots)
            {
                bot.TextBoxBotChips.Enabled = false;
                bot.TextBoxBotChips.Text = "Chips : " + bot.Chips;
            }
        }

        private async Task Shuffle()
        {
            this.pokerDatabase.PlayersGameStatus.Add(this.human.OutOfChips);
            foreach (var bot in this.bots)
            {
                this.pokerDatabase.PlayersGameStatus.Add(bot.OutOfChips);
            }

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
                this.deckImages[this.cardNumber] = Image.FromFile(this.cardsImageLocation[this.cardNumber]);

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

                // Throwing cards
                if (this.cardNumber < 2)
                {
                    if (this.cardsPictureBoxList[0].Tag != null)
                    {
                        this.cardsPictureBoxList[1].Tag = this.reservedGameCardsIndeces[1];
                    }

                    this.cardsPictureBoxList[0].Tag = this.reservedGameCardsIndeces[0];
                    this.cardsPictureBoxList[this.cardNumber].Image = this.deckImages[this.cardNumber];
                    this.cardsPictureBoxList[this.cardNumber].Anchor = AnchorStyles.Bottom;

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

                if (this.cardNumber >= Constants.NeededCardsFromDeskForPlayersOnly)
                {
                    this.cardsPictureBoxList[12].Tag = this.reservedGameCardsIndeces[12];
                    if (this.cardNumber > Constants.NeededCardsFromDeskForPlayersOnly)
                    {
                        this.cardsPictureBoxList[Constants.NeededCardsFromDeskForPlayersOnly + 1].Tag = this.reservedGameCardsIndeces[Constants.NeededCardsFromDeskForPlayersOnly + 1];
                    }

                    if (this.cardNumber > Constants.NeededCardsFromDeskForPlayersOnly + 1)
                    {
                        this.cardsPictureBoxList[Constants.NeededCardsFromDeskForPlayersOnly + 2].Tag = this.reservedGameCardsIndeces[Constants.NeededCardsFromDeskForPlayersOnly + 2];
                    }

                    if (this.cardNumber > Constants.NeededCardsFromDeskForPlayersOnly + 2)
                    {
                        this.cardsPictureBoxList[Constants.NeededCardsFromDeskForPlayersOnly + 3].Tag = this.reservedGameCardsIndeces[Constants.NeededCardsFromDeskForPlayersOnly + 3];
                    }

                    if (this.cardNumber > Constants.NeededCardsFromDeskForPlayersOnly + 3)
                    {
                        this.cardsPictureBoxList[Constants.NeededCardsFromDeskForPlayersOnly + 4].Tag = this.reservedGameCardsIndeces[Constants.NeededCardsFromDeskForPlayersOnly + 4];
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
            }

            // GameOver state
            if (this.foldedBotsCount == Constants.DefaultBotsCount)
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
                this.foldedBotsCount = Constants.DefaultBotsCount;
            }

            if (this.cardNumber == Constants.NeededCardsFromDeck)
            {
                this.buttonRaise.Enabled = true;
                this.buttonCall.Enabled = true;
                this.buttonRaise.Enabled = true;
                this.buttonRaise.Enabled = true;
                this.buttonFold.Enabled = true;
            }
        }

        private void DealCardsForBots(
            IBot bot,
            int cardNumberIndex,
            Bitmap backImage,
            ref bool check,
            ref int horizontal,
            ref int vertical)
        {
            if (bot.Chips > 0)
            {
                this.foldedBotsCount--;
                if (cardNumberIndex >= bot.StartCard && cardNumberIndex < bot.StartCard + 2)
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
                    this.cardsPictureBoxList[cardNumberIndex].Anchor = bot.GetAnchorStyles();
                    this.cardsPictureBoxList[cardNumberIndex].Image = backImage;

                    //cardsPictureBoxList[i].Image = deckImages[i];
                    this.cardsPictureBoxList[cardNumberIndex].Location = new Point(horizontal, vertical);
                    horizontal += this.cardsPictureBoxList[cardNumberIndex].Width;
                    this.cardsPictureBoxList[cardNumberIndex].Visible = true;
                    this.Controls.Add(bot.Panel);
                    bot.InitializePanel(new Point(
                        this.cardsPictureBoxList[bot.StartCard].Left - 10,
                        this.cardsPictureBoxList[bot.StartCard].Top - 10));

                    if (cardNumberIndex == bot.StartCard + 1)
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
            // Rotation
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

                await this.AllIn();
                if (!this.restart)
                {
                    await this.Turns();
                }

                this.restart = false;
            }
        }

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
                        this.Rules(this.bots[botNumber].StartCard, this.bots[botNumber].StartCard + 1, this.bots[botNumber]);
                        MessageBox.Show("Bot " + (botNumber + 1) + "'s turn");
                        this.AI(this.bots[botNumber].StartCard, this.bots[botNumber].StartCard + 1, this.bots[botNumber].Status, botNumber, this.bots[botNumber]);
                        this.turnCount++;
                        this.last = botNumber + 1;
                        this.bots[botNumber].CanMakeTurn = false;
                        if (botNumber < this.bots.Count - 1)
                        {
                            this.bots[botNumber + 1].CanMakeTurn = true;
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

        // TODO: need to improve method's code quality
        private void Rules(int card1, int card2, IPlayer player)
        {
            if (card1 == 0 && card2 == 1)
            {
            }

            if (((!player.OutOfChips || card1 == 0) && card2 == 1) && !this.humanStatus.Text.Contains("Fold"))
            {
                // Initialization
                bool done = false;
                bool vf = false;
                int[] cardsOnBoard = new int[5];
                int[] straight = new int[7];
                straight[0] = this.reservedGameCardsIndeces[card1];
                straight[1] = this.reservedGameCardsIndeces[card2];
                cardsOnBoard[0] = straight[2] = this.reservedGameCardsIndeces[12];
                cardsOnBoard[1] = straight[3] = this.reservedGameCardsIndeces[13];
                cardsOnBoard[2] = straight[4] = this.reservedGameCardsIndeces[14];
                cardsOnBoard[3] = straight[5] = this.reservedGameCardsIndeces[15];
                cardsOnBoard[4] = straight[6] = this.reservedGameCardsIndeces[16];
                int[] getClubes = straight.Where(o => o % 4 == 0).ToArray();
                int[] getDimonds = straight.Where(o => o % 4 == 1).ToArray();
                int[] getHearts = straight.Where(o => o % 4 == 2).ToArray();
                int[] getSpades = straight.Where(o => o % 4 == 3).ToArray();
                int[] clubes = getClubes.Select(o => o / 4).Distinct().ToArray();
                int[] diamonds = getDimonds.Select(o => o / 4).Distinct().ToArray();
                int[] hearts = getHearts.Select(o => o / 4).Distinct().ToArray();
                int[] spades = getSpades.Select(o => o / 4).Distinct().ToArray();
                Array.Sort(straight);
                Array.Sort(clubes);
                Array.Sort(diamonds);
                Array.Sort(hearts);
                Array.Sort(spades);

                for (int index = 0; index < 16; index++)
                {
                    if (this.reservedGameCardsIndeces[index] == int.Parse(this.cardsPictureBoxList[card1].Tag.ToString()) &&
                        this.reservedGameCardsIndeces[index + 1] == int.Parse(this.cardsPictureBoxList[card2].Tag.ToString()))
                    {
                        this.assertHandType.PairFromHand(player, index, this.pokerDatabase.Winners, ref this.sorted, ref this.reservedGameCardsIndeces);
                        this.assertHandType.PairTwoPair(player, index, this.pokerDatabase.Winners, ref this.sorted, ref this.reservedGameCardsIndeces);
                        this.assertHandType.TwoPair(player, index, this.pokerDatabase.Winners, ref this.sorted, ref this.reservedGameCardsIndeces);
                        this.assertHandType.ThreeOfAKind(player, straight, index, this.pokerDatabase.Winners, ref this.sorted);
                        this.assertHandType.Straight(player, straight, index, this.pokerDatabase.Winners, ref this.sorted);
                        this.assertHandType.Flush(player, ref vf, cardsOnBoard, ref index, this.pokerDatabase.Winners, ref this.sorted, ref this.reservedGameCardsIndeces);
                        this.assertHandType.FullHouse(player, ref done, straight, this.pokerDatabase.Winners, ref this.sorted, ref this.type);
                        this.assertHandType.FourOfAKind(player, straight, this.pokerDatabase.Winners, ref this.sorted);
                        this.assertHandType.StraightFlush(player, clubes, diamonds, hearts, spades, this.pokerDatabase.Winners, ref this.sorted);
                        this.assertHandType.HighCard(player, index, this.pokerDatabase.Winners, ref this.sorted, ref this.reservedGameCardsIndeces);
                    }
                }
            }
        }

        private void Winner(IPlayer player, string lastFixedPlayer)
        {
            if (lastFixedPlayer == " ")
            {
                lastFixedPlayer = "Bot 5";
            }

            for (int j = 0; j <= 16; j++)
            {
                //await Task.Delay(5);
                if (this.cardsPictureBoxList[j].Visible)
                {
                    this.cardsPictureBoxList[j].Image = this.deckImages[j];
                }
            }

            if (player.HandType == this.sorted.Current)
            {
                if (player.HandPower == this.sorted.Power)
                {
                    this.winners++;
                    this.pokerDatabase.CheckWinners.Add(player.Name);

                    //Converted to switch case because 5.5 Player type was converted to 5 in the if statement as well
                    switch ((int)player.HandType)
                    {
                        case -1:
                            MessageBox.Show(player.Name + " High Card ");
                            break;
                        case 0:
                        case 1:
                            MessageBox.Show(player.Name + " Pair ");
                            break;
                        case 2:
                            MessageBox.Show(player.Name + " Two Pair ");
                            break;
                        case 3:
                            MessageBox.Show(player.Name + " Three of a Kind ");
                            break;
                        case 4:
                            MessageBox.Show(player.Name + " Straight ");
                            break;
                        case 5:
                            MessageBox.Show(player.Name + " Flush ");
                            break;
                        case 6:
                            MessageBox.Show(player.Name + " Full House ");
                            break;
                        case 7:
                            MessageBox.Show(player.Name + " Four of a Kind ");
                            break;
                        case 8:
                            MessageBox.Show(player.Name + " Straight Flush ");
                            break;
                        case 9:
                            MessageBox.Show(player.Name + " Royal Flush ! ");
                            break;
                    }
                }
            }

            if (player.Name == lastFixedPlayer)
            {
                if (this.winners > 1)
                {
                    if (this.pokerDatabase.CheckWinners.Contains("Player"))
                    {
                        this.human.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        this.txtBoxHumanChips.Text = this.human.Chips.ToString();

                        //playerPanel.Visible = true;
                    }

                    foreach (var bot in this.bots)
                    {
                        if (!this.pokerDatabase.CheckWinners.Contains(bot.Name))
                        {
                            continue;
                        }

                        bot.Chips += int.Parse(this.potStatus.Text) / this.winners;
                        bot.TextBoxBotChips.Text = bot.Chips.ToString();
                    }
                }
                else if (this.winners == 1)
                {
                    if (this.pokerDatabase.CheckWinners.Contains("Player"))
                    {
                        this.human.Chips += int.Parse(this.potStatus.Text);
                        this.txtBoxHumanChips.Text = this.human.Chips.ToString();

                        //playerPanel.Visible = true;
                        return;
                    }

                    foreach (var bot in this.bots)
                    {
                        if (this.pokerDatabase.CheckWinners.Contains(bot.Name))
                        {
                            bot.Chips += int.Parse(this.potStatus.Text);
                            return;
                        }
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

                        for (int i = 0; i < this.bots.Count; i++)
                        {
                            if (!this.bots[i].OutOfChips)
                            {
                                this.bots[i].Status.Text = string.Empty;
                            }
                        }
                    }
                }
            }

            if (this.rounds == (int)GameStage.Flop)
            {
                for (int j = 12; j <= 14; j++)
                {
                    if (this.cardsPictureBoxList[j].Image != this.deckImages[j])
                    {
                        this.ResetPlayersStats(j);
                    }
                }
            }

            if (this.rounds == (int)GameStage.Turn)
            {
                for (int j = 14; j <= 15; j++)
                {
                    if (this.cardsPictureBoxList[j].Image != this.deckImages[j])
                    {
                        this.ResetPlayersStats(j);
                    }
                }
            }

            if (this.rounds == (int)GameStage.River)
            {
                for (int j = 15; j <= 16; j++)
                {
                    if (this.cardsPictureBoxList[j].Image != this.deckImages[j])
                    {
                        this.ResetPlayersStats(j);
                    }
                }
            }

            if (this.rounds == (int)GameStage.End && this.maxLeft == 6)
            {
                string fixedLast = "qwerty";
                if (!this.humanStatus.Text.Contains("Fold"))
                {
                    fixedLast = "Player";
                    this.Rules(0, 1, this.human);
                }

                foreach (var bot in this.bots)
                {
                    if (!bot.Status.Text.Contains("Fold"))
                    {
                        fixedLast = bot.Name;
                        this.Rules(bot.StartCard, bot.StartCard + 1, bot);
                    }
                }

                this.Winner(this.human, fixedLast);

                foreach (var bot in this.bots)
                {
                    this.Winner(bot, fixedLast);
                }

                this.restart = true;
                this.human.CanMakeTurn = true;
                this.human.OutOfChips = false;
                foreach (var bot in this.bots)
                {
                    bot.OutOfChips = false;
                }

                if (this.human.Chips <= 0)
                {
                    this.AddChips();
                }

                this.human.Panel.Visible = false;

                foreach (var bot in this.bots)
                {
                    bot.Panel.Visible = false;
                }

                this.human.Call = 0;
                this.human.Raise = 0;

                foreach (var bot in this.bots)
                {
                    bot.Call = 0;
                    bot.Raise = 0;
                }

                this.last = 0;
                this.neededChipsToCall = this.bigBlindValue;
                this.raise = 0;
                this.cardsImageLocation = Directory.GetFiles("..\\..\\Resources\\Assets\\Cards", "*.png", SearchOption.TopDirectoryOnly);
                this.pokerDatabase.PlayersGameStatus.Clear();
                this.rounds = 0;
                this.type = 0;

                this.human.HandPower = 0;

                foreach (var bot in this.bots)
                {
                    bot.HandPower = 0;
                    bot.HandType = -1;
                }

                this.human.HandType = -1;
                this.pokerDatabase.Chips.Clear();
                this.pokerDatabase.CheckWinners.Clear();
                this.winners = 0;
                this.pokerDatabase.Winners.Clear();
                this.sorted.Current = 0;
                this.sorted.Power = 0;
                for (int os = 0; os < 17; os++)
                {
                    this.cardsPictureBoxList[os].Image = null;
                    this.cardsPictureBoxList[os].Invalidate();
                    this.cardsPictureBoxList[os].Visible = false;
                }

                this.potStatus.Text = "0";
                this.humanStatus.Text = string.Empty;
                await this.Shuffle();
                await this.Turns();
            }
        }

        private void AddChips()
        {
            AddChips chipAdder = new AddChips();
            chipAdder.ShowDialog();
            if (chipAdder.ChipsAmount > 0)
            {
                this.human.Chips = chipAdder.ChipsAmount;

                foreach (var bot in this.bots)
                {
                    bot.Chips += chipAdder.ChipsAmount;
                }

                this.human.OutOfChips = false;
                this.human.CanMakeTurn = true;
                this.buttonRaise.Enabled = true;
                this.buttonFold.Enabled = true;
                this.buttonCheck.Enabled = true;
                this.buttonRaise.Text = "Raise";
            }
        }

        private void ResetPlayersStats(int cardIndex)
        {
            this.cardsPictureBoxList[cardIndex].Image = this.deckImages[cardIndex];
            this.human.Call = 0;
            this.human.Raise = 0;

            foreach (var bot in this.bots)
            {
                bot.Call = 0;
                bot.Raise = 0;
            }
        }

        private void FixCall(Label status, int call, int raiseCall, int options)
        {
            if (this.rounds != 4)
            {
                if (options == 1)
                {
                    if (status.Text.Contains("Raise"))
                    {
                        var changeRaise = status.Text.Substring(6);
                        raiseCall = int.Parse(changeRaise);
                    }

                    if (status.Text.Contains("Call"))
                    {
                        var changeCall = status.Text.Substring(5);
                        call = int.Parse(changeCall);
                    }

                    if (status.Text.Contains("PlayerCheck"))
                    {
                        raiseCall = 0;
                        call = 0;
                    }
                }

                if (options == 2)
                {
                    if (raiseCall != this.raise && raiseCall <= this.raise)
                    {
                        this.neededChipsToCall = Convert.ToInt32(this.raise) - raiseCall;
                    }

                    if (call != this.neededChipsToCall || call <= this.neededChipsToCall)
                    {
                        this.neededChipsToCall = this.neededChipsToCall - call;
                    }

                    if (raiseCall == this.raise && this.raise > 0)
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
            // ALl in
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
            for (int botCount = 0; botCount < this.bots.Count; botCount++)
            {
                if (this.bots[botCount].Chips <= 0 && !this.bots[botCount].OutOfChips)
                {
                    if (!this.chipsAreAdded)
                    {
                        this.pokerDatabase.Chips.Add(this.bots[botCount].Chips);
                        this.chipsAreAdded = true;
                    }

                    this.chipsAreAdded = false;
                }
            }

            if (this.pokerDatabase.Chips.ToArray().Length == this.maxLeft)
            {
                await this.Finish(2);
            }
            else
            {
                this.pokerDatabase.Chips.Clear();
            }

            var leftPlayers = this.pokerDatabase.PlayersGameStatus.Count(x => x == false);

            // LastManStanding
            if (leftPlayers == 1)
            {
                int index = this.pokerDatabase.PlayersGameStatus.IndexOf(false);
                switch (index)
                {
                    case 0:
                        this.human.Chips += int.Parse(this.potStatus.Text);
                        this.txtBoxHumanChips.Text = this.human.Chips.ToString();
                        this.human.Panel.Visible = true;
                        MessageBox.Show("Player Wins");
                        break;
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                        this.bots[index - 1].Chips += int.Parse(this.potStatus.Text);
                        this.txtBoxHumanChips.Text = this.bots[index - 1].Chips.ToString();
                        this.bots[index - 1].Panel.Visible = true;
                        MessageBox.Show("Bot " + index + " Wins");
                        break;
                    default:
                        break;
                }

                for (int j = 0; j <= 16; j++)
                {
                    this.cardsPictureBoxList[j].Visible = false;
                }

                await this.Finish(1);
            }

            this.chipsAreAdded = false;

            // FiveOrLessLeft
            if (leftPlayers < 6 && leftPlayers > 1 && this.rounds >= (int)GameStage.End)
            {
                await this.Finish(2);
            }
        }

        private async Task Finish(int n)
        {
            if (n == 2)
            {
                this.FixWinners();
            }

            this.neededChipsToCall = this.bigBlindValue;
            this.raise = 0;
            this.foldedBotsCount = 5;
            this.type = 0;
            this.rounds = 0;
            this.raise = 0;
            this.restart = false;
            this.raising = false;
            this.height = 0;
            this.width = 0;
            this.winners = 0;
            this.maxLeft = 6;
            this.last = 123;
            this.raisedTurn = 1;
            this.pokerDatabase.PlayersGameStatus.Clear();
            this.pokerDatabase.CheckWinners.Clear();
            this.pokerDatabase.Chips.Clear();
            this.pokerDatabase.Winners.Clear();
            this.sorted.Current = 0;
            this.sorted.Power = 0;
            this.potStatus.Text = "0";
            this.secondsToMakeTurn = 60;
            this.up = 10000000;
            this.turnCount = 0;

            this.human.Panel.Visible = false;
            this.human.HandPower = 0;
            this.human.HandType = -1;
            this.human.Folded = false;
            this.human.OutOfChips = false;
            this.human.CanMakeTurn = true;
            this.human.Call = 0;
            this.human.Raise = 0;
            this.humanStatus.Text = string.Empty;

            for (int botCount = 0; botCount < this.bots.Count; botCount++)
            {
                this.bots[botCount].Panel.Visible = false;
                this.bots[botCount].HandPower = 0;
                this.bots[botCount].HandType = -1;
                this.bots[botCount].CanMakeTurn = false;
                this.bots[botCount].OutOfChips = false;
                this.bots[botCount].Folded = false;
                this.bots[botCount].Call = 0;
                this.bots[botCount].Raise = 0;
                this.bots[botCount].Status.Text = string.Empty;
            }

            if (this.human.Chips <= 0)
            {
                AddChips chipAdder = new AddChips();
                chipAdder.ShowDialog();
                if (chipAdder.ChipsAmount > 0)
                {
                    this.human.Chips = chipAdder.ChipsAmount;

                    foreach (var bot in this.bots)
                    {
                        bot.Chips += chipAdder.ChipsAmount;
                    }

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
            this.pokerDatabase.Winners.Clear();
            this.sorted.Current = 0;
            this.sorted.Power = 0;
            string fixedLast = "qwerty";
            if (!this.humanStatus.Text.Contains("Fold"))
            {
                fixedLast = "Player";
                this.Rules(0, 1, this.human);
            }

            foreach (var bot in this.bots)
            {
                if (!bot.Status.Text.Contains("Fold"))
                {
                    fixedLast = bot.Name;
                    this.Rules(bot.StartCard, bot.StartCard + 1, bot);
                }
            }

            //TODO: code repetition
            this.Winner(this.human, fixedLast);

            foreach (var bot in this.bots)
            {
                this.Winner(bot, fixedLast);
            }
        }

        private void AI(int c1, int c2, Label playerStatus, int name, IPlayer player)
        {
            if (!player.OutOfChips)
            {
                switch ((int)player.HandType)
                {
                    case -1:
                        this.handType.HighCard(player, playerStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising);
                        break;
                    case 0:
                        this.handType.PairTable(player, playerStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising);
                        break;
                    case 1:
                        this.handType.PairHand(player, playerStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                        break;
                    case 2:
                        this.handType.TwoPair(player, playerStatus, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                        break;
                    case 3:
                        this.handType.ThreeOfAKind(player, playerStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                        break;
                    case 4:
                        this.handType.Straight(player, playerStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                        break;
                    case 5:
                        this.handType.Flush(player, playerStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                        break;
                    case 6:
                        this.handType.FullHouse(player, playerStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                        break;
                    case 7:
                        this.handType.FourOfAKind(player, playerStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                        break;
                    case 8:
                    case 9:
                        this.handType.StraightFlush(player, playerStatus, name, this.neededChipsToCall, this.potStatus, ref this.raise, ref this.raising, ref this.rounds);
                        break;
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

            foreach (var bot in this.bots)
            {
                if (bot.Chips <= 0)
                {
                    bot.TextBoxBotChips.Text = "Chips : 0";
                }
            }

            //TODO: extact in method
            this.txtBoxHumanChips.Text = "Chips : " + this.human.Chips;

            foreach (var bot in this.bots)
            {
                bot.TextBoxBotChips.Text = "Chips : " + bot.Chips;
            }

            if (this.human.Chips <= 0)
            {
                this.human.CanMakeTurn = false;
                this.human.OutOfChips = true;
                this.buttonCall.Enabled = false;
                this.buttonRaise.Enabled = false;
                this.buttonFold.Enabled = false;
                this.buttonCheck.Enabled = false;
            }

            if (this.up > 0)
            {
                this.up--;
            }

            if (this.human.Chips >= this.neededChipsToCall)
            {
                this.buttonCall.Text = "Call " + this.neededChipsToCall;
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

            if (tbRaise.Text != string.Empty && int.TryParse(tbRaise.Text, out parsedValue))
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
            this.Rules(0, 1, this.human);
            if (this.human.Chips >= this.neededChipsToCall)
            {
                this.human.Chips -= this.neededChipsToCall;
                this.txtBoxHumanChips.Text = "Chips : " + this.human.Chips.ToString();
                if (this.potStatus.Text != string.Empty)
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
            this.Rules(0, 1, this.human);
            int parsedValue;
            if (tbRaise.Text != string.Empty && int.TryParse(tbRaise.Text, out parsedValue))
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

        private void ButtonAddChips_Click(object sender, EventArgs e)
        {
            if (tbAdd.Text == string.Empty)
            {
            }
            else
            {
                int parsed = 0;

                if (!int.TryParse(tbAdd.Text, out parsed))
                {
                    MessageBox.Show("Chips amount should be a round number.");
                    return;
                }

                if (int.Parse(tbAdd.Text) <= 0)
                {
                    MessageBox.Show("Chips amount should be positive number.");
                }

                this.human.Chips += int.Parse(tbAdd.Text);

                foreach (var bot in this.bots)
                {
                    bot.Chips += int.Parse(tbAdd.Text);
                }
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
            try
            {
                this.BlindExceptionCheck(1, this.tbSmallBlind.Text, ref this.smallBlindValue);
            }
            catch (ArgumentException p)
            {
                MessageBox.Show(p.Message);
            }
        }

        private void ButtonBigBlind_Click(object sender, EventArgs e)
        {
            try
            {
                this.BlindExceptionCheck(2, this.tbBigBlind.Text, ref this.bigBlindValue);
            }
            catch (ArgumentException p)
            {
                MessageBox.Show(p.Message);
            }
        }

        private void BlindExceptionCheck(int number, string text, ref int blindValue)
        {
            int smallBlindNewValue;
            if (text.Contains(",") || text.Contains("."))
            {
                throw new ArgumentException("The Small Blind can be only round number !");
            }

            if (!int.TryParse(text, out smallBlindNewValue))
            {
                throw new ArgumentException("This is a number only field");
            }

            if (int.Parse(text) > 100000 * number)
            {
                throw new ArgumentException("The maximum of the Small Blind is " + Constants.MaxSmallBlindValue * number + " $");
            }

            if (int.Parse(text) < 250 * number)
            {
                throw new ArgumentException("The maximum of the Small Blind is " + Constants.MinSmallBlindValue * number + " $");
            }

            if (int.Parse(text) >= Constants.MinSmallBlindValue * number && int.Parse(text) <= Constants.MaxSmallBlindValue * number)
            {
                blindValue = int.Parse(text);
                MessageBox.Show(
                    "The changes have been saved ! They will become available the next hand you play. ");
            }
        }

        private void ChangeLayout(object sender, LayoutEventArgs e)
        {
            this.width = this.Width;
            this.height = this.Height;
        }
        #endregion
    }
}