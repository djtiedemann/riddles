using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.NumberTheory.Core;

namespace Riddles.Tests.NumberTheory
{
	public class PrimeFinderTest
	{
		[TestCase(100, new int[] { 1, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97})]
		public void TestFindPrimeNumbers(int n, int[] expectedPrimes)
		{
			var primeFinder = new PrimeFinder();
			var primes = primeFinder.FindPrimeNumbersLessThanOrEqualTo(n).ToArray();
			Assert.AreEqual(expectedPrimes.Length, primes.Length);
			for (int i=0; i<primes.Length; i++)
			{
				Assert.AreEqual(expectedPrimes[i], primes[i]);
			}
		}
	}
}
