using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Riddles.Probability.Core;

namespace Riddles.Probability.Application
{
    public class DungeonsAndDragonsSkillCheckProbabilityCalculator
    {
        private CumulativeTrialOutcomeDistributionCalculator
            _cumulativeTrialOutcomeDistributionCalculator;
        private CumulativeFrequencyCalculator
            _cumulativeFrequencyCalculator;

        public DungeonsAndDragonsSkillCheckProbabilityCalculator()
        {
            this._cumulativeTrialOutcomeDistributionCalculator 
                = new CumulativeTrialOutcomeDistributionCalculator();
            this._cumulativeFrequencyCalculator 
                = new CumulativeFrequencyCalculator();
        }

        public Dictionary<int, double> CalculateOddsOfPassingCheckAtVariousTargets(
            int[] dice
        )
        {
            var distributions = dice.Select(
                x => Enumerable.Range(1, x).ToDictionary( y => y, y => 1.0/x )
            ).ToList();
            var outcomeDistribution = 
                this._cumulativeTrialOutcomeDistributionCalculator
                    .CalculateCumulativeOutcomeDistributionForTrials(
                        (x, y) => x + y,
                        distributions,
                        0,
                        1.0
                    );
            return
                this._cumulativeFrequencyCalculator
                    .CalculateCumulativeFrequency(
                        outcomeDistribution,
                        (x, y) => x >= y
                    );
        }

        public double CalculateOddsOfPassingCheckAtTarget(
            int[] dice,
            int target
        )
        {
            var cumulativeFrequency =
                this.CalculateOddsOfPassingCheckAtVariousTargets(dice);
            return cumulativeFrequency[target];
        }
    }
}
