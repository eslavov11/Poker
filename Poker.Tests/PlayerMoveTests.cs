namespace Poker.Tests
{
    using System.Windows.Forms;
    using Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models;
    using Models.Players;
    using Poker.Core.AI;

    [TestClass]
    public class PlayerMoveTests
    {
        [TestMethod]
        public void TestRaised_CreateBot_RisingShouldBeActivated()
        {
            IPlayerMove playerMove = new PlayerMove();
            IBot currentPlayer = new Bot("Bot 1", 0, 0, 0);
            currentPlayer.Status = new Label();
            bool isRisingActivated = false;
            int globalCall = 0;
            int globalRaise = 500;
            TextBox box = new TextBox();
            box.Text = "0";
            int playerChipsResult = currentPlayer.Chips - globalRaise;

            playerMove.Raise(currentPlayer, currentPlayer.Status, ref isRisingActivated, ref globalRaise, ref globalCall, box);

            Assert.IsTrue(isRisingActivated, "Rising is still activated.");
            Assert.AreEqual(globalCall, globalCall, "The global call and raise are not equal.");
            Assert.AreEqual(playerChipsResult, currentPlayer.Chips, "The player's chips have not been lowered correctly.");
            Assert.IsFalse(currentPlayer.OutOfChips, "Player should be out of chips.");
            Assert.IsFalse(currentPlayer.Folded, "Player should fold.");
            Assert.AreEqual(globalRaise.ToString(), box.Text, "The pot tex box is not displaying correct information.");
        }

        [TestMethod]
        public void TestFold_CreateBot_BotStateShouldBeFold()
        {
            IPlayerMove playerMove = new PlayerMove();
            IBot currentPlayer = new Bot("Bot 1", 0, 0, 0);
            currentPlayer.Status = new Label();
            bool isRisingActivated = false;
            TextBox box = new TextBox();
            box.Text = "0";

            playerMove.Fold(currentPlayer, currentPlayer.Status, ref isRisingActivated);
            Assert.IsFalse(isRisingActivated, "Rising should be false.");
            Assert.IsFalse(currentPlayer.CanMakeTurn, "Player shouldn't be albe to make turn");
        }

        [TestMethod]
        public void TestCall_CreateBot_BotChipsShouldBeSubtractedByRaise()
        {
            IPlayerMove playerMove = new PlayerMove();
            IBot currentPlayer = new Bot("Bot 1", 0, 0, 0);
            currentPlayer.Status = new Label();
            bool isRisingActivated = false;
            int globalRaise = 500;
            TextBox box = new TextBox();
            box.Text = "0";

            playerMove.Call(currentPlayer, currentPlayer.Status, ref isRisingActivated, ref globalRaise, box);

            Assert.AreEqual(9500, currentPlayer.Chips, "Player chips should be 9500 after checked.");
        }

        [TestMethod]
        public void TestCheck_CreateBot_BotCantRise()
        {
            IPlayerMove playerMove = new PlayerMove();
            IBot currentPlayer = new Bot("Bot 1", 0, 0, 0);
            currentPlayer.Status = new Label();
            bool isRisingActivated = false;
            int globalRaise = 500;
            TextBox box = new TextBox();
            box.Text = "0";

            playerMove.Call(currentPlayer, currentPlayer.Status, ref isRisingActivated, ref globalRaise, box);

            Assert.IsFalse(currentPlayer.CanMakeTurn, "Bot shouldnt be able to make turn.");
            Assert.IsFalse(isRisingActivated, "Bot shouldnt be able to raise");
        }
    }
}
