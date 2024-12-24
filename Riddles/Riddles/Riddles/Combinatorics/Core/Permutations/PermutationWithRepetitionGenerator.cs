using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Combinatorics.Core.Permutations
{
	public class PermutationWithRepetitionGenerator
	{
		private class EncodeDecodeInfo
		{
			public EncodeDecodeInfo(
				Dictionary<char, char> encodeMap,
                Dictionary<char, char> decodeMap,
				int numCharacters
            ) {
				this.EncodeMap = encodeMap;
				this.DecodeMap = decodeMap;
				this.NumCharacters = numCharacters;
			}
			public Dictionary<char, char> EncodeMap { get; }
			public Dictionary<char, char> DecodeMap { get; }
			public int NumCharacters { get; }
		}

		private Dictionary<List<char>, EncodeDecodeInfo> _encodeDecodeCache;
		public PermutationWithRepetitionGenerator()
		{
			this._encodeDecodeCache =
				new Dictionary<List<char>, EncodeDecodeInfo>();

        }

		public List<string> GenerateAllOutcomes(int numTrials, List<char> possibleOutcomes, bool isOrdered)
		{
			var firstOutcome = (char)0;
			if (!this._encodeDecodeCache.ContainsKey(possibleOutcomes))
			{
				this._encodeDecodeCache[possibleOutcomes] = this.GenerateEncodeDecodeInfo(possibleOutcomes);
			}
			var outcomes = this.GenerateAllOutcomes(numTrials, possibleOutcomes.Count, firstOutcome, isOrdered);
			return outcomes
				.Select(o => new string(o.ToCharArray().Select(x => this._encodeDecodeCache[possibleOutcomes].DecodeMap[x]).ToArray()))
				.ToList();
		}

		public List<string> GenerateAllOutcomes(
			int numTrials, 
			int numOutcomes, 
			char firstOutcome,
			bool isOrdered
		)
		{
			if (numTrials <= 0 || numOutcomes <= 0)
			{
				return null;
			}

			List<string> outcomes = new List<string>();
			var initialOutcome = new String(firstOutcome, numTrials);
			var lastOutcome = (char)(firstOutcome + (numOutcomes - 1));

			var currentPasscode = initialOutcome;
			outcomes.Add(currentPasscode);
			while (currentPasscode != null)
			{
				currentPasscode = this.GenerateNextOutcome(currentPasscode, firstOutcome, lastOutcome, isOrdered);
				if (currentPasscode != null)
				{
					outcomes.Add(currentPasscode);
				}
			}
			return outcomes;
		}

		public string GenerateNextOutcome(
			string currentOutcome, 
			List<char> possibleOutcomes, 
			int numTrials, 
			bool isOrdered
		)
		{
            if (!this._encodeDecodeCache.ContainsKey(possibleOutcomes))
            {
                this._encodeDecodeCache[possibleOutcomes] = this.GenerateEncodeDecodeInfo(possibleOutcomes);
            }
			if(currentOutcome == null)
			{
				return new string(Enumerable.Range(0, numTrials).Select(i => possibleOutcomes[0]).ToArray());
			}
			var encodedCurrentOutcome = new string(
				currentOutcome.ToCharArray().Select(c => this._encodeDecodeCache[possibleOutcomes].EncodeMap[c]).ToArray()
			);
			var encodedNextOutcome = this.GenerateNextOutcome(encodedCurrentOutcome, (char)0, (char)(0 + possibleOutcomes.Count - 1), isOrdered);
			if(encodedNextOutcome == null)
			{
				return null;
			}
			return new string(
                encodedNextOutcome.ToCharArray().Select(c => this._encodeDecodeCache[possibleOutcomes].DecodeMap[c]).ToArray()
            ); ;
        }

		private string GenerateNextOutcome(string currentOutcome, char firstOutcome, char lastOutcome, bool isOrdered)
		{
			var currentOutcomeAsCharArray = currentOutcome.ToCharArray();

			// this is the last passcode if every character is the last character
			if (currentOutcomeAsCharArray.All(c => c == lastOutcome))
			{
				return null;
			}

			for (int i = currentOutcomeAsCharArray.Length - 1; i >= 0; i--)
			{
				if (currentOutcomeAsCharArray[i] != lastOutcome)
				{
					currentOutcomeAsCharArray[i]++;
					for (int j = i + 1; j < currentOutcomeAsCharArray.Length; j++)
					{
                        // if order doesn't matter skip over any outcome that
                        // has already been generated
                        currentOutcomeAsCharArray[j] = isOrdered ? firstOutcome : currentOutcomeAsCharArray[i];
					}
					break;
				}
			}
			var nextOutcome = currentOutcomeAsCharArray.Aggregate("", (agg, c) => $@"{agg}{c}");
			return nextOutcome;
		}

		public List<int[]> GenerateAllOutcomes(
			int firstOutcome,
			int lastOutcome,
			int numTrials,
			bool doesOrderMatter,
			Func<int[], bool> inclusionFilter = null)
		{
			var results = new List<int[]>();
			var currentOutcome = Enumerable.Range(0, numTrials)
				.Select(i => firstOutcome).ToArray();
			while (currentOutcome != null)
			{
				if (inclusionFilter == null || inclusionFilter(currentOutcome))
				{
                    results.Add(currentOutcome);
                }
				currentOutcome = this.GenerateNextOutcome(
					currentOutcome,
					firstOutcome,
					lastOutcome,
					doesOrderMatter
				);
			}
			return results;
        }

		public int[] GenerateNextOutcome(
			int[] currentOutcome, 
			int firstOutcome, 
			int lastOutcome, 
			bool doesOrderMatter
		)
		{
			if(currentOutcome == null)
			{
				return null;
			}
			var nextOutcome = currentOutcome.ToArray();
			if (nextOutcome.All(c => c == lastOutcome))
			{
				return null;
			}

			for (int i = nextOutcome.Length - 1; i >= 0; i--)
			{
				if (nextOutcome[i] != lastOutcome)
				{
					nextOutcome[i]++;
					for (int j = i + 1; j < nextOutcome.Length; j++)
					{
						// if order doesn't matter skip over any outcome that
						// has already been generated
						nextOutcome[j] = doesOrderMatter ? firstOutcome : nextOutcome[i];
					}
					break;
				}
			}
			return nextOutcome;
		}

        private EncodeDecodeInfo GenerateEncodeDecodeInfo(List<char> possibleOutcomes)
        {
			return new EncodeDecodeInfo(
                encodeMap: possibleOutcomes.Select((v, i) => (v, i)).ToDictionary(x => x.v, x => (char)(0 + x.i)),
                decodeMap: possibleOutcomes.Select((v, i) => (v, i)).ToDictionary(x => (char)(0 + x.i), x => x.v),
                numCharacters: possibleOutcomes.Count
            );
        }
    }
}
