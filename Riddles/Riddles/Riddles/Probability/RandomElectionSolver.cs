using System;
using System.Collections.Generic;
using System.Text;

using Riddles.Combinatorics.Core;

namespace Riddles.Probability
{
    // https://thefiddler.substack.com/p/how-long-would-it-take-to-pick-a
    // If all electors in the Elephant Party randomly vote for a speaker
    // across the candidates, what is the expected number of rounds to elect them
    //
    // If no candidate obtains a majority, the candidate with the fewest votes
    // is eliminated
    public class RandomElectionSolver
    {
        private BinomialTheoremCalculator _binomialTheoremCalculator;
        public RandomElectionSolver()
        {
            this._binomialTheoremCalculator = new BinomialTheoremCalculator();
        }

        public double CalculateExpectedNumberOfRoundsToRandomlyElect(
            int numElectors,
            int numCandidates
        )
        {
            var expectedValue = 0.0;
            var oddsOfReachingRound = 1.0;
            for(int numCandidatesInRound=numCandidates; numCandidatesInRound>1; numCandidatesInRound--)
            {
                var oddsOfSelectingSpeaker = this
                    .CalculateProbabilityOfElectingSpeaker(numElectors, numCandidatesInRound);
                int roundCount = (numCandidates - numCandidatesInRound) + 1;
                expectedValue += roundCount * oddsOfReachingRound * oddsOfSelectingSpeaker;
                oddsOfReachingRound -= oddsOfSelectingSpeaker;
            }
            return expectedValue;
        }

        private double CalculateProbabilityOfElectingSpeaker(
            int numElectors,
            int numCandidates
        )
        {
            int numElectorsForMajority = (numElectors + 1) / 2;

            var probabilityOfSelectingCandidate = 0.0;
            // pick the number of votes that are voting for the winning candidate
            for(int majoritySize=numElectorsForMajority; majoritySize<=numElectors; majoritySize++)
            {
                probabilityOfSelectingCandidate +=
                    // calculate the number of ways to assign the electors
                    // in the majority
                    this._binomialTheoremCalculator.CalculateBinomialCoefficient(
                        numElectors,
                        majoritySize
                    ) 
                    // calculate the probability of that majority voting for the
                    // same candidate
                    * Math.Pow(1.0/numCandidates, majoritySize) 
                    // calculate the probability of the minority voting for a
                    // different candidate
                    * Math.Pow((numCandidates-1.0)/numCandidates, numElectors-majoritySize);
            }
            // multiply by the number of candidates because any of them can win
            probabilityOfSelectingCandidate *= numCandidates;
            return probabilityOfSelectingCandidate;
        }
    }
}
