using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riddles.Combinatorics.Core.SetGeneration;

namespace Riddles.Optimization
{
    public enum Strategy
    {
        Optimal = 0,
        EvenSpacing = 1,
    }
    public class BlindLetterChallengeOptimizer
    {
        private PermutationWithoutRepetitionGenerator _permutationGenerator;
        public BlindLetterChallengeOptimizer() { 
            this._permutationGenerator = new PermutationWithoutRepetitionGenerator();
        }

        public double GetPercentSuccessfulAttempts(int numLetters, int numSlots, Strategy strategy)
        {
            switch (strategy)
            {
                case Strategy.Optimal:
                    return 0.0;
                case Strategy.EvenSpacing:
                    return this.GetPercentSuccessfulAttemptsEvenSpacing(numLetters, numSlots);
            }
            return 0.0;
        }

        private double GetPercentSuccessfulAttemptsEvenSpacing(int numLetters, int numSlots)
        {
            var permutationState = this._permutationGenerator.GenerateNextPermutation(new PermutationWithoutRepetitionGenerator.PermutationState(null, null, Enumerable.Range(1, numLetters).ToArray(), numSlots));
            var numPermutations = 0;
            var numSuccessfulPermutations = 0;
            while(permutationState.CurrentPermutation != null)
            {
                if(this.IsSuccessfulAttemptEvenSpacingStrategy(permutationState.CurrentPermutation.GetPermutation(), numLetters)){
                    numSuccessfulPermutations++;
                }
                numPermutations++;
                permutationState = this._permutationGenerator.GenerateNextPermutation(permutationState);
            }
            return (double)numSuccessfulPermutations/(double)numPermutations;
        }

        public bool IsSuccessfulAttemptEvenSpacingStrategy(int[] attempt, int numLetters)
        {
            int[] result = Enumerable.Range(0, attempt.Length).Select(x => -1).ToArray();
            foreach(int letter in attempt)
            {
                var minInfo = result.Select((x, i) => new { Value = x + 1, Index = i + 1}).Where(x => x.Value <= letter && x.Value != 0).OrderByDescending(x => x.Value).FirstOrDefault() ?? new { Value = 1, Index = 0};
                var maxInfo = result.Select((x, i) => new { Value = x - 1, Index = i - 1 }).Where(x => x.Value >= letter && x.Value != -2).OrderBy(x => x.Value).FirstOrDefault() ?? new { Value = numLetters, Index = attempt.Length - 1 };
                if(minInfo.Index >= attempt.Length || maxInfo.Index < 0 || minInfo.Index > maxInfo.Index)
                {
                    return false;
                }
                var numSlots = (maxInfo.Index - minInfo.Index) + 1;
                var numPossibleCharacters = (maxInfo.Value - minInfo.Value) + 1;
                var characterIndex = letter - minInfo.Value;
                var percentage = (double)characterIndex / (numPossibleCharacters);
                var index = (int)(percentage * numSlots) + minInfo.Index;
                result[index] = letter;
            }
            return true;
        }
    }
}
