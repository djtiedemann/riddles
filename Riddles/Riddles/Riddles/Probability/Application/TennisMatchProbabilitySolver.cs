using Riddles.MarkovChains;
using Riddles.Probability.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability.Application
{
    public class TennisMatchProbabilitySolver
    {
        private CumulativeTrialOutcomeDistributionCalculator 
            _outcomeDistributionCalculator;
        private TennisGameSolver
            _tennisGameSolver;

        public TennisMatchProbabilitySolver()
        {
            this._outcomeDistributionCalculator =
                new CumulativeTrialOutcomeDistributionCalculator();
            this._tennisGameSolver = new TennisGameSolver();
        }

        public double CalculateProbabilityOfWinningMatchAndLosingMostGames()
        {
            var gameProbabilityDistribution = this._tennisGameSolver
                .CalculateOddsOfWinningStandardGame()
                .GroupBy(x => (x.Key.Item1 - x.Key.Item2))
                .Select(x => (x.Key,  x.ToList().Sum(x => x.Value)))
                .ToDictionary(x => x.Item1, x => x.Item2);
            var tiebreakGameProbabilityDistribution = this._tennisGameSolver
                .CalculateOddsOfWinningTiebreakGame()
                .GroupBy(x => (x.Key.Item1 - x.Key.Item2))
                .Select(x => (x.Key, x.ToList().Sum(x => x.Value)))
                .ToDictionary(x => x.Item1, x => x.Item2);
            var setProbabilityDistribution = this._tennisGameSolver
                .CalculateOddsOfWinningSet();
            var matchProbabilityDistribution = this._tennisGameSolver
                .CalculateOddsOfWinningMatch();

            var setOutcomeProbabilities =
                this.CalculateOddsOfWinningSetByNPoints(
                    setProbabilityDistribution,
                    gameProbabilityDistribution,
                    tiebreakGameProbabilityDistribution
                );
            var matchOutcomeProbabilites =
                this.CalculateOddsOfWinningMatchByNPoints(
                    matchProbabilityDistribution,
                    setOutcomeProbabilities
                );
            return matchOutcomeProbabilites.Keys.Where(k => k < 0)
                .Sum(k => matchOutcomeProbabilites[k]);
        }

        public Dictionary<int, double> CalculateOddsOfWinningMatchByNPoints(
            Dictionary<(int, int), double> matchOutcomeProbabilities,
            Dictionary<int, double> setProbability
        )
        {
            var setProbabilityLoss = setProbability.ToDictionary(
                t => t.Key * -1,
                t => t.Value
            );

            var matchOutcomeDict = new
                Dictionary<int, double>();
            foreach (var outcome in matchOutcomeProbabilities.Keys)
            {
                var distributions = new List<Dictionary<int, double>>();
                for (int i = 0; i < outcome.Item1; i++)
                {
                    distributions.Add(setProbability);
                }
                for (int i = 0; i < outcome.Item2; i++)
                {
                    distributions.Add(setProbabilityLoss);
                }
                var resultDistribution = this._outcomeDistributionCalculator
                    .CalculateCumulativeOutcomeDistributionForTrials(
                        (x, y) => x + y,
                        distributions,
                        0,
                        matchOutcomeProbabilities[outcome]
                    );
                foreach (var result in resultDistribution)
                {
                    if (!matchOutcomeDict.ContainsKey(result.Key))
                    {
                        matchOutcomeDict[result.Key] = 0;
                    }
                    matchOutcomeDict[result.Key] += result.Value;
                }
            }
            return matchOutcomeDict;
        }

        public Dictionary<int, double> CalculateOddsOfWinningSetByNPoints(
                Dictionary<(int, int), double> setOutcomeProbabilities,
                Dictionary<int, double> gameProbability,
                Dictionary<int, double> tiebreakGameProbability
            )
        {
            var gameProbabilityLoss = gameProbability.ToDictionary(
                t => t.Key * -1,
                t => t.Value
            );

            var setOutcomeDict = new
                Dictionary<int, double>();            
            foreach (var outcome in setOutcomeProbabilities.Keys)
            {
                var distributions = new List<Dictionary<int, double>>();
                for (int i=0; i<outcome.Item1; i++)
                {
                    if(i == 6 && outcome.Item2 == 6)
                    {
                        distributions.Add(tiebreakGameProbability);
                    }
                    else
                    {
                        distributions.Add(gameProbability);
                    }
                }
                for(int i=0; i<outcome.Item2; i++)
                {
                    distributions.Add(gameProbabilityLoss);
                }
                var resultDistribution = this._outcomeDistributionCalculator
                    .CalculateCumulativeOutcomeDistributionForTrials(
                        (x, y) => x + y,
                        distributions,
                        0,
                        setOutcomeProbabilities[outcome]
                    );
                foreach(var result in resultDistribution)
                {
                    if (!setOutcomeDict.ContainsKey(result.Key))
                    {
                        setOutcomeDict[result.Key] = 0;
                    }
                    setOutcomeDict[result.Key]+=result.Value;
                }
            }
            return setOutcomeDict;
        }
    }
}
