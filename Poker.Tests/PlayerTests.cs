namespace Poker.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Interfaces;
    using Models.Players;

    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void TestName_InitializeOnePlayer_ShouldCreatePlayer()
        {
            IPlayer bot = new Bot("Bot 1", 0, 0, 0);

            Assert.AreEqual("Bot 1", bot.Name, "Name should be Bot 1");
            Assert.AreEqual(10000, bot.Chips, "Player initial chips should be 10000");
        }
    }
}
