namespace Poker.Core.AI
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;
    using Poker.Models;

    /// <summary>
    /// Class which contains methods with different kinds of poker hand types.
    /// </summary>
    /// <seealso cref="Poker.Interfaces.IAssertHandType" />
    public class AssertHandType : IAssertHandType
    {
        public void StraightFlush(
            IPlayer player,
            int[] clubes,
            int[] dimonds,
            int[] hearts,
            int[] spades,
            List<Type> winners,
            ref Type sorted)
        {
            if (player.HandType >= -1)
            {
                if (clubes.Length >= 5)
                {
                    GetValue(player, clubes, winners, out sorted);
                }

                if (dimonds.Length >= 5)
                {
                    GetValue(player, dimonds, winners, out sorted);
                }

                if (hearts.Length >= 5)
                {
                    GetValue(player, hearts, winners, out sorted);
                }

                if (spades.Length >= 5)
                {
                    GetValue(player, spades, winners, out sorted);
                }
            }
        }

        /// <summary>
        /// Sets the player type if his hand type is four of a kind.
        /// </summary>
        public void FourOfAKind(IPlayer player, int[] straight, List<Type> winners, ref Type sorted)
        {
            if (player.HandType >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (straight[j] / 4 == straight[j + 1] / 4 && straight[j] / 4 == straight[j + 2] / 4
                        && straight[j] / 4 == straight[j + 3] / 4)
                    {
                        player.HandType = 7;
                        player.HandPower = (straight[j] / 4) * 4 + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 7 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (straight[j] / 4 == 0 && straight[j + 1] / 4 == 0 && straight[j + 2] / 4 == 0
                        && straight[j + 3] / 4 == 0)
                    {
                        player.HandType = 7;
                        player.HandPower = (13 * 4) + (player.HandType * 100);
                        winners.Add(new Type() { Power = player.HandPower, Current = 7 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        /// <summary>
        /// Sets the player type if his hand type is full house.
        /// </summary>
        public void FullHouse(
            IPlayer player,
            ref bool done,
            int[] straight,
            List<Type> winners,
            ref Type sorted,
            ref double type)
        {
            if (player.HandType >= -1)
            {
                type = player.HandPower;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                sorted = GetWinner(player, winners);
                                break;
                            }

                            if (fh.Max() / 4 > 0)
                            {
                                player.HandType = 6;
                                player.HandPower = fh.Max() / 4 * 2 + player.HandType * 100;
                                winners.Add(new Type() { Power = player.HandPower, Current = 6 });
                                sorted =
                                    winners.OrderByDescending(op1 => op1.Current)
                                    .ThenByDescending(op1 => op1.Power)
                                    .First();
                                break;
                            }
                        }

                        if (done)
                        {
                            continue;
                        }

                        if (fh.Max() / 4 == 0)
                        {
                            player.HandPower = 13;
                            done = true;
                            j = -1;
                        }
                        else
                        {
                            player.HandPower = fh.Max() / 4;
                            done = true;
                            j = -1;
                        }
                    }
                }

                if (player.HandType != 6)
                {
                    player.HandPower = type;
                }
            }
        }

        /// <summary>
        /// Sets the player type if his hand type is flush.
        /// </summary>
        public void Flush(
            IPlayer player,
            ref bool vf,
            int[] straight1,
            ref int index,
            List<Type> winners,
            ref Type sorted,
            ref int[] reserve)
        {
            if (player.HandType >= -1)
            {
                var f1 = straight1.Where(o => o % 4 == 0).ToArray();
                var f2 = straight1.Where(o => o % 4 == 1).ToArray();
                var f3 = straight1.Where(o => o % 4 == 2).ToArray();
                var f4 = straight1.Where(o => o % 4 == 3).ToArray();
                if (f1.Length == 3 || f1.Length == 4)
                {
                    if (reserve[index] % 4 == reserve[index + 1] % 4 && reserve[index] % 4 == f1[0] % 4)
                    {
                        if (reserve[index] / 4 > f1.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[index + 1] / 4 > f1.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index + 1] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[index] / 4 < f1.Max() / 4 && reserve[index + 1] / 4 < f1.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = f1.Max() + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                //different cards in hand
                if (f1.Length == 4) 
                {
                    if (reserve[index] % 4 != reserve[index + 1] % 4 && reserve[index] % 4 == f1[0] % 4)
                    {
                        if (reserve[index] / 4 > f1.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.HandType = 5;
                            player.HandPower = f1.Max() + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[index + 1] % 4 != reserve[index] % 4 && reserve[index + 1] % 4 == f1[0] % 4)
                    {
                        if (reserve[index + 1] / 4 > f1.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index + 1] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.HandType = 5;
                            player.HandPower = f1.Max() + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 5)
                {
                    if (reserve[index] % 4 == f1[0] % 4 && reserve[index] / 4 > f1.Min() / 4)
                    {
                        player.HandType = 5;
                        player.HandPower = reserve[index] + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[index + 1] % 4 == f1[0] % 4 && reserve[index + 1] / 4 > f1.Min() / 4)
                    {
                        player.HandType = 5;
                        player.HandPower = reserve[index + 1] + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[index] / 4 < f1.Min() / 4 && reserve[index + 1] / 4 < f1.Min())
                    {
                        player.HandType = 5;
                        player.HandPower = f1.Max() + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (reserve[index] % 4 == reserve[index + 1] % 4 && reserve[index] % 4 == f2[0] % 4)
                    {
                        if (reserve[index] / 4 > f2.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[index + 1] / 4 > f2.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index + 1] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[index] / 4 < f2.Max() / 4 && reserve[index + 1] / 4 < f2.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = f2.Max() + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                // different cards in hand
                if (f2.Length == 4) 
                {
                    if (reserve[index] % 4 != reserve[index + 1] % 4 && reserve[index] % 4 == f2[0] % 4)
                    {
                        if (reserve[index] / 4 > f2.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.HandType = 5;
                            player.HandPower = f2.Max() + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[index + 1] % 4 != reserve[index] % 4 && reserve[index + 1] % 4 == f2[0] % 4)
                    {
                        if (reserve[index + 1] / 4 > f2.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index + 1] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.HandType = 5;
                            player.HandPower = f2.Max() + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 5)
                {
                    if (reserve[index] % 4 == f2[0] % 4 && reserve[index] / 4 > f2.Min() / 4)
                    {
                        player.HandType = 5;
                        player.HandPower = reserve[index] + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[index + 1] % 4 == f2[0] % 4 && reserve[index + 1] / 4 > f2.Min() / 4)
                    {
                        player.HandType = 5;
                        player.HandPower = reserve[index + 1] + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[index] / 4 < f2.Min() / 4 && reserve[index + 1] / 4 < f2.Min())
                    {
                        player.HandType = 5;
                        player.HandPower = f2.Max() + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (reserve[index] % 4 == reserve[index + 1] % 4 && reserve[index] % 4 == f3[0] % 4)
                    {
                        if (reserve[index] / 4 > f3.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[index + 1] / 4 > f3.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index + 1] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[index] / 4 < f3.Max() / 4 && reserve[index + 1] / 4 < f3.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = f3.Max() + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                // different cards in hand
                if (f3.Length == 4) 
                {
                    if (reserve[index] % 4 != reserve[index + 1] % 4 && reserve[index] % 4 == f3[0] % 4)
                    {
                        if (reserve[index] / 4 > f3.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.HandType = 5;
                            player.HandPower = f3.Max() + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[index + 1] % 4 != reserve[index] % 4 && reserve[index + 1] % 4 == f3[0] % 4)
                    {
                        if (reserve[index + 1] / 4 > f3.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index + 1] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.HandType = 5;
                            player.HandPower = f3.Max() + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 5)
                {
                    if (reserve[index] % 4 == f3[0] % 4 && reserve[index] / 4 > f3.Min() / 4)
                    {
                        player.HandType = 5;
                        player.HandPower = reserve[index] + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[index + 1] % 4 == f3[0] % 4 && reserve[index + 1] / 4 > f3.Min() / 4)
                    {
                        player.HandType = 5;
                        player.HandPower = reserve[index + 1] + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[index] / 4 < f3.Min() / 4 && reserve[index + 1] / 4 < f3.Min())
                    {
                        player.HandType = 5;
                        player.HandPower = f3.Max() + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (reserve[index] % 4 == reserve[index + 1] % 4 && reserve[index] % 4 == f4[0] % 4)
                    {
                        if (reserve[index] / 4 > f4.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[index + 1] / 4 > f4.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index + 1] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[index] / 4 < f4.Max() / 4 && reserve[index + 1] / 4 < f4.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = f4.Max() + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                // different cards in hand
                if (f4.Length == 4) 
                {
                    if (reserve[index] % 4 != reserve[index + 1] % 4 && reserve[index] % 4 == f4[0] % 4)
                    {
                        if (reserve[index] / 4 > f4.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.HandType = 5;
                            player.HandPower = f4.Max() + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[index + 1] % 4 != reserve[index] % 4 && reserve[index + 1] % 4 == f4[0] % 4)
                    {
                        if (reserve[index + 1] / 4 > f4.Max() / 4)
                        {
                            player.HandType = 5;
                            player.HandPower = reserve[index + 1] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.HandType = 5;
                            player.HandPower = f4.Max() + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 5)
                {
                    if (reserve[index] % 4 == f4[0] % 4 && reserve[index] / 4 > f4.Min() / 4)
                    {
                        player.HandType = 5;
                        player.HandPower = reserve[index] + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[index + 1] % 4 == f4[0] % 4 && reserve[index + 1] / 4 > f4.Min() / 4)
                    {
                        player.HandType = 5;
                        player.HandPower = reserve[index + 1] + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[index] / 4 < f4.Min() / 4 && reserve[index + 1] / 4 < f4.Min())
                    {
                        player.HandType = 5;
                        player.HandPower = f4.Max() + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                //ace
                if (f1.Length > 0)
                {
                    if (reserve[index] / 4 == 0 && reserve[index] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        player.HandType = 5.5;
                        player.HandPower = 13 + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5.5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[index + 1] / 4 == 0 && reserve[index + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        player.HandType = 5.5;
                        player.HandPower = 13 + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5.5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f2.Length > 0)
                {
                    if (reserve[index] / 4 == 0 && reserve[index] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        player.HandType = 5.5;
                        player.HandPower = 13 + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5.5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[index + 1] / 4 == 0 && reserve[index + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        player.HandType = 5.5;
                        player.HandPower = 13 + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5.5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f3.Length > 0)
                {
                    if (reserve[index] / 4 == 0 && reserve[index] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        player.HandType = 5.5;
                        player.HandPower = 13 + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5.5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[index + 1] / 4 == 0 && reserve[index + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        player.HandType = 5.5;
                        player.HandPower = 13 + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5.5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f4.Length > 0)
                {
                    if (reserve[index] / 4 == 0 && reserve[index] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        player.HandType = 5.5;
                        player.HandPower = 13 + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5.5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[index + 1] / 4 == 0 && reserve[index + 1] % 4 == f4[0] % 4 && vf)
                    {
                        player.HandType = 5.5;
                        player.HandPower = 13 + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 5.5 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        /// <summary>
        /// Sets the player type if his hand type is straight.
        /// </summary>
        public void Straight(IPlayer player, int[] stright, int index, List<Type> winners, ref Type sorted)
        {
            if (player.HandType >= -1)
            {
                var op = stright.Select(o => o / 4).Distinct().ToArray();
                for (index = 0; index < op.Length - 4; index++)
                {
                    if (op[index] + 4 == op[index + 4])
                    {
                        if (op.Max() - 4 == op[index])
                        {
                            player.HandType = 4;
                            player.HandPower = op.Max() + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 4 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            player.HandType = 4;
                            player.HandPower = op[index + 4] + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 4 });
                            sorted =
                                winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }

                    if (op[index] == 0 && op[index + 1] == 9 && op[index + 2] == 10 && op[index + 3] == 11
                        && op[index + 4] == 12)
                    {
                        player.HandType = 4;
                        player.HandPower = 13 + player.HandType * 100;
                        winners.Add(new Type() { Power = player.HandPower, Current = 4 });
                        sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        /// <summary>
        /// Sets the player type if his hand type is three of a kind.
        /// </summary>
        public void ThreeOfAKind(IPlayer player, int[] stright, int index, List<Type> winners, ref Type sorted)
        {
            if (player.HandType >= -1)
            {
                for (index = 0; index <= 12; index++)
                {
                    var fh = stright.Where(o => o / 4 == index).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            player.HandType = 3;
                            player.HandPower = 13 * 3 + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 3 });
                            sorted = winners.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            player.HandType = 3;
                            player.HandPower = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + player.HandType * 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 3 });
                            sorted = winners.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the player type if his hand type is pair of two cards.
        /// </summary>
        public void TwoPair(IPlayer player, int index, List<Type> winners, ref Type sorted, ref int[] reserve)
        {
            if (player.HandType >= -1)
            {
                bool msgbox = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    if (reserve[index] / 4 != reserve[index + 1] / 4)
                    {
                        for (int k = 1; k <= max; k++)
                        {
                            if (tc - k < 12)
                            {
                                max--;
                            }

                            if (tc - k >= 12)
                            {
                                if (reserve[index] / 4 == reserve[tc] / 4
                                    && reserve[index + 1] / 4 == reserve[tc - k] / 4
                                    || reserve[index + 1] / 4 == reserve[tc] / 4
                                    && reserve[index] / 4 == reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (reserve[index] / 4 == 0)
                                        {
                                            player.HandType = 2;
                                            player.HandPower = 13 * 4 + (reserve[index + 1] / 4) * 2 + player.HandType * 100;
                                            winners.Add(new Type() { Power = player.HandPower, Current = 2 });
                                            sorted =
                                                winners.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (reserve[index + 1] / 4 == 0)
                                        {
                                            player.HandType = 2;
                                            player.HandPower = 13 * 4 + (reserve[index] / 4) * 2 + player.HandType * 100;
                                            winners.Add(new Type() { Power = player.HandPower, Current = 2 });
                                            sorted =
                                                winners.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (reserve[index + 1] / 4 != 0 && reserve[index] / 4 != 0)
                                        {
                                            player.HandType = 2;
                                            player.HandPower = (reserve[index] / 4) * 2 + (reserve[index + 1] / 4) * 2
                                                           + player.HandType * 100;
                                            winners.Add(new Type() { Power = player.HandPower, Current = 2 });
                                            sorted =
                                                winners.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }
                                    }

                                    msgbox = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the player type if his hand type is pair of two pairs.
        /// </summary>
        public void PairTwoPair(IPlayer player, int index, List<Type> winners, ref Type sorted, ref int[] reserve)
        {
            if (player.HandType >= -1)
            {
                bool msgbox = false;
                bool msgbox1 = false;
                for (int tc = 16; tc >= 12; tc--)
                {
                    int max = tc - 12;
                    for (int k = 1; k <= max; k++)
                    {
                        if (tc - k < 12)
                        {
                            max--;
                        }

                        if (tc - k >= 12)
                        {
                            if (reserve[tc] / 4 == reserve[tc - k] / 4)
                            {
                                if (reserve[tc] / 4 != reserve[index] / 4 && reserve[tc] / 4 != reserve[index + 1] / 4
                                    && player.HandType == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (reserve[index + 1] / 4 == 0)
                                        {
                                            player.HandType = 2;
                                            player.HandPower = (reserve[index] / 4) * 2 + 13 * 4 + player.HandType * 100;
                                            winners.Add(new Type() { Power = player.HandPower, Current = 2 });
                                            sorted =
                                                winners.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (reserve[index] / 4 == 0)
                                        {
                                            player.HandType = 2;
                                            player.HandPower = (reserve[index + 1] / 4) * 2 + 13 * 4 + player.HandType * 100;
                                            winners.Add(new Type() { Power = player.HandPower, Current = 2 });
                                            sorted =
                                                winners.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (reserve[index + 1] / 4 != 0)
                                        {
                                            player.HandType = 2;
                                            player.HandPower = (reserve[tc] / 4) * 2 + (reserve[index + 1] / 4) * 2
                                                           + player.HandType * 100;
                                            winners.Add(new Type() { Power = player.HandPower, Current = 2 });
                                            sorted =
                                                winners.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (reserve[index] / 4 != 0)
                                        {
                                            player.HandType = 2;
                                            player.HandPower = (reserve[tc] / 4) * 2 + (reserve[index] / 4) * 2
                                                           + player.HandType * 100;
                                            winners.Add(new Type() { Power = player.HandPower, Current = 2 });
                                            sorted =
                                                winners.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }
                                    }

                                    msgbox = true;
                                }

                                if (player.HandType == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (reserve[index] / 4 > reserve[index + 1] / 4)
                                        {
                                            if (reserve[tc] / 4 == 0)
                                            {
                                                player.HandType = 0;
                                                player.HandPower = 13 + reserve[index] / 4 + player.HandType * 100;
                                                winners.Add(new Type() { Power = player.HandPower, Current = 1 });
                                                sorted =
                                                    winners.OrderByDescending(op => op.Current)
                                                        .ThenByDescending(op => op.Power)
                                                        .First();
                                            }
                                            else
                                            {
                                                player.HandType = 0;
                                                player.HandPower = reserve[tc] / 4 + reserve[index] / 4 + player.HandType * 100;
                                                winners.Add(new Type() { Power = player.HandPower, Current = 1 });
                                                sorted =
                                                    winners.OrderByDescending(op => op.Current)
                                                        .ThenByDescending(op => op.Power)
                                                        .First();
                                            }
                                        }
                                        else
                                        {
                                            if (reserve[tc] / 4 == 0)
                                            {
                                                player.HandType = 0;
                                                player.HandPower = 13 + reserve[index + 1] + player.HandType * 100;
                                                winners.Add(new Type() { Power = player.HandPower, Current = 1 });
                                                sorted =
                                                    winners.OrderByDescending(op => op.Current)
                                                        .ThenByDescending(op => op.Power)
                                                        .First();
                                            }
                                            else
                                            {
                                                player.HandType = 0;
                                                player.HandPower = reserve[tc] / 4 + reserve[index + 1] / 4
                                                               + player.HandType * 100;
                                                winners.Add(new Type() { Power = player.HandPower, Current = 1 });
                                                sorted =
                                                    winners.OrderByDescending(op => op.Current)
                                                        .ThenByDescending(op => op.Power)
                                                        .First();
                                            }
                                        }
                                    }

                                    msgbox1 = true;
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Sets the player type if his hand type is pair from hand.
        /// </summary>
        public void PairFromHand(IPlayer player, int index, List<Type> winners, ref Type sorted, ref int[] reserve)
        {
            if (player.HandType >= -1)
            {
                bool msgbox = false;
                if (reserve[index] / 4 == reserve[index + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (reserve[index] / 4 == 0)
                        {
                            player.HandType = 1;
                            player.HandPower = 13 * 4 + 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 1 });
                            sorted = winners.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            player.HandType = 1;
                            player.HandPower = (reserve[index + 1] / 4) * 4 + 100;
                            winners.Add(new Type() { Power = player.HandPower, Current = 1 });
                            sorted = winners.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }

                    msgbox = true;
                }

                for (int tc = 16; tc >= 12; tc--)
                {
                    if (reserve[index + 1] / 4 == reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (reserve[index + 1] / 4 == 0)
                            {
                                player.HandType = 1;
                                player.HandPower = 13 * 4 + reserve[index] / 4 + 100;
                                winners.Add(new Type() { Power = player.HandPower, Current = 1 });
                                sorted =
                                    winners.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                player.HandType = 1;
                                player.HandPower = (reserve[index + 1] / 4) * 4 + reserve[index] / 4 + 100;
                                winners.Add(new Type() { Power = player.HandPower, Current = 1 });
                                sorted =
                                    winners.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }

                        msgbox = true;
                    }

                    if (reserve[index] / 4 == reserve[tc] / 4)
                    {
                        if (!msgbox)
                        {
                            if (reserve[index] / 4 == 0)
                            {
                                player.HandType = 1;
                                player.HandPower = 13 * 4 + reserve[index + 1] / 4 + 100;
                                winners.Add(new Type() { Power = player.HandPower, Current = 1 });
                                sorted =
                                    winners.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                player.HandType = 1;
                                player.HandPower = (reserve[tc] / 4) * 4 + reserve[index + 1] / 4 + 100;
                                winners.Add(new Type() { Power = player.HandPower, Current = 1 });
                                sorted =
                                    winners.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }

                        msgbox = true;
                    }
                }
            }
        }

        /// <summary>
        /// Sets the player type if his hand is high card.
        /// </summary>
        public void HighCard(IPlayer player, int index, List<Type> winners, ref Type sorted, ref int[] reserve)
        {
            if (player.HandType == -1)
            {
                if (reserve[index] / 4 > reserve[index + 1] / 4)
                {
                    player.HandType = -1;
                    player.HandPower = reserve[index] / 4;
                    winners.Add(new Type() { Power = player.HandPower, Current = -1 });
                    sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    player.HandType = -1;
                    player.HandPower = reserve[index + 1] / 4;
                    winners.Add(new Type() { Power = player.HandPower, Current = -1 });
                    sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }

                if (reserve[index] / 4 == 0 || reserve[index + 1] / 4 == 0)
                {
                    player.HandType = -1;
                    player.HandPower = 13;
                    winners.Add(new Type() { Power = player.HandPower, Current = -1 });
                    sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }

        private static void GetValue(IPlayer player, int[] colour, List<Type> winners, out Type sorted)
        {
            player.HandPower = colour.Max() / 4 + player.HandType * 100;
            if (colour[0] + 4 == colour[4])
            {
                //Straight Flush
                player.HandType = 8;
                winners.Add(new Type() { Power = player.HandPower, Current = player.HandType });
            }

            if (colour[0] == 0 && colour[1] == 9 && colour[2] == 10 && colour[3] == 11 && colour[4] == 12)
            {
                //Royal Straight Flush
                player.HandType = 9;
                winners.Add(new Type() { Power = player.HandPower, Current = player.HandType });
            }

            sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        }

        private static Type GetWinner(IPlayer player, List<Type> winners)
        {
            player.HandType = 6;
            player.HandPower = 13 * 2 + player.HandType * 100;
            winners.Add(new Type() { Power = player.HandPower, Current = 6 });
            var sorted = winners.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
            return sorted;
        }
    }
}
