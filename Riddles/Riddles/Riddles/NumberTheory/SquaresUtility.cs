using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.NumberTheory
{
	public class SquaresUtility
	{
		public SquaresUtility()
		{

		}

		public long? FindSquareBySummingOddNumbersUpTo(int n)
		{
			if (n == 0)
			{
				return 0;
			}
			if (n < 0)
			{
				return null;
			}
			if (n % 2 == 0)
			{
				return null;
			}

			// n = 2*root - 1
			// root = (n + 1)/2
			// square = ((n+1)/2)^2
			return (long)Math.Pow((n + 1) / 2, 2);
		}
	}
}
