namespace Poker
{
    using System;
    using System.Windows.Forms;
    using UserInterface;


    static class PokerMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PokerTable());
        }
    }
}
