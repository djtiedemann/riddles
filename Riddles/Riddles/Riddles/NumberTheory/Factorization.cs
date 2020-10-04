using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.NumberTheory
{
	public class Factorization
	{
		private PrimeFinder primeFinder;
		public Factorization()
		{
			primeFinder = new PrimeFinder();
		}

		public List<int> GetPrimeFactors(int n)
		{
			if(n <= 0)
			{
				return new List<int> { };
			}
			if(n == 1)
			{
				return new List<int> { 1 };
			}
			var primeFactors = new List<int>();
			var primes = primeFinder.FindPrimeNumbersLessThanOrEqualTo(n).Where(p => p != 1);
			var remainingValueToFactor = n;
			while (!primes.Contains(remainingValueToFactor))
			{
				foreach(int prime in primes)
				{
					if(remainingValueToFactor % prime == 0)
					{
						primeFactors.Add(prime);
						remainingValueToFactor = remainingValueToFactor / prime;
						break;
					}
				}
			}
			primeFactors.Add(remainingValueToFactor);
			return primeFactors;
		}

		public List<FactorPair> GetFactors(int n)
		{
			var primeFactors = this.GetPrimeFactors(n).ToArray();
			var factors = new List<FactorPair>();
			if(primeFactors.Length == 0)
			{
				return factors;
			}
			// in order to find the factors, we create all subsets of prime factors and put them in one of two sets
			// the values in the first set will form the first factor, and the second set will form the second set
			// since each pair of factors will be represented twice, we will assume the first value will always be in the first set
			// we've also already added the value of (1, n).
			//
			// if we represent these by bit strings, we need to iterate from i=2^(n-1) (the most significant bit is a 1) 
			// to i=(i^2n) - 1
			//
			// it's also possible for the same value to be added multiple times.
			// for instance for n=16, the prime factorization is (2, 2, 2, 2)
			// bitstrings 1010 and 1001 create the same value. because of that, we need to implement equality and make sure that it doesn't already exist
			var numPrimesInPrimeFactorization = primeFactors.Length;
			for(int i=(int)Math.Pow(2, numPrimesInPrimeFactorization-1); i<= (int)Math.Pow(2, numPrimesInPrimeFactorization) - 1; i++)
			{
				var bitString = Convert.ToString(i, 2).PadLeft(numPrimesInPrimeFactorization, '0').ToCharArray().Select(a => a - '0').ToArray();
				var factor1 = 1;
				var factor2 = 1;
				for(int prime=0; prime < numPrimesInPrimeFactorization; prime++)
				{
					if(bitString[prime] == 1)
					{
						factor1 *= primeFactors[prime];
					}
					else
					{
						factor2 *= primeFactors[prime];
					}
				}
				var newFactor = new FactorPair(factor1, factor2);
				if(!factors.Any(f => f.SmallerFactor == newFactor.SmallerFactor && f.LargerFactor == newFactor.LargerFactor))
				{
					factors.Add(newFactor);
				}
			}
			return factors;
		}

		public class FactorPair
		{
			public int SmallerFactor { get; private set; }
			public int LargerFactor { get; private set; }
			public FactorPair(int a, int b)
			{
				SmallerFactor = Math.Min(a, b);
				LargerFactor = Math.Max(a, b);
			}
		}
	}
}
