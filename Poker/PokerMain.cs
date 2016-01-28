namespace Poker
{
    using System;
    using System.Windows.Forms;
    using Core;
    using Interfaces;
    using UserInterface;

    public static class PokerMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            IEngine engine = new PokerEngine();
            engine.Run();
            
        }
    }
}
