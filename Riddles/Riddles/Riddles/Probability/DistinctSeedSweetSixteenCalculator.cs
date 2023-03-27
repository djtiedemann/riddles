using Riddles.Combinatorics.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Riddles.Probability
{
    public class DistinctSeedSweetSixteenCalculator
    {
        public enum WinningLikelihoodModel
        {
            EqualOdds = 1,
            WeightedOdds = 2
        }
        private FactorialCalculator _factorialCalculator;

        public DistinctSeedSweetSixteenCalculator()
        {
            this._factorialCalculator = new FactorialCalculator();
        }

        public double CalculateOddsOfDistinctSeedsSurvivingBracket(
            List<int[]> brackets, 
            WinningLikelihoodModel likelihoodModel)
        {
            var totalProbabilityOfAllDistinctSeeds = 1.0;
            foreach(var seeds in brackets)
            {
                var oddsOfEachTeamAdvancing = this.CalculateOddsOfTeamEachAdvancing(seeds, likelihoodModel);
                var probabilityOfUniqueSeedsSurvivingThisBracket
                    = oddsOfEachTeamAdvancing.Aggregate(1.0, (agg, v) => agg * v) * this._factorialCalculator.Factorial(seeds.Length);
                totalProbabilityOfAllDistinctSeeds *= probabilityOfUniqueSeedsSurvivingThisBracket;
            }
            return totalProbabilityOfAllDistinctSeeds;
        }

        public double[] CalculateOddsOfTeamEachAdvancing(
           int[] seeds,
           WinningLikelihoodModel likelihoodModel)
        {
            if(seeds.Length != 4)
            {
                throw new ArgumentException("Currently doesn't support a bracket of >4 teams");
            }
            var headToHeadProbabilityMatrix = this.CalculateHeadToHeadProbabilityMatrix(seeds, likelihoodModel);
            var oddsToAdvance = new double[seeds.Length];
            for(int i=0; i<seeds.Length; i++)
            {
                var firstOpponent = i % 2 == 1 ? i - 1 : i + 1;
                var potentialSecondGameOpponentOne = i >= 2 ? i - 2 : i + 2;
                var potentialSecondGameOpponentTwo = i >= 2 ? firstOpponent - 2 : firstOpponent + 2;
                oddsToAdvance[i] = 
                    // odds of winning first game
                    headToHeadProbabilityMatrix[i, firstOpponent]
                    // odds potential opponent 1 advances and team beats that opponent
                    * (headToHeadProbabilityMatrix[potentialSecondGameOpponentOne, potentialSecondGameOpponentTwo] * headToHeadProbabilityMatrix[i, potentialSecondGameOpponentOne]
                    + headToHeadProbabilityMatrix[potentialSecondGameOpponentTwo, potentialSecondGameOpponentOne] * headToHeadProbabilityMatrix[i, potentialSecondGameOpponentTwo]);
            }
            return oddsToAdvance;
        }

        /// <summary>
        /// Calculates a matrix representing with [i, j] 
        /// representing the odds team i would beat team j
        /// </summary>
        /// <param name="seeds"></param>
        /// <param name="winningLikelihoodModel"></param>
        /// <returns></returns>
        private double[,] CalculateHeadToHeadProbabilityMatrix(
            int[] seeds,
            WinningLikelihoodModel winningLikelihoodModel)
        {
            var calculationMatrix = new double[seeds.Length, seeds.Length];
            for (int i = 0; i < seeds.Length; i++)
            {
                for (int j = 0; j < seeds.Length; j++)
                {
                    switch (winningLikelihoodModel)
                    {
                        case WinningLikelihoodModel.EqualOdds:
                            calculationMatrix[i, j] = 0.5;
                            break;
                        case WinningLikelihoodModel.WeightedOdds:
                            calculationMatrix[i, j] = 0.5
                            + 0.033 * (seeds[j] - seeds[i]);
                            break;
                    }
                }
            }
            return calculationMatrix;
        }
    }
}
