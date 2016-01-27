namespace Poker.Tests
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Poker.Data;
    using Poker.Models;

    [TestClass]
    public class PokerDatabaseTests
    {
        [TestMethod]
        public void TestConstructor_InitializeWinners_CountOne()
        {
            var database = new PokerDatabase();
            database.Winners.Add(new Type());

            Assert.AreEqual(1, database.Winners.Count, "Count should be one.");
        }
    }
}
