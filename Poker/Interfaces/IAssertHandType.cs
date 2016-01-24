namespace Poker.Interfaces
{
    using System.Collections.Generic;

    public interface IAssertHandType
    {
        void rStraightFlush(IPlayer player, int[] clubes, int[] dimonds, int[] hearts, int[] spades, List<Type> Win, ref Type sorted);

        void rFourOfAKind(IPlayer player, int[] Straight, List<Type> Win, ref Type sorted);

        void rFullHouse(IPlayer player, ref bool done, int[] Straight, List<Type> Win, ref Type sorted, ref double type);

        void rFlush(IPlayer player, ref bool vf, int[] Straight1, ref int index, List<Type> Win, ref Type sorted, ref int[] Reserve);

        void rStraight(IPlayer player, int[] Straight, int index, List<Type> Win, ref Type sorted);

        void rThreeOfAKind(IPlayer player, int[] Straight, int index, List<Type> Win, ref Type sorted);

        void rTwoPair(IPlayer player, int index, List<Type> Win, ref Type sorted, ref int[] Reserve);

        void rPairTwoPair(IPlayer player, int index, List<Type> Win, ref Type sorted, ref int[] Reserve);

        void rPairFromHand(IPlayer player, int index, List<Type> Win, ref Type sorted, ref int[] Reserve);

        void rHighCard(IPlayer player, int index, List<Type> Win, ref Type sorted, ref int[] Reserve);

    }
}
