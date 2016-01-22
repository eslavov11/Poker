﻿namespace Poker.Models.Players
{
    using Interfaces;

    public class Bot : Player, IBot
    {
        public Bot(string name)
            : base(name)
        {
        }
    }
}