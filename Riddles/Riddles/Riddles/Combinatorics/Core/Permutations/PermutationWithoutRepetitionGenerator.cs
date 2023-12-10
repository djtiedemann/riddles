using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Riddles.Combinatorics.Core.Domain;
using Riddles.Combinatorics.Core;
using static Riddles.Combinatorics.Core.SetGeneration.PermutationWithoutRepetitionGenerator;

namespace Riddles.Combinatorics.Core.SetGeneration
{
    public class PermutationWithoutRepetitionGenerator
    {
        private class EncodeDecodeInfo
        {
            public EncodeDecodeInfo(
                Dictionary<char, int> encodeMap,
                Dictionary<int, char> decodeMap,
                int numCharacters
            )
            {
                this.EncodeMap = encodeMap;
                this.DecodeMap = decodeMap;
                this.NumCharacters = numCharacters;
            }
            public Dictionary<char, int> EncodeMap { get; }
            public Dictionary<int, char> DecodeMap { get; }
            public int NumCharacters { get; }
        }

        private FactorialCalculator _factorialCalculator;
        private Random _random;
        private Dictionary<string, EncodeDecodeInfo> _encodeDecodeCache;
        public PermutationWithoutRepetitionGenerator()
        {
            this._factorialCalculator = new FactorialCalculator();
            this._random = new Random();
            this._encodeDecodeCache =
                new Dictionary<string, EncodeDecodeInfo>();
        }

        public List<Permutation> GenerateAllPermutations(int n, int r)
        {
            var permutations = new List<Permutation>();
            if (r > n || r <= 0 || n <= 0)
            {
                return permutations;
            }

            bool hasNextPermutation = true;
            var state = new PermutationState(null, null, Enumerable.Range(1, n).ToArray(), r);
            while (hasNextPermutation)
            {
                state = this.GenerateNextPermutation(state);
                hasNextPermutation = state.CurrentPermutation != null;
                if (hasNextPermutation)
                {
                    permutations.Add(state.CurrentPermutation);
                }
            }
            return permutations;
        }

        // this is essentially an anagram generator which is similar to permutations
        public List<string> GenerateAllPermutationsOfString(string s) 
        {
            var encodeDecodeInfo = this.GetEncodeDecodeInfo(s);
            var allPermutations = new List<string>();
            var currentPermutation = new string(s.ToCharArray().OrderBy(x => x).ToArray());
            while (currentPermutation != null) { 
                allPermutations.Add(currentPermutation);
                currentPermutation = this.GenerateNextPermutation(currentPermutation, s);
            }
            return allPermutations;
        }

        public string GenerateNextPermutation(
            string currentPermutation, 
            string initialString
        )
        {
            var encodeDecodeInfo = this.GetEncodeDecodeInfo(initialString);
            if(currentPermutation == null)
            {
                return new string(initialString.ToCharArray().OrderBy(x => x).ToArray());
            }
            var encodedPermutation = 
                currentPermutation.ToCharArray()
                .Select(c => encodeDecodeInfo.EncodeMap[c]).ToArray();
            var nextPermutation = this.RotatePermutation(encodedPermutation);
            if(nextPermutation == null)
            {
                return null;
            }
            var decodedPermutation = new string(nextPermutation
                .Select(c => encodeDecodeInfo.DecodeMap[c]).ToArray());
            return decodedPermutation;
        }

        public PermutationState GenerateNextPermutation(PermutationState permutationState)
        {
            if (permutationState.IndicatorPermutation == null)
            {
                var n = permutationState.CharacterSet.Length;
                var indicatorPermutation = new Permutation(Enumerable.Range(1, n - permutationState.PermutationSize).Select(r => 0)
                .Concat(Enumerable.Range(1, permutationState.PermutationSize).Select(r => 1)).ToArray());
                var indicatorCharSet =
                    permutationState.CharacterSet.Where((x, i) => indicatorPermutation.GetPermutation()[i] == 1)
                    .OrderBy(x => x).ToArray();
                var nextPermutation = new Permutation(indicatorCharSet);
                return new PermutationState(nextPermutation, indicatorPermutation, permutationState.CharacterSet, permutationState.PermutationSize);
            }
            var innerPermutation = permutationState.CurrentPermutation.GetPermutation();
            var newPermutation = this.RotatePermutation(innerPermutation);
            if(newPermutation != null)
            {
                return new PermutationState(new Permutation(newPermutation), permutationState.IndicatorPermutation, permutationState.CharacterSet, permutationState.PermutationSize);
            }
            // attempt to rotate Indicator Permutation, if successful, generate the next value with the new indicator permutation
            if (permutationState.IndicatorPermutation.GetPermutation().Any(x => x != 1))
            {
                var indicatorPermutationState = new PermutationState(
                    permutationState.IndicatorPermutation,
                    new Permutation(permutationState.IndicatorPermutation.GetPermutation().Select(i => 1).ToArray()),
                    new int[] { 0, 1 },
                    permutationState.IndicatorPermutation.GetPermutation().Length
                );
                var newIndicatorPermutation = this.GenerateNextPermutation(indicatorPermutationState).CurrentPermutation;
                if (newIndicatorPermutation != null)
                {
                    var indicatorCharSet =
                    permutationState.CharacterSet.Where((x, i) => newIndicatorPermutation.GetPermutation()[i] == 1)
                    .OrderBy(x => x).ToArray();
                    var nextPermutation = new Permutation(indicatorCharSet);
                    return
                        new PermutationState(
                            nextPermutation,
                            newIndicatorPermutation,
                            permutationState.CharacterSet,
                            permutationState.PermutationSize
                        );
                }
            }
            return new PermutationState(null, null, permutationState.CharacterSet, permutationState.PermutationSize);
        }

