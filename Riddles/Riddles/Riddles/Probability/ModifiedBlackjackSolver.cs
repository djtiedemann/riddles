using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability
{
    /// <summary>
    /// You're playing a modified version of Blackjack where the cards
    /// are 1-10 only, 1 serves as only a 1. You draw cards until you hit
    /// 21, what are the chances that you do that
    /// </summary>
    public class ModifiedBlackjackSolver
    {
        public double CalculateOddsOfHitting21Exactly(
            List<int> cards, 
            bool isConservativeStrategy,
            int targetScore)
        {
            var winningCombos = new List<List<int>>();
            return this.CalculateOddsOfHitting21Exactly(
                cards,
                isConservativeStrategy,
                targetScore,
                0,
                new List<int>(),
                winningCombos
            );
        }

        private double CalculateOddsOfHitting21Exactly(
            List<int> cards,
            bool isConservativeStrategy,
            int targetScore,
            double currentScore,
            List<int> currentCards,
            List<List<int>> winningCombos)
        {
            if(currentScore > targetScore)
            {
                return 0;
            }
            if(currentScore == targetScore)
            {
                winningCombos.Add(currentCards.ToList());
                return 1;
            }
            if(isConservativeStrategy 
                && currentScore + cards.Max() > targetScore)
            {
                return 0;
            }
            var probabilityOfWinning = 0.0;
            var cardsCopy = cards.ToList();
            foreach(var card in cards)
            {
                cardsCopy.Remove(card);
                currentCards.Add(card);
                probabilityOfWinning += 
                    this.CalculateOddsOfHitting21Exactly(
                        cardsCopy,
                        isConservativeStrategy,
                        targetScore,
                        currentScore + card,
                        currentCards,
                        winningCombos
                    ) / ((double)cards.Count);
                currentCards.Remove(card);
                cardsCopy.Add(card);
            }
            return probabilityOfWinning;
        }
    }
}
