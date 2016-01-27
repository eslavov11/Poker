namespace Poker.Interfaces
{
    using System.Collections.Generic;
    using Models;

    /// <summary>
    /// Interface which contains methods with different kinds of poker hand types.
    /// </summary>
    public interface IAssertHandType
    {
        void StraightFlush(IPlayer player, int[] clubes, int[] dimonds, int[] hearts, int[] spades, List<Type> winners, ref Type sorted);

        void FourOfAKind(IPlayer player, int[] straight, List<Type> winners, ref Type sorted);

        void FullHouse(IPlayer player, ref bool done, int[] straight, List<Type> winners, ref Type sorted, ref double type);

        void Flush(IPlayer player, ref bool vf, int[] straight1, ref int index, List<Type> winners, ref Type sorted, ref int[] reserve);

        void Straight(IPlayer player, int[] stright, int index, List<Type> winners, ref Type sorted);

        void ThreeOfAKind(IPlayer player, int[] stright, int index, List<Type> winners, ref Type sorted);

        void TwoPair(IPlayer player, int index, List<Type> winners, ref Type sorted, ref int[] reserve);

        void PairTwoPair(IPlayer player, int index, List<Type> winners, ref Type sorted, ref int[] reserve);

        void PairFromHand(IPlayer player, int index, List<Type> winners, ref Type sorted, ref int[] reserve);

        void HighCard(IPlayer player, int index, List<Type> winners, ref Type sorted, ref int[] reserve);
    }
}
