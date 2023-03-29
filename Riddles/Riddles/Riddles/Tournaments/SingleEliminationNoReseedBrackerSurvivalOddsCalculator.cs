using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Tournaments
{
    public class SingleEliminationNoReseedBrackerSurvivalOddsCalculator
    {
        public enum WinningLikelihoodModel
        {
            EqualOdds = 1,
            WeightedOdds = 2
        }

        private SingleEliminationNoReseedBracketCreator _singleEliminationNoReseedBracketCreator;
        public SingleEliminationNoReseedBrackerSurvivalOddsCalculator()
        {
            this._singleEliminationNoReseedBracketCreator =
                new SingleEliminationNoReseedBracketCreator();
        }

        public Dictionary<int, double> CalculateOddsOfTeamEachAdvancing(
            List<int> seeds,
            WinningLikelihoodModel likelihoodModel
        )
        {
            var bracket = this._singleEliminationNoReseedBracketCreator
                .CreateBracket(seeds);
            var oddsOfAdvancingLastRound = bracket.Select(i => 1.0).ToArray();
            var oddsOfAdvancingThisRound = new double[bracket.Length];
            var numRounds = Math.Log2(bracket.Length);
            for (int round = 1; round <= numRounds; round++)
            { 
                for (int seed = 0; seed < bracket.Length; seed++)
                {
                    var range = this.GenerateCompetitionRangeInclusive(seed, round);
                    var cumulativeOddsOfWinningRound = 0.0;
                    for(int competitor=range.Item1; competitor<=range.Item2; competitor++)
                    {
                        cumulativeOddsOfWinningRound += this.HeadToHeadProbabilityFunction(bracket[seed], bracket[competitor], likelihoodModel) * oddsOfAdvancingLastRound[competitor];
                    }
                    oddsOfAdvancingThisRound[seed] = cumulativeOddsOfWinningRound * oddsOfAdvancingLastRound[seed];
                }
                oddsOfAdvancingLastRound = oddsOfAdvancingThisRound.ToArray();
            }
            var result =
                bracket.Select((x, i) => (x, ((x != null) ? (double?)oddsOfAdvancingLastRound[i] : (double?)null)))
                .Where(i => i.x != null && i.Item2 != null)
                .ToDictionary(i => (int)i.x, i => (double)i.Item2);
            return result;
        }

        /*
         * Generates the range of competitors for seed for this round of the bracket
         * The idea is that you can calculate the competition that should be in range
         * for this round of the bracket and subtract the competitors that you already
         * played (the ones that were in range for the last round of the bracket).
         * 
         * Then you can use the probability that the team would make it to this part
         * of the bracket and the probability that this team would beat that team
         */
        private (int, int) GenerateCompetitionRangeInclusive(int seed, int round)
        {
            if(round == 0)
            {
                return (seed, seed);
            }
            var rangeSize = 1 << round;
            var min = (seed / rangeSize) * rangeSize;
            var max = min + (rangeSize - 1);
            var previousRangeSize = 1 << (round - 1);
            var prevMin = round > 1 ? (seed / previousRangeSize) * previousRangeSize : seed;
            var prevMax = prevMin + (previousRangeSize - 1);
            var actualMin = (min == prevMin) ? prevMax + 1 : min;
            var actualMax = (max == prevMax) ? prevMin - 1 : max;
            return (actualMin, actualMax);
        }

        private double HeadToHeadProbabilityFunction(
            int? seed1,
            int? seed2,
            WinningLikelihoodModel likelihoodModel)
        {
            // null means the team has a bye.
            // if two teams that are null face each other in a hypothetical bracket
            // the matchup is inconsequential because they will both already be 
            // eliminated
            if(seed2 == null)
            {
                return 1.0;
            }
            if(seed1 == null)
            {
                return 0.0;
            }
            switch (likelihoodModel)
            {
                case WinningLikelihoodModel.EqualOdds:
                    return 0.5;
                case WinningLikelihoodModel.WeightedOdds:
                    return 0.5 + 0.033 * (seed2.Value - seed1.Value);
            }
            throw new NotImplementedException("Unsupported likelihood model");
        }
    }
}