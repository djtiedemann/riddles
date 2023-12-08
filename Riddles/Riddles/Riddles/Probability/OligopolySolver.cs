using Riddles.Probability.DistributionGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability
{
    // https://thefiddler.substack.com/p/can-you-race-around-the-monopoly
    // Given a monopoly board and 1 pass, which is the most likely space
    // to land on at some point
    public class OligopolySolver
    {
        private CumulativeSumOfIndependentTrialsDistributionGenerator _distributionGenerator;
        public OligopolySolver() {
            this._distributionGenerator = new CumulativeSumOfIndependentTrialsDistributionGenerator();
        }

        public (int, double) GetMostLikelyLandingSpace(
            int numSpaces, 
            int numDiceRolled, 
            int sizeOfDice)
        {
            var probabilityOfLandingOnEachSpace =
                this.GetProbabilityOfLandingOnEachSpace(
                    numSpaces,
                    numDiceRolled,
                    sizeOfDice
                );
            int bestPosition = -1;
            double bestProbability = 0.0;
            for(int space=1; space<=numSpaces; space++)
            {
                if (probabilityOfLandingOnEachSpace[space] > bestProbability)
                {
                    bestPosition = space;
                    bestProbability = probabilityOfLandingOnEachSpace[space];
                }
            }
            return (bestPosition, bestProbability);
        }

        public Dictionary<int, double> GetProbabilityOfLandingOnEachSpace(
            int numSpaces, 
            int numDiceRolled, 
            int sizeOfDice) {

            var probabilityOfVisitingEachSpace = new Dictionary<int, double>();
            var diceProbability = Enumerable
                .Range(1, sizeOfDice)
                .ToDictionary(i => i, i => 1.0/(double)sizeOfDice);
            var probabilityDistribution = this._distributionGenerator
                .GetOutcomeProbabilityDistribution(diceProbability, numDiceRolled);
            probabilityOfVisitingEachSpace[0] = 1;
            for (int i=1; i<=numSpaces; i++) {
                probabilityOfVisitingEachSpace[i] = 0;
                foreach(var key in probabilityDistribution.Keys)
                {
                    if((i-key) >= 0)
                    {
                        probabilityOfVisitingEachSpace[i] 
                            += probabilityOfVisitingEachSpace[(i-key)]
                                * probabilityDistribution[key];
                    }
                }
            }
            return probabilityOfVisitingEachSpace;
        }
    }
}
