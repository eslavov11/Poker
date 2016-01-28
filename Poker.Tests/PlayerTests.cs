namespace Poker.Tests
{
    using Interfaces;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Models.Players;

    [TestClass]
    public class PlayerTests
    {
        [TestMethod]
        public void TestConstructor_InitializeBot_ShouldCreatePlayer()
        {
            IPlayer bot = new Bot("Bot 1", 0, 0, 0);

            Assert.AreEqual("Bot 1", bot.Name, "Name should be Bot 1");
            Assert.IsFalse(bot.CanMakeTurn, "The bot is allowed to play.");
            Assert.AreEqual(10000, bot.Chips, "Player initial chips should be 10000");
            Assert.IsNotNull(bot.Panel, "The bot panel is null.");
        }
    }
}
