using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riddles.Caching;

namespace Riddles.Probability
{
    public class NeapolitanSolitaireSolver
    {

        public NeapolitanSolitaireSolver() { 
            
        }
        public double CalculateOddsOfSuccess()
        {
            return 0;
        }
    }

    public class SolitaireState
    {
        Dictionary<string, int> RemainingCardsOfType { get; }
        public SolitaireState(Dictionary<string, int> remainingCardsOfType)
        {
            this.RemainingCardsOfType = remainingCardsOfType;
        }
    }
}
