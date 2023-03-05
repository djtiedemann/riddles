using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Combinatorics.Application
{
    public class ListInterleaver
    {
        public Dictionary<(int, int), int> _cache;
        public ListInterleaver()
        {
            this._cache = new Dictionary<(int, int), int>();
        }
        /// <summary>
        /// Used to count the number of ways to interleave lists of specific sizes.
        /// https://docs.google.com/document/d/1pnmNARUuVb0AGc95-xyJDHboSB_MsS41U9Sl7w0uFvk/edit
        /// </summary>
        /// <param name="listCounts"></param>
        /// <returns></returns>
        public int CountNumberOfWaysToInterleaveLists(List<int> listCounts)
        {
            if(listCounts.Count() == 0 || !listCounts.All(x => x >= 0))
            {
                return 0;
            }
            var numElementsInList = 0;
            var numWaysToOrderList = 1;
            // when finding the ways to interleave multiple lists, can do it iteratively
            // first, find the number of ways to interleave the first two elements in the list.
            // then consider the result one list, and interleave it with the next list
            // then multiply the number of ways to combine the lists together.
            foreach(var count in listCounts)
            {
                numWaysToOrderList *= 
                    this.CountNumberOfWaysToInterleaveTwoLists(count, numElementsInList);
                numElementsInList += count;
            }
            return numWaysToOrderList;
        }

        private int CountNumberOfWaysToInterleaveTwoLists(int list1Count, int list2Count)
        {
            var min = Math.Min(list1Count, list2Count);
            var max = Math.Max(list1Count, list2Count);

            if(!this._cache.ContainsKey((min, max)))
            {
                if(min == 0)
                {
                    this._cache[(min, max)] = 1;
                }
                else if(min == 1)
                {
                    this._cache[(min, max)] = max + 1;
                }
                else
                {
                    // there are two options: either take the first element off list 1
                    // or take the first element off of list 2
                    this._cache[(min, max)] =
                        this.CountNumberOfWaysToInterleaveTwoLists(list1Count - 1, list2Count) +
                        this.CountNumberOfWaysToInterleaveTwoLists(list1Count, list2Count - 1);

                }
            }
            return this._cache[(min, max)];
        }
    }
}
