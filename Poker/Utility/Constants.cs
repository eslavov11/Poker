namespace Poker.Utility
{
    public static class Constants
    {
        public const int CardsInADeck = 52;
        public const int NeededCardsFromDeskForPlayersOnly = 12; // Calculate 6 platers * 2 cards
        public const int NeededCardsFromDeck = 17; // Calculate 6 players * 2 card + 5 cards on the table
        public const int MaxChipsToAdd = 100000000;
        public const int MaxSmallBlindValue = 100000;
        public const int MinSmallBlindValue = 250;
        public const int MaxBigBlindValue = 200000;
        public const int MinBigBlindValue = 500;
        public const int DefaultSecondsToMakeTurn = 60;
        public const int DefaultBotsCount = 5;
    }
}
