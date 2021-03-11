using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Interviews
{
	public class SumOfSquares
	{
		public int FindMinimumNumberOfSquaresSummingTo(int n) {
			int sqrt = (int)Math.Sqrt(n);
			int[] squares = new int[sqrt];
			for (int i = 1; i <= sqrt; i++) {
				squares[i - 1] = i * i;
			}

			List<Tuple<int, int>> valuesToProcess = new List<Tuple<int, int>> { Tuple.Create(n, 0) };
			HashSet<int> numsAlreadySeen = new HashSet<int>();
			numsAlreadySeen.Add(n);
			while (valuesToProcess.Count > 0) {
				var nextValue = valuesToProcess.First();
				valuesToProcess.RemoveAt(0);

				foreach (var square in squares) {
					var nextNum = nextValue.Item1 - square;
					if (nextNum == 0 ) {
						return nextValue.Item2 + 1;
					}
					if (nextNum > 0 && !numsAlreadySeen.Contains(nextNum)) {
						numsAlreadySeen.Add(nextNum);
						valuesToProcess.Add(Tuple.Create(nextNum, nextValue.Item2 + 1));
					}					
				}
			}
			return -1;
		}
	}
}
