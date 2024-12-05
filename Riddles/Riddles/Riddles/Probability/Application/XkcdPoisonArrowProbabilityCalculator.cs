using Riddles.Combinatorics.Core;
using Riddles.Combinatorics.Core.Permutations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability.Application
{
    /// <summary>
    /// https://thefiddler.substack.com/p/did-xkcd-get-its-math-right
    /// https://xkcd.com/3015/
    /// 
    /// The scenario is: draw 2 arrows at random, avoid the 5 poisoned
    /// arrows out of 10. The question is what skill check in D&D gets
    /// those odds. 3 d6s and a d4 with a check of 16 works. What else?
    /// </summary>
    public class XkcdPoisonArrowProbabilityCalculator
    {
        private double _epsilon = 0.00000000001;

        public BinomialTheoremCalculator _binomialTheoremCalculator;
        public PermutationWithRepetitionGenerator _permutationGenerator;
        public DungeonsAndDragonsSkillCheckProbabilityCalculator _skillCheckProbabilityGenerator;

        public XkcdPoisonArrowProbabilityCalculator() { 
            this._binomialTheoremCalculator = new BinomialTheoremCalculator();
            this._permutationGenerator = new PermutationWithRepetitionGenerator();
            this._skillCheckProbabilityGenerator = new DungeonsAndDragonsSkillCheckProbabilityCalculator();
        }

        public List<(int[], int)> FindSkillChecksThatSumToSameOddsAsAvoidingPoisonArrows(
            int[] diceTypes,
            int maxNumDiceRolled,
            int numArrows,
            int numPoisonedArrows,
            int numDraws
        )
        {
            var targetOdds = this.CalculateOddsOfDrawingAllNonPoisonedArrows(
                numArrows,
                numPoisonedArrows,
                numDraws
            );
            var validChecks = new List<(int[], int)>();
            for(int numDiceRolled=1; numDiceRolled<=maxNumDiceRolled; numDiceRolled++)
            {
                var firstOutcome = 0;
                var lastOutcome = diceTypes.Length - 1;
                var currentOutcome = Enumerable.Range(0, numDiceRolled)
                    .Select(i => firstOutcome).ToArray();
                int count = 0;
                while(currentOutcome != null)
                {
                    count++;
                    var dice = currentOutcome.Select(i => diceTypes[i]).ToArray();

                    var skillCheckDistribution = this._skillCheckProbabilityGenerator
                        .CalculateOddsOfPassingCheckAtVariousTargets(dice);
                    foreach (var skillCheck in skillCheckDistribution.Keys)
                    {
                        var successProbability = skillCheckDistribution[skillCheck];
                        if (Math.Abs(successProbability - targetOdds)
                            <= this._epsilon)
                        {
                            validChecks.Add((dice, skillCheck));
                        }
                    }
                    currentOutcome = this._permutationGenerator
                        .GenerateNextOutcome(currentOutcome, firstOutcome, lastOutcome, isOrdered: false);
                }
            }
            return validChecks;
        }

        public double CalculateOddsOfDrawingAllNonPoisonedArrows(
            int numArrows,
            int numPoisonedArrows,
            int numDraws)
        {
            return this._binomialTheoremCalculator
                .CalculateBinomialCoefficient(
                    numArrows - numPoisonedArrows,
                    numDraws
                ) / this._binomialTheoremCalculator
                .CalculateBinomialCoefficient(
                    numArrows,
                    numDraws
                );
        }
    }
}