        private int[] RotatePermutation(int[] oldPermutation)
        {
            var newPermutation = new int[oldPermutation.Length];
            for (int i = 0; i < oldPermutation.Length; i++)
            {
                newPermutation[i] = oldPermutation[i];
            }

            for (int i = newPermutation.Length - 1; i > 0; i--)
            {
                // the first time you find a digit that's less than the next digit, find the smallest digit to the right of the digit being swapped
                // that's larger than the digit being swapped. swap those two digits, then sort the remaining digits to the right of the digit swapped
                // for instance: 12534 becomes 12453, 21543 becomes 23145
                if (newPermutation[i] > newPermutation[i - 1])
                {
                    var minDigitToSwap = newPermutation[i];
                    var indexToSwap = i;
                    for (var j = i + 1; j < newPermutation.Length; j++)
                    {
                        if (newPermutation[j] < minDigitToSwap && newPermutation[j] > newPermutation[i - 1])
                        {
                            minDigitToSwap = newPermutation[j];
                            indexToSwap = j;
                        }
                    }

                    var temp = newPermutation[indexToSwap];
                    newPermutation[indexToSwap] = newPermutation[i - 1];
                    newPermutation[i - 1] = temp;

                    var digitsToSort = new int[newPermutation.Length - i];
                    for (var j = i; j < newPermutation.Length; j++)
                    {
                        digitsToSort[j - i] = newPermutation[j];
                    }
                    var sortedDigits = digitsToSort.OrderBy(i => i).ToArray();

                    for (var j = i; j < newPermutation.Length; j++)
                    {
                        newPermutation[j] = sortedDigits[j - i];
                    }
                    return newPermutation;
                }
            }
            return null;
        }
        public Permutation GenerateRandomPermutation(int n)
        {
            var nFactorial = this._factorialCalculator.Factorial(n);
            var randKey = this._random.Next(0, (int)nFactorial);
			return this.GeneratePermutationFromKey(randKey, n);
		}

		public Permutation GeneratePermutationFromKey(int key, int n)
		{
			var permutation = Enumerable.Range(1, n).ToArray();
			for(int i=n-1; i>0; i--) {
				var iFactorial = this._factorialCalculator.Factorial(i);
				var indexToSwap = key / (int)iFactorial;
				var temp = permutation[indexToSwap];
				permutation[indexToSwap] = permutation[i];
				permutation[i] = temp;
				key = key % (int)iFactorial;
			}
			return new Permutation(permutation);
		}

        private EncodeDecodeInfo GetEncodeDecodeInfo(string permutationString)
        {
            if (!this._encodeDecodeCache.ContainsKey(permutationString))
            {
                var orderedCharacters = permutationString.ToCharArray().Distinct().OrderBy(x => x);
                this._encodeDecodeCache[permutationString] = new EncodeDecodeInfo(
                    encodeMap: orderedCharacters.Select((v, i) => (v, i)).ToDictionary(x => x.v, x => (0 + x.i)),
                    decodeMap: orderedCharacters.Select((v, i) => (v, i)).ToDictionary(x => (0 + x.i), x => x.v),
                    numCharacters: orderedCharacters.Count()
                );
            }
            return this._encodeDecodeCache[permutationString];
        }

        public class PermutationState
        {
            public PermutationState(Permutation currentPermutation, Permutation indicatorPermutation, int[] characterSet, int permutationSize)
            {
                CurrentPermutation = currentPermutation;
                IndicatorPermutation = indicatorPermutation;
                CharacterSet = characterSet;
                PermutationSize = permutationSize;
            }

            public Permutation CurrentPermutation { get; }
            /* 
             * This is used to generate permutations that contain some, but not all of the values in the initial set (r < n) in the n P r formula
             * The indicator permutation is used to indicate which elements of the set are included in this round of permutations. 
             * We loop through all possible indicator permutations, as well as all possible permutations of those elements
             */
            public Permutation IndicatorPermutation { get; }
            public int[] CharacterSet { get; }
            public int PermutationSize { get; }
        }
    }
}
