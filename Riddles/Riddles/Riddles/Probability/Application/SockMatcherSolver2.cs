using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability.Application
{
    // https://thefiddler.substack.com/p/can-you-find-a-matching-pair-of-socks
    // If you're trying to optimize your odds of drawing exactly enough
    // socks to get a pair, how many do you draw
    public class SockMatcherSolver2
    {

        public (int, double) CalculateIdealNumberOfSocksToDraw(int numPairs)
        {
            var probabilities = this
                .CalculateIdealNumberOfSocksToDrawInternal(numPairs);
            var bestProbability = probabilities.Select(x => x.Item2).Max();
            var bestOptions = probabilities.Where(x => x.Item2 == bestProbability);
            return (bestOptions.OrderBy(x => x.Item1).First());
        }
        public List<(int, double)> CalculateIdealNumberOfSocksToDrawInternal(
            int numPairs)
        {
            double oddsOfAlreadyDrawingPair = 0.0;
            double numSingleSocksRemaining = 0;
            double numPairsRemaining = numPairs;
            List<(int, double)> probabilities = new List<(int, double)> ();
            for(int i=1; i<=(numPairs + 1); i++)
            {
                if(numPairsRemaining < 0)
                {
                    break;
                }
                var oddsOfDrawingAPairThisRound = (double)numSingleSocksRemaining
                    / (numSingleSocksRemaining + 2 * numPairsRemaining);
                var probabilityOfMakingFirstPairThisRound =
                    (1 - oddsOfAlreadyDrawingPair) * oddsOfDrawingAPairThisRound;
                probabilities.Add((i, probabilityOfMakingFirstPairThisRound));
                oddsOfAlreadyDrawingPair += probabilityOfMakingFirstPairThisRound;
                numPairsRemaining--;
                numSingleSocksRemaining++;
            }
            return probabilities;
        }
    }
}
