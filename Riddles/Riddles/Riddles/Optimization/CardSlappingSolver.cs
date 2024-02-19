using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Optimization
{
    /// <summary>
    /// https://thefiddler.substack.com/p/can-you-sum-some-cards
    /// You are playing a game with cards from 1 to n. You have infinitely many of each
    /// If at any point a sequence of cards has the same sum as a sequence immediately
    /// before it, you can slap it.
    /// 
    /// What is the largest sequence of cards that could never be slapped
    /// </summary>
    public class CardSlappingSolver
    {
        public CardSlappingSolver()
        {

        }

        public List<int> FindLongestSequenceThatIsNeverSlappable(int numCards)
        {
            var currentSequence = new List<int>();
            var longestSequence = new List<int>();
            var finalLongestSequence = this.FindLongestSequenceThatIsNeverSlappable(
                numCards,
                currentSequence,
                longestSequence
            );
            finalLongestSequence.RemoveAt(finalLongestSequence.Count - 1);
            return finalLongestSequence;
        }

        private List<int> FindLongestSequenceThatIsNeverSlappable(
            int numCards,
            List<int> currentSequence,
            List<int> currentLongestSequence)
        {
            if(this.CanSequenceBeSlapped(currentSequence))
            {
                if(currentSequence.Count > currentLongestSequence.Count)
                {
                    currentLongestSequence = currentSequence.ToList();
                }
                return currentLongestSequence;
            }
            for(int i=1; i<=numCards; i++)
            {
                currentSequence.Add(i);
                var potentialLongestSequence = 
                    this.FindLongestSequenceThatIsNeverSlappable(
                        numCards,
                        currentSequence,
                        currentLongestSequence
                    );
                if(potentialLongestSequence.Count > currentLongestSequence.Count)
                {
                    currentLongestSequence = potentialLongestSequence;
                }
                currentSequence.RemoveAt(currentSequence.Count-1);
            }
            return currentLongestSequence;
        }

        /**
         * The sequence can be slapped if any group of recent cards
         * sums to a group of cards played immediately before it.
         * 
         * Cards are played to the end of the list
         */
        public bool CanSequenceBeSlapped(List<int> stackOfCards)
        {
            var sumOfRecentCards = 0;
            for(int i=stackOfCards.Count-1; i >= 0; i--)
            {
                sumOfRecentCards += stackOfCards[i];
                var sumOfOtherCards = 0;
                for(int j=i-1; j >= 0; j--)
                {
                    sumOfOtherCards += stackOfCards[j];
                    // if the sums are equal, then we found the grouping, return true
                    if(sumOfRecentCards == sumOfOtherCards)
                    {
                        return true;
                    }
                    // if the j group of cards is greater than the i group,
                    // adding more cards won't help. break out of loop and increase i
                    if(sumOfOtherCards > sumOfRecentCards)
                    {
                        break;
                    }
                    // if the j group of cards is less than the i group and we've
                    // run out of cards. Return false, adding cards to the i group
                    // won't help
                    if(j == 0)
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
