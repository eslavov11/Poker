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
        /// Performs the needed actions if the player's hand is straight flush.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="clubes">The clubes.</param>
        /// <param name="dimonds">The dimonds.</param>
        /// <param name="hearts">The hearts.</param>
        /// <param name="spades">The spades.</param>
        /// <param name="winners">The winners.</param>
        void StraightFlush(IPlayer player, int[] clubes, int[] dimonds, int[] hearts, int[] spades, List<Type> winners, ref Type sorted);

        /// <summary>
        /// Performs the needed actions if the player's hand is four of a kind.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="winners">The winners.</param>
        /// <param name="sorted">The sorted.</param>
        void FourOfAKind(IPlayer player, int[] straight, List<Type> winners, ref Type sorted);

        /// <summary>
        /// Performs the needed actions if the player's hand is full house.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="winners">The winners.</param>
        /// <param name="sorted">The sorted.</param>
        /// <param name="type">The type.</param>
        void FullHouse(IPlayer player, ref bool done, int[] straight, List<Type> winners, ref Type sorted, ref double type);
        
        /// <summary>
        /// Performs the needed actions if the player's hand is flush.
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
