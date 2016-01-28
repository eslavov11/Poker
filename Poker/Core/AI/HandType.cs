namespace Poker.Core.AI
{
    using System;
    using System.Windows.Forms;
    using Interfaces;

    public class HandType : IHandType
    {
        private readonly IPlayerMove playerMove;
        private readonly Random randomGenerator;

        public HandType()
        {
            this.randomGenerator = new Random();
            this.playerMove = new PlayerMove();
        }

        public void HighCard(IPlayer pokerPlayer, Label playerStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising)
        {
            this.playerMove.HP(pokerPlayer, playerStatus, 20, 25, neededChipsToCall, potStatus, ref raise, ref raising);
        }

        public void PairTable(IPlayer pokerPlayer, Label playerStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising)
        {
            this.playerMove.HP(pokerPlayer, playerStatus, 16, 25, neededChipsToCall, potStatus, ref raise, ref raising);
        }

        public void PairHand(IPlayer player, Label playerStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int rCall = this.randomGenerator.Next(10, 16);
            int rRaise = this.randomGenerator.Next(10, 13);
            if (player.HandPower <= 199 && player.HandPower >= 140)
            {
                this.playerMove.PH(player, playerStatus, rCall, 6, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }

            if (player.HandPower <= 139 && player.HandPower >= 128)
            {
                this.playerMove.PH(player, playerStatus, rCall, 7, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }

            if (player.HandPower < 128 && player.HandPower >= 101)
            {
                this.playerMove.PH(player, playerStatus, rCall, 9, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }
        }

        public void TwoPair(IPlayer player, Label playerStatus, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int rCall = this.randomGenerator.Next(6, 11);
            int rRaise = this.randomGenerator.Next(6, 11);
            if (player.HandPower <= 290 && player.HandPower >= 246)
            {
                this.playerMove.PH(player, playerStatus, rCall, 3, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }

            if (player.HandPower <= 244 && player.HandPower >= 234)
            {
                this.playerMove.PH(player, playerStatus, rCall, 4, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }

            if (player.HandPower < 234 && player.HandPower >= 201)
            {
                this.playerMove.PH(player, playerStatus, rCall, 4, rRaise, neededChipsToCall, potStatus, ref raise, ref raising, rounds);
            }
        }

        public void ThreeOfAKind(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int tCall = this.randomGenerator.Next(3, 7);
            int tRaise = this.randomGenerator.Next(4, 8);
            if (player.HandPower <= 390 && player.HandPower >= 330)
            {
                this.Smooth(player, playerStatus, name, tCall, tRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            //10  8
            if (player.HandPower <= 327 && player.HandPower >= 321)
            {
                this.Smooth(player, playerStatus, name, tCall, tRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            //7 2
            if (player.HandPower < 321 && player.HandPower >= 303)
            {
                this.Smooth(player, playerStatus, name, tCall, tRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void Straight(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int sCall = this.randomGenerator.Next(3, 6);
            int sRaise = this.randomGenerator.Next(3, 8);
            if (player.HandPower <= 480 && player.HandPower >= 410)
            {
                this.Smooth(player, playerStatus, name, sCall, sRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (player.HandPower <= 409 && player.HandPower >= 407)
            {
                this.Smooth(player, playerStatus, name, sCall, sRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (player.HandPower < 407 && player.HandPower >= 404)
            {
                this.Smooth(player, playerStatus, name, sCall, sRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void Flush(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int fCall = this.randomGenerator.Next(2, 6);
            int fRaise = this.randomGenerator.Next(3, 7);
            this.Smooth(player, playerStatus, name, fCall, fRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
        }

        public void FullHouse(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int fhCall = this.randomGenerator.Next(1, 5);
            int fhRaise = this.randomGenerator.Next(2, 6);
            if (player.HandPower <= 626 && player.HandPower >= 620)
            {
                this.Smooth(player, playerStatus, name, fhCall, fhRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }

            if (player.HandPower < 620 && player.HandPower >= 602)
            {
                this.Smooth(player, playerStatus, name, fhCall, fhRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void FourOfAKind(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int fkCall = this.randomGenerator.Next(1, 4);
            int fkRaise = this.randomGenerator.Next(2, 5);
            if (player.HandPower <= 752 && player.HandPower >= 704)
            {
                this.Smooth(player, playerStatus, name, fkCall, fkRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }

        public void StraightFlush(IPlayer player, Label playerStatus, int name, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int sfCall = this.randomGenerator.Next(1, 3);
            int sfRaise = this.randomGenerator.Next(1, 3);
            if (player.HandPower <= 913 && player.HandPower >= 804)
            {
                this.Smooth(player, playerStatus, name, sfCall, sfRaise, neededChipsToCall, potStatus, ref raise, ref raising, ref rounds);
            }
        }
        
        private void Smooth(IPlayer player, Label botStatus, int name, int n, int r, int neededChipsToCall, TextBox potStatus, ref int raise, ref bool raising, ref int rounds)
        {
            int rnd = this.randomGenerator.Next(1, 3);
            if (neededChipsToCall <= 0)
            {
                this.playerMove.Check(player, botStatus, ref raising);
            }
            else
            {
                if (neededChipsToCall >= PlayerMove.RoundN(player.Chips, n))
                {
                    if (player.Chips > neededChipsToCall)
                    {
                        this.playerMove.Call(player, botStatus, ref raising, ref neededChipsToCall, potStatus);
                    }
                    else if (player.Chips <= neededChipsToCall)
                    {
                        raising = false;
                        player.CanMakeTurn = false;
                        player.Chips = 0;
                        botStatus.Text = "Call " + player.Chips;
                        potStatus.Text = (int.Parse(potStatus.Text) + player.Chips).ToString();
                    }
                }
                else
                {
                    if (raise > 0)
                    {
                        if (player.Chips >= raise * 2)
                        {
                            raise *= 2;
                            this.playerMove.Raise(player, botStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                        }
                        else
                        {
                            this.playerMove.Call(player, botStatus, ref raising, ref neededChipsToCall, potStatus);
                        }
                    }
                    else
                    {
                        raise = neededChipsToCall * 2;
                        this.playerMove.Raise(player, botStatus, ref raising, ref raise, ref neededChipsToCall, potStatus);
                    }
                }
            }

            if (player.Chips <= 0)
            {
                player.OutOfChips = true;
            }
        }
    }
}