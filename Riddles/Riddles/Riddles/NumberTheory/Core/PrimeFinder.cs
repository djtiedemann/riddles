using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.NumberTheory.Core
{
	public class PrimeFinder
	{
		private HashSet<int> primeNumbers;
		private HashSet<int> compositeNumbers;

		public PrimeFinder()
		{
			primeNumbers = new HashSet<int> { 1 };
			compositeNumbers = new HashSet<int> { };
		}

		public List<int> FindPrimeNumbersLessThanOrEqualTo(int n)
		{
			for (int i=2; i<=n; i++)
			{
				if (compositeNumbers.Contains(i) || primeNumbers.Contains(i))
				{
					continue;
				}
				this.AddNewPrimeNumberToStructure(i, n);
			}
			return primeNumbers.OrderBy(i => i).ToList();
		}

		public void AddNewPrimeNumberToStructure(int newPrime, int largestNumberInConsideration)
		{
			primeNumbers.Add(newPrime);
			int newMultiple = 2;
			while(newPrime*newMultiple <= largestNumberInConsideration)
			{
				var nextComposite = newPrime * newMultiple;
				if (!compositeNumbers.Contains(nextComposite))
				{
					compositeNumbers.Add(nextComposite);
				}
				newMultiple++;
			}
		}
	}
}
