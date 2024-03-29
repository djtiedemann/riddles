﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riddles.Combinatorics;
using Riddles.Combinatorics.Core.Permutations;
using Riddles.Probability.Core;

namespace Riddles.Optimization
{
    // Answer: https://fivethirtyeight.com/features/can-you-take-down-all-the-bottles-of-beer/
    public class BlindLetterChallengeOptimizer
    {
        private PermutationWithoutRepetitionGenerator _permutationGenerator;
        private ProbabilityUtil _probabilityUtil;
        private Dictionary<(int, int), double> _cache;
        public BlindLetterChallengeOptimizer() { 
            this._permutationGenerator = new PermutationWithoutRepetitionGenerator();
            this._probabilityUtil = new ProbabilityUtil();
            this._cache = new Dictionary<(int, int), double>();
        }

        public double GetPercentSuccessfulAttempts(int numLetters, int numSlots, Strategy strategy)
        {
            switch (strategy)
            {
                case Strategy.Optimal:
                    return this.GetPercentSuccessfulAttemptsOptimalStrategy(numLetters, numSlots);
                case Strategy.EvenSpacing:
                    return this.GetPercentSuccessfulAttemptsEvenSpacing(numLetters, numSlots);
            }
            return 0.0;
        }

        private double GetPercentSuccessfulAttemptsOptimalStrategy(int numLetters, int numSlots)
        {
            if (this._cache.ContainsKey((numLetters, numSlots)))
            {
                return this._cache[(numLetters, numSlots)];
            }
            if(numSlots == 0)
            {
                this._cache[(numLetters, numSlots)] = 1;
            }
            else if (numLetters == 0)
            {
                this._cache[(numLetters, numSlots)] = 0;
            }
            else if(numLetters == numSlots || numSlots <= 1)
            {
                this._cache[(numLetters, numSlots)] = 1;
            }
            else
            {
                var runningTotal = 0.0;
                for(int i=0; i<numLetters; i++)
                {
                    var highestProbability = 0.0;
                    for(int j=0; j<numSlots; j++)
                    {
                        var setInfo = new List<BaseSetInfo>
                        {
                            new BaseSetInfo(i, j),
                            new BaseSetInfo(numLetters - 1 - i, numSlots - 1 - j)
                        };
                        var probability = this._probabilityUtil
                            .CalculateOddsOfDrawingIndistinctObjectsWithoutReplacement(setInfo)
                            * this.GetPercentSuccessfulAttemptsOptimalStrategy(i, j)
                            * this.GetPercentSuccessfulAttemptsOptimalStrategy(numLetters - 1 - i, numSlots - 1 - j);
                        if(probability > highestProbability)
                        {
                            highestProbability = probability;
                        }
                    }
                    runningTotal+= (highestProbability / numLetters);
                }
                this._cache[(numLetters, numSlots)] = runningTotal; 
            }
            return this._cache[(numLetters, numSlots)];
        }

        private double GetPercentSuccessfulAttemptsEvenSpacing(int numLetters, int numSlots)
        {
            var permutationState = this._permutationGenerator.GenerateNextPermutation(
                new PermutationWithoutRepetitionGenerator.PermutationState(
                    null, 
                    null, 
                    Enumerable.Range(1, numLetters).ToArray(), 
                    numSlots
                ));
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
                var minInfo = result.Select((x, i) => new { Value = x + 1, Index = i + 1})
                    .Where(x => x.Value <= letter && x.Value != 0)
                    .OrderByDescending(x => x.Value).FirstOrDefault() ?? 
                        new { Value = 1, Index = 0};
                var maxInfo = result.Select((x, i) => new { Value = x - 1, Index = i - 1 })
                    .Where(x => x.Value >= letter && x.Value != -2)
                    .OrderBy(x => x.Value).FirstOrDefault() ?? 
                    new { Value = numLetters, Index = attempt.Length - 1 };
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

        public enum Strategy
        {
            Optimal = 0,
            EvenSpacing = 1,
        }
    }
}
