namespace Poker.Tests
{
    using System.Windows.Forms;

    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Interfaces;
    using Models.Players;

    using Poker.Models;

    using UserInterface;

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

            playerMove.Raised(currentPlayer, currentPlayer.Status, ref isRisingActivated, ref globalRaise, ref globalCall, box);

            Assert.IsTrue(isRisingActivated, "Rising is still activated.");
            Assert.AreEqual(globalCall, globalCall, "The global call and raise are not equal.");
            Assert.AreEqual(playerChipsResult, currentPlayer.Chips, "The player's chips have not been lowered correctly.");
            Assert.IsFalse(currentPlayer.OutOfChips, "Player should be out of chips.");
            Assert.IsFalse(currentPlayer.Folded, "Player should fold.");
            Assert.AreEqual(globalRaise.ToString(), box.Text, "The pot tex box is not displaying correct information.");
        }
    }
}
