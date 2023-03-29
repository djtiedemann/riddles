using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Riddles.NumberTheory.Core;

namespace Riddles.Tournaments
{
    public class SingleEliminationNoReseedBracketCreator
    {
        private PowerOfTwoCalculator _powerOfTwoCalculator;
        public SingleEliminationNoReseedBracketCreator() {
            this._powerOfTwoCalculator = new PowerOfTwoCalculator();
        }
        // Return a march madness style bracket where the best teams play the worst teams
        // Include the possibility that there isn't a power of two number of seeds.
        public int?[] CreateBracket(List<int> seeds)
        {
            // if there are 2 or fewer teams, the bracket is trivial
            if(seeds.Count() <= 2)
            {
                return seeds.Select(s => (int?)s).OrderBy(s => s).ToArray();
            }
            var sortedSeeds = seeds.OrderBy(s => s).ToList();
            // used to recover the initial order if the seeds have gaps in them
            var indexToOriginalSeedMap = sortedSeeds
                .Select((s, i) => (s, i))
                .ToDictionary(s => s.i, s => s.s);

            var resultingBracketSize = (int)this._powerOfTwoCalculator
                .FindNextPowerOfTwoInclusive(sortedSeeds.Count());

            var bracket = new int[resultingBracketSize];
            var rankedSeeds = Enumerable.Range(0, resultingBracketSize).ToList();

            this.PopulateBracket(rankedSeeds, bracket, 0);
            return bracket.Select(i =>
                indexToOriginalSeedMap.ContainsKey(i)
                    ? indexToOriginalSeedMap[i]
                    : (int?)null
            ).ToArray();
        }

        private void PopulateBracket(List<int> seeds, int[] bracket, int startIndex)
        {
            if(seeds.Count() == 2)
            {
                bracket[startIndex] = seeds[0];
                bracket[startIndex+ 1] = seeds[1];
                return;
            }
            var list1 = new List<int>();
            var list2 = new List<int>();
            list1.Add(seeds[0]);
            var addToListOne = false;
            for(int i=1; i<seeds.Count() - 1; i+=2)
            {
                if (addToListOne)
                {
                    list1.Add(seeds[i]);
                    list1.Add(seeds[i + 1]);
                }
                else
                {
                    list2.Add(seeds[i]);
                    list2.Add(seeds[i + 1]);
                }
                addToListOne = !addToListOne;
            }
            list1.Add(seeds[seeds.Count() - 1]);
            this.PopulateBracket(list1, bracket, startIndex);
            this.PopulateBracket(list2, bracket, startIndex + (seeds.Count() / 2));
        }
    }
}
