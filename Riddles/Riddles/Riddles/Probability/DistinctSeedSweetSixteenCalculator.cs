﻿using Riddles.Combinatorics.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Riddles.Tournaments;

namespace Riddles.Probability
{
    // https://fivethirtyeight.com/features/the-riddler-meets-jeopardy/
    // Calculate the odds that the sweet 16 is comprised of 16 distinct seeds.
    // Use 2 probability models:
    // The first assumes each team is equally likely to win
    // The second weights the likelihood of winning by the odds of the seed winning.
    public class DistinctSeedSweetSixteenCalculator
    {
        private FactorialCalculator _factorialCalculator;
        private SingleEliminationNoReseedBrackerSurvivalOddsCalculator _singleEliminationNoReseedBrackerSurvivalOddsCalculator;

        public DistinctSeedSweetSixteenCalculator()
        {
            this._factorialCalculator = new FactorialCalculator();
            this._singleEliminationNoReseedBrackerSurvivalOddsCalculator = new SingleEliminationNoReseedBrackerSurvivalOddsCalculator();
        }

        public double CalculateOddsOfDistinctSeedsSurvivingBracket(
            List<int[]> brackets,
            SingleEliminationNoReseedBrackerSurvivalOddsCalculator.WinningLikelihoodModel likelihoodModel)
        {
            var totalProbabilityOfAllDistinctSeeds = 1.0;
            foreach(var seeds in brackets)
            {
                var oddsOfEachTeamAdvancing = this._singleEliminationNoReseedBrackerSurvivalOddsCalculator.CalculateOddsOfTeamEachAdvancing(seeds.ToList(), likelihoodModel);
                var probabilityOfUniqueSeedsSurvivingThisBracket
                    = oddsOfEachTeamAdvancing.Aggregate(1.0, (agg, v) => agg * v.Value) * this._factorialCalculator.Factorial(seeds.Length);
                totalProbabilityOfAllDistinctSeeds *= probabilityOfUniqueSeedsSurvivingThisBracket;
            }
            return totalProbabilityOfAllDistinctSeeds;
        }

        
    }
}
