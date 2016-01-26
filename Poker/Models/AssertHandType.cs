namespace Poker.Models
{
    using System.Collections.Generic;
    using System.Linq;
    using Interfaces;

    public class AssertHandType : IAssertHandType
    {
        public void StraightFlush(IPlayer player, int[] clubes, int[] dimonds, int[] hearts, int[] spades, List<Type> win, ref Type sorted)
        {
            if (player.Type >= -1)
            {
                if (clubes.Length >= 5)
                {
                    GetValue(player, clubes, win, out sorted);
                }

                if (dimonds.Length >= 5)
                {
                    GetValue(player, dimonds, win, out sorted);
                }

                if (hearts.Length >= 5)
                {
                    GetValue(player, hearts, win, out sorted);
                }

                if (spades.Length >= 5)
                {
                    GetValue(player, spades, win, out sorted);
                }
            }
        }

        private static void GetValue(IPlayer player, int[] colour, List<Type> Win, out Type sorted)
        {
            player.Power = (colour.Max()) / 4 + player.Type * 100;
            if (colour[0] + 4 == colour[4])
            {
                //Straight Flush
                player.Type = 8;
                Win.Add(new Type() { Power = player.Power, Current = player.Type });
            }

            if (colour[0] == 0 && colour[1] == 9 && colour[2] == 10 && colour[3] == 11 && colour[4] == 12)
            {
                //Royal Straight Flush
                player.Type = 9;
                Win.Add(new Type() { Power = player.Power, Current = player.Type });
            }
            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
        }

        public void FourOfAKind(IPlayer player, int[] straight, List<Type> win, ref Type sorted)
        {
            if (player.Type >= -1)
            {
                for (int j = 0; j <= 3; j++)
                {
                    if (straight[j] / 4 == straight[j + 1] / 4 &&
                        straight[j] / 4 == straight[j + 2] / 4 &&
                        straight[j] / 4 == straight[j + 3] / 4)
                    {
                        player.Type = 7;
                        player.Power = (straight[j] / 4) * 4 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 7 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (straight[j] / 4 == 0 &&
                        straight[j + 1] / 4 == 0 &&
                        straight[j + 2] / 4 == 0 &&
                        straight[j + 3] / 4 == 0)
                    {
                        player.Type = 7;
                        player.Power = 13 * 4 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 7 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void FullHouse(IPlayer player, ref bool done, int[] straight, List<Type> win, ref Type sorted, ref double type)
        {
            if (player.Type >= -1)
            {
                type = player.Power;
                for (int j = 0; j <= 12; j++)
                {
                    var fh = straight.Where(o => o / 4 == j).ToArray();
                    if (fh.Length == 3 || done)
                    {
                        if (fh.Length == 2)
                        {
                            if (fh.Max() / 4 == 0)
                            {
                                sorted = NewMethod(player, win);
                                break;
                            }

                            if (fh.Max() / 4 > 0)
                            {
                                player.Type = 6;
                                player.Power = fh.Max() / 4 * 2 + player.Type * 100;
                                win.Add(new Type() { Power = player.Power, Current = 6 });
                                sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                                break;
                            }
                        }

                        if (done)
                        {
                            continue;
                        }

                        if (fh.Max() / 4 == 0)
                        {
                            player.Power = 13;
                            done = true;
                            j = -1;
                        }
                        else
                        {
                            player.Power = fh.Max() / 4;
                            done = true;
                            j = -1;
                        }
                    }
                }

                if (player.Type != 6)
                {
                    player.Power = type;
                }
            }
        }

        private static Type NewMethod(IPlayer player, List<Type> Win)
        {
            Type sorted;
            player.Type = 6;
            player.Power = 13 * 2 + player.Type * 100;
            Win.Add(new Type() { Power = player.Power, Current = 6 });
            sorted = Win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
            return sorted;
        }

        public void Flush(IPlayer player, ref bool vf, int[] straight1, ref int index, List<Type> win, ref Type sorted, ref int[] reserve)
        {
            if (player.Type >= -1)
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
                            player.Type = 5;
                            player.Power = reserve[index] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[index + 1] / 4 > f1.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index + 1] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[index] / 4 < f1.Max() / 4 && reserve[index + 1] / 4 < f1.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = f1.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 4)//different cards in hand
                {
                    if (reserve[index] % 4 != reserve[index + 1] % 4 && reserve[index] % 4 == f1[0] % 4)
                    {
                        if (reserve[index] / 4 > f1.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f1.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[index + 1] % 4 != reserve[index] % 4 && reserve[index + 1] % 4 == f1[0] % 4)
                    {
                        if (reserve[index + 1] / 4 > f1.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index + 1] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f1.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f1.Length == 5)
                {
                    if (reserve[index] % 4 == f1[0] % 4 && reserve[index] / 4 > f1.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[index] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[index + 1] % 4 == f1[0] % 4 && reserve[index + 1] / 4 > f1.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[index + 1] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[index] / 4 < f1.Min() / 4 && reserve[index + 1] / 4 < f1.Min())
                    {
                        player.Type = 5;
                        player.Power = f1.Max() + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f2.Length == 3 || f2.Length == 4)
                {
                    if (reserve[index] % 4 == reserve[index + 1] % 4 && reserve[index] % 4 == f2[0] % 4)
                    {
                        if (reserve[index] / 4 > f2.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[index + 1] / 4 > f2.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index + 1] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[index] / 4 < f2.Max() / 4 && reserve[index + 1] / 4 < f2.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = f2.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 4)//different cards in hand
                {
                    if (reserve[index] % 4 != reserve[index + 1] % 4 && reserve[index] % 4 == f2[0] % 4)
                    {
                        if (reserve[index] / 4 > f2.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f2.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[index + 1] % 4 != reserve[index] % 4 && reserve[index + 1] % 4 == f2[0] % 4)
                    {
                        if (reserve[index + 1] / 4 > f2.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index + 1] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f2.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f2.Length == 5)
                {
                    if (reserve[index] % 4 == f2[0] % 4 && reserve[index] / 4 > f2.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[index] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[index + 1] % 4 == f2[0] % 4 && reserve[index + 1] / 4 > f2.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[index + 1] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[index] / 4 < f2.Min() / 4 && reserve[index + 1] / 4 < f2.Min())
                    {
                        player.Type = 5;
                        player.Power = f2.Max() + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f3.Length == 3 || f3.Length == 4)
                {
                    if (reserve[index] % 4 == reserve[index + 1] % 4 && reserve[index] % 4 == f3[0] % 4)
                    {
                        if (reserve[index] / 4 > f3.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[index + 1] / 4 > f3.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index + 1] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[index] / 4 < f3.Max() / 4 && reserve[index + 1] / 4 < f3.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = f3.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 4)//different cards in hand
                {
                    if (reserve[index] % 4 != reserve[index + 1] % 4 && reserve[index] % 4 == f3[0] % 4)
                    {
                        if (reserve[index] / 4 > f3.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f3.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[index + 1] % 4 != reserve[index] % 4 && reserve[index + 1] % 4 == f3[0] % 4)
                    {
                        if (reserve[index + 1] / 4 > f3.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index + 1] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f3.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f3.Length == 5)
                {
                    if (reserve[index] % 4 == f3[0] % 4 && reserve[index] / 4 > f3.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[index] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[index + 1] % 4 == f3[0] % 4 && reserve[index + 1] / 4 > f3.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[index + 1] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[index] / 4 < f3.Min() / 4 && reserve[index + 1] / 4 < f3.Min())
                    {
                        player.Type = 5;
                        player.Power = f3.Max() + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                if (f4.Length == 3 || f4.Length == 4)
                {
                    if (reserve[index] % 4 == reserve[index + 1] % 4 && reserve[index] % 4 == f4[0] % 4)
                    {
                        if (reserve[index] / 4 > f4.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }

                        if (reserve[index + 1] / 4 > f4.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index + 1] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else if (reserve[index] / 4 < f4.Max() / 4 && reserve[index + 1] / 4 < f4.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = f4.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 4)//different cards in hand
                {
                    if (reserve[index] % 4 != reserve[index + 1] % 4 && reserve[index] % 4 == f4[0] % 4)
                    {
                        if (reserve[index] / 4 > f4.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f4.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }

                    if (reserve[index + 1] % 4 != reserve[index] % 4 && reserve[index + 1] % 4 == f4[0] % 4)
                    {
                        if (reserve[index + 1] / 4 > f4.Max() / 4)
                        {
                            player.Type = 5;
                            player.Power = reserve[index + 1] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                        else
                        {
                            player.Type = 5;
                            player.Power = f4.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 5 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                            vf = true;
                        }
                    }
                }

                if (f4.Length == 5)
                {
                    if (reserve[index] % 4 == f4[0] % 4 && reserve[index] / 4 > f4.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[index] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }

                    if (reserve[index + 1] % 4 == f4[0] % 4 && reserve[index + 1] / 4 > f4.Min() / 4)
                    {
                        player.Type = 5;
                        player.Power = reserve[index + 1] + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                    else if (reserve[index] / 4 < f4.Min() / 4 && reserve[index + 1] / 4 < f4.Min())
                    {
                        player.Type = 5;
                        player.Power = f4.Max() + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        vf = true;
                    }
                }

                //ace
                if (f1.Length > 0)
                {
                    if (reserve[index] / 4 == 0 && reserve[index] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[index + 1] / 4 == 0 && reserve[index + 1] % 4 == f1[0] % 4 && vf && f1.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f2.Length > 0)
                {
                    if (reserve[index] / 4 == 0 && reserve[index] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[index + 1] / 4 == 0 && reserve[index + 1] % 4 == f2[0] % 4 && vf && f2.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f3.Length > 0)
                {
                    if (reserve[index] / 4 == 0 && reserve[index] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[index + 1] / 4 == 0 && reserve[index + 1] % 4 == f3[0] % 4 && vf && f3.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }

                if (f4.Length > 0)
                {
                    if (reserve[index] / 4 == 0 && reserve[index] % 4 == f4[0] % 4 && vf && f4.Length > 0)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }

                    if (reserve[index + 1] / 4 == 0 && reserve[index + 1] % 4 == f4[0] % 4 && vf)
                    {
                        player.Type = 5.5;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 5.5 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void Straight(IPlayer player, int[] stright, int index, List<Type> win, ref Type sorted)
        {

            if (player.Type >= -1)
            {
                var op = stright.Select(o => o / 4).Distinct().ToArray();
                for (index = 0; index < op.Length - 4; index++)
                {
                    if (op[index] + 4 == op[index + 4])
                    {
                        if (op.Max() - 4 == op[index])
                        {
                            player.Type = 4;
                            player.Power = op.Max() + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 4 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                        else
                        {
                            player.Type = 4;
                            player.Power = op[index + 4] + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 4 });
                            sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                        }
                    }

                    if (op[index] == 0 && op[index + 1] == 9 && op[index + 2] == 10 && op[index + 3] == 11 && op[index + 4] == 12)
                    {
                        player.Type = 4;
                        player.Power = 13 + player.Type * 100;
                        win.Add(new Type() { Power = player.Power, Current = 4 });
                        sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                    }
                }
            }
        }

        public void ThreeOfAKind(IPlayer player, int[] stright, int index, List<Type> win, ref Type sorted)
        {
            if (player.Type >= -1)
            {
                for (index = 0; index <= 12; index++)
                {
                    var fh = stright.Where(o => o / 4 == index).ToArray();
                    if (fh.Length == 3)
                    {
                        if (fh.Max() / 4 == 0)
                        {
                            player.Type = 3;
                            player.Power = 13 * 3 + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 3 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            player.Type = 3;
                            player.Power = fh[0] / 4 + fh[1] / 4 + fh[2] / 4 + player.Type * 100;
                            win.Add(new Type() { Power = player.Power, Current = 3 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                    }
                }
            }
        }

        public void TwoPair(IPlayer player, int index, List<Type> win, ref Type sorted, ref int[] reserve)
        {
            if (player.Type >= -1)
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
                                if (reserve[index] / 4 == reserve[tc] / 4 && reserve[index + 1] / 4 == reserve[tc - k] / 4 ||
                                    reserve[index + 1] / 4 == reserve[tc] / 4 && reserve[index] / 4 == reserve[tc - k] / 4)
                                {
                                    if (!msgbox)
                                    {
                                        if (reserve[index] / 4 == 0)
                                        {
                                            player.Type = 2;
                                            player.Power = 13 * 4 + (reserve[index + 1] / 4) * 2 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted =
                                                    win.OrderByDescending(op => op.Current)
                                                    .ThenByDescending(op => op.Power)
                                                    .First();
                                        }

                                        if (reserve[index + 1] / 4 == 0)
                                        {
                                            player.Type = 2;
                                            player.Power = 13 * 4 + (reserve[index] / 4) * 2 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[index + 1] / 4 != 0 && reserve[index] / 4 != 0)
                                        {
                                            player.Type = 2;
                                            player.Power = (reserve[index] / 4) * 2 + (reserve[index + 1] / 4) * 2 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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

        public void PairTwoPair(IPlayer player, int index, List<Type> win, ref Type sorted, ref int[] reserve)
        {
            if (player.Type >= -1)
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
                                if (reserve[tc] / 4 != reserve[index] / 4 && reserve[tc] / 4 != reserve[index + 1] / 4 && player.Type == 1)
                                {
                                    if (!msgbox)
                                    {
                                        if (reserve[index + 1] / 4 == 0)
                                        {
                                            player.Type = 2;
                                            player.Power = (reserve[index] / 4) * 2 + 13 * 4 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[index] / 4 == 0)
                                        {
                                            player.Type = 2;
                                            player.Power = (reserve[index + 1] / 4) * 2 + 13 * 4 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[index + 1] / 4 != 0)
                                        {
                                            player.Type = 2;
                                            player.Power = (reserve[tc] / 4) * 2 + (reserve[index + 1] / 4) * 2 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }

                                        if (reserve[index] / 4 != 0)
                                        {
                                            player.Type = 2;
                                            player.Power = (reserve[tc] / 4) * 2 + (reserve[index] / 4) * 2 + player.Type * 100;
                                            win.Add(new Type() { Power = player.Power, Current = 2 });
                                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                        }
                                    }

                                    msgbox = true;
                                }

                                if (player.Type == -1)
                                {
                                    if (!msgbox1)
                                    {
                                        if (reserve[index] / 4 > reserve[index + 1] / 4)
                                        {
                                            if (reserve[tc] / 4 == 0)
                                            {
                                                player.Type = 0;
                                                player.Power = 13 + reserve[index] / 4 + player.Type * 100;
                                                win.Add(new Type() { Power = player.Power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                player.Type = 0;
                                                player.Power = reserve[tc] / 4 + reserve[index] / 4 + player.Type * 100;
                                                win.Add(new Type() { Power = player.Power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                        }
                                        else
                                        {
                                            if (reserve[tc] / 4 == 0)
                                            {
                                                player.Type = 0;
                                                player.Power = 13 + reserve[index + 1] + player.Type * 100;
                                                win.Add(new Type() { Power = player.Power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                                            }
                                            else
                                            {
                                                player.Type = 0;
                                                player.Power = reserve[tc] / 4 + reserve[index + 1] / 4 + player.Type * 100;
                                                win.Add(new Type() { Power = player.Power, Current = 1 });
                                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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

        public void PairFromHand(IPlayer player, int index, List<Type> win, ref Type sorted, ref int[] reserve)
        {
            if (player.Type >= -1)
            {
                bool msgbox = false;
                if (reserve[index] / 4 == reserve[index + 1] / 4)
                {
                    if (!msgbox)
                    {
                        if (reserve[index] / 4 == 0)
                        {
                            player.Type = 1;
                            player.Power = 13 * 4 + 100;
                            win.Add(new Type() { Power = player.Power, Current = 1 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                        }
                        else
                        {
                            player.Type = 1;
                            player.Power = (reserve[index + 1] / 4) * 4 + 100;
                            win.Add(new Type() { Power = player.Power, Current = 1 });
                            sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                                player.Type = 1;
                                player.Power = 13 * 4 + reserve[index] / 4 + 100;
                                win.Add(new Type() { Power = player.Power, Current = 1 });
                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                player.Type = 1;
                                player.Power = (reserve[index + 1] / 4) * 4 + reserve[index] / 4 + 100;
                                win.Add(new Type() { Power = player.Power, Current = 1 });
                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
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
                                player.Type = 1;
                                player.Power = 13 * 4 + reserve[index + 1] / 4 + 100;
                                win.Add(new Type() { Power = player.Power, Current = 1 });
                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                            else
                            {
                                player.Type = 1;
                                player.Power = (reserve[tc] / 4) * 4 + reserve[index + 1] / 4 + 100;
                                win.Add(new Type() { Power = player.Power, Current = 1 });
                                sorted = win.OrderByDescending(op => op.Current).ThenByDescending(op => op.Power).First();
                            }
                        }
                        msgbox = true;
                    }
                }
            }
        }

        public void HighCard(IPlayer player, int index, List<Type> win, ref Type sorted, ref int[] reserve)
        {
            if (player.Type == -1)
            {
                if (reserve[index] / 4 > reserve[index + 1] / 4)
                {
                    player.Type = -1;
                    player.Power = reserve[index] / 4;
                    win.Add(new Type() { Power = player.Power, Current = -1 });
                    sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
                else
                {
                    player.Type = -1;
                    player.Power = reserve[index + 1] / 4;
                    win.Add(new Type() { Power = player.Power, Current = -1 });
                    sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }

                if (reserve[index] / 4 == 0 || reserve[index + 1] / 4 == 0)
                {
                    player.Type = -1;
                    player.Power = 13;
                    win.Add(new Type() { Power = player.Power, Current = -1 });
                    sorted = win.OrderByDescending(op1 => op1.Current).ThenByDescending(op1 => op1.Power).First();
                }
            }
        }
    }
}
