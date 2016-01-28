namespace Poker.Core
{
    using System;
    using System.Windows.Forms;
    using Interfaces;
    using UserInterface;

    public class PokerEngine : IEngine
    {
        public void Run()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new PokerTable());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
