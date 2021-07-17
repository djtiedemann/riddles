using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Interviews
{
	public class ThreeSum
	{
		public List<Tuple<int, int, int>> CalculateThreeSum(int[] values)
		{
			Dictionary<int, int> valuesSeen = new Dictionary<int, int> { };
			for(int i=0; i<values.Length; i++)
			{
				valuesSeen[values[i]] = valuesSeen.ContainsKey(values[i]) ? valuesSeen[values[i]] + 1 : 1;
			}
			HashSet<Tuple<int, int, int>> tuplesSummingToZero = new HashSet<Tuple<int, int, int>>();
			for(int i=0; i<values.Length; i++)
			{				
				for(int j=i+1; j<values.Length; j++)
				{
					var remainingValueToSumToZero = -1 * values[i] - values[j];
					if (!valuesSeen.ContainsKey(remainingValueToSumToZero)) { continue; }
					List<int> tripleSummingToZero = new List<int> { values[i], values[j], remainingValueToSumToZero };
					var numInstancesOfRemainingValueSummingToZero = tripleSummingToZero.Where(v => v == remainingValueToSumToZero).Count();
					var numOccurrences = valuesSeen[remainingValueToSumToZero];
					if (numOccurrences >= numInstancesOfRemainingValueSummingToZero) {
						tripleSummingToZero = tripleSummingToZero.OrderBy(v => v).ToList();
						var triple = Tuple.Create(tripleSummingToZero[0], tripleSummingToZero[1], tripleSummingToZero[2]);
						if (!tuplesSummingToZero.Contains(triple)) {
							tuplesSummingToZero.Add(triple);
						}
					}
				}
			}
			return tuplesSummingToZero.ToList();
		}
	}
}
