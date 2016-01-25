﻿namespace Poker.Utility
{
    public static class Constants
    {
        public const int CardsInADeck = 52;
        public const int NeededCardsFromDeck = 17; // Calculate 6 players * 2 card + 5 cards on the table
        public const int MaxChipsToAdd = 100000000;
        public const int MaxSmallBlind = 100000;
        public const int MinSmallBlind = 250;
        public const int MaxBigBlind = 200000;
        public const int MinBigBlind = 500;
    }
}
