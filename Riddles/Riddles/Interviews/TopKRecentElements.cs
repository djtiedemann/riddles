using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Interviews
{
	public class TopKRecentElements
	{
		public int[] TopKMostFrequentElements(int[] array, int k) {
			int numUniqueItemsSeen = 0;
			HashSet<int> topKFrequentElements = new HashSet<int>();
			Dictionary<int, int> elementToCountDictionary = new Dictionary<int, int>();
			int minValueToBeInTopK = 1;

			foreach (var i in array) {
				// if you're a new element, you are in the top K if you're one of the first K elements seen
				if (!elementToCountDictionary.ContainsKey(i)) {
					elementToCountDictionary[i] = 1;
					numUniqueItemsSeen++;
					if (numUniqueItemsSeen <= k)
					{
						topKFrequentElements.Add(i);
					}
				}
				else
				{
					elementToCountDictionary[i]++;
					if (!topKFrequentElements.Contains(i))
					{
						if (elementToCountDictionary[i] > minValueToBeInTopK) {
							var minValueInTopKPlus1 = elementToCountDictionary[i];
							var minElement = i;
							foreach (var element in topKFrequentElements) {
								if (elementToCountDictionary[element] < minValueInTopKPlus1) {
									minValueInTopKPlus1 = elementToCountDictionary[element];
									minElement = element;
								}
							}
							if (minElement != i) {
								topKFrequentElements.Remove(minElement);
								topKFrequentElements.Add(i);
							}
							minValueToBeInTopK = topKFrequentElements.Select(e => elementToCountDictionary[e]).Min();
						}
					}
				}
			}

			int[] topK = new int[k];
			int position = 0;
			foreach (var element in topKFrequentElements) {
				topK[position] = element;
				position++;
			}
			return topK;
		}
	}
}
