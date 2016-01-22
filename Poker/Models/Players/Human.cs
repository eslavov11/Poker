﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker.Models.Players
{
    using Interfaces;

    public class Human : Player, IHuman
    {
        public Human(string name)
            : base(name)
        {
        }
    }
}