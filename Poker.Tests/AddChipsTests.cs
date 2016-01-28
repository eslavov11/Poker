namespace Poker.Tests
{
    using Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models.Players;
    using UserInterface;

    [TestClass]
    public class AddChipsTests
    {
        [TestMethod]
        public void TestAdding_ShouldIncreaseNumberOfChips()
        {
            IPlayer human = new Human("Player");
            var chippAdder = new AddChips();
            chippAdder.ChipsAmount = 500;

            human.Chips += chippAdder.ChipsAmount;

            Assert.AreEqual(10500, human.Chips, "Player chips should be 10500.");
        }
    }
}
