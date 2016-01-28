namespace Poker.Interfaces
{
    using System.Collections.Generic;
    using Models;

    /// <summary>
    /// Interface which contains methods with different kinds of poker hand types.
    /// </summary>
    public interface IAssertHandType
    {

        /// <summary>
        /// Sets the player type if his hand type is straight flush.
        /// </summary>
        void StraightFlush(IPlayer player, int[] clubes, int[] dimonds, int[] hearts, int[] spades, List<Type> winners, ref Type sorted);


        /// <summary>
        /// Sets the player type if his hand type is four of a kind.
        /// </summary>
        void FourOfAKind(IPlayer player, int[] straight, List<Type> winners, ref Type sorted);


        /// <summary>
        /// Sets the player type if his hand type is full house.
        /// </summary>
        void FullHouse(IPlayer player, ref bool done, int[] straight, List<Type> winners, ref Type sorted, ref double type);


        /// <summary>
        /// Sets the player type if his hand type is flush.
        /// </summary>
        void Flush(IPlayer player, ref bool vf, int[] straight1, ref int index, List<Type> winners, ref Type sorted, ref int[] reserve);

        /// <summary>
        /// Performs the needed actions if the player's hand is straight.
        /// </summary>
        void Straight(IPlayer player, int[] stright, int index, List<Type> winners, ref Type sorted);

        /// <summary>
        /// Performs the needed actions if the player's hand is three of a kind.
        /// </summary>
        void ThreeOfAKind(IPlayer player, int[] stright, int index, List<Type> winners, ref Type sorted);

        /// <summary>
        /// Performs the needed actions if the player's hand is two pairs.
        /// </summary>
        void TwoPair(IPlayer player, int index, List<Type> winners, ref Type sorted, ref int[] reserve);

        /// <summary>
        /// Performs the needed actions if the player's hand is pair of two pairs.
        /// </summary>
        void PairTwoPair(IPlayer player, int index, List<Type> winners, ref Type sorted, ref int[] reserve);

        /// <summary>
        /// Performs the needed actions if the player's hand is pair from hand.
        /// </summary>
        void PairFromHand(IPlayer player, int index, List<Type> winners, ref Type sorted, ref int[] reserve);

        /// <summary>
        /// Performs the needed actions if the player's hand is high card.
        /// </summary>
        void HighCard(IPlayer player, int index, List<Type> winners, ref Type sorted, ref int[] reserve);
    }
}
