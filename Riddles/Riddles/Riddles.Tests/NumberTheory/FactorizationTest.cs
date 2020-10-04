using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.NumberTheory;
using System.Linq;

namespace Riddles.Tests.NumberTheory
{
	public class FactorizationTest
	{
		private Dictionary<int, List<Factorization.FactorPair>> expectedFactorPairs = new Dictionary<int, List<Factorization.FactorPair>>
		{
			{1, new List<Factorization.FactorPair> { new Factorization.FactorPair(1, 1) } },
			{2, new List<Factorization.FactorPair> { new Factorization.FactorPair(2, 1) } },
			{3, new List<Factorization.FactorPair> { new Factorization.FactorPair(3, 1) } },
			{7, new List<Factorization.FactorPair> { new Factorization.FactorPair(7, 1) } },
			{4, new List<Factorization.FactorPair> { new Factorization.FactorPair(4, 1), new Factorization.FactorPair(2, 3) } },
		};

		[TestCase(1, new int[] { 1 })]
		[TestCase(2, new int[] { 2 })]
		[TestCase(3, new int[] { 3 })]
		[TestCase(7, new int[] { 7 })]
		[TestCase(21, new int[] { 3, 7 })]
		[TestCase(1540, new int[] { 2, 2, 5, 7, 11 })]
		[TestCase(16, new int[] { 2, 2, 2, 2})]
		public void TestGetPrimeFactors(int n, int[] expectedPrimeFactors)
		{
			var factorization = new Factorization();
			var primeFactors = factorization.GetPrimeFactors(n).OrderBy(n => n).ToArray();
			Assert.AreEqual(expectedPrimeFactors.Length, primeFactors.Length);
			for(int i=0; i<primeFactors.Length; i++)
			{
				Assert.AreEqual(expectedPrimeFactors[i], primeFactors[i]);
			}
		}

		[TestCase(1)]
		[TestCase(2)]
		[TestCase(3)]
		[TestCase(7)]
		[TestCase(4)]
		public void TestGetFactors(int n)
		{
			var factorization = new Factorization();
			var factors = factorization.GetFactors(n).OrderBy(n => n.SmallerFactor).ToArray();
			var expectedFactors = factorization.GetFactors(n).OrderBy(n => n.SmallerFactor).ToArray();
			Assert.AreEqual(expectedFactors.Length, factors.Length);
			for(int i=0; i<factors.Length; i++)
			{
				Assert.AreEqual(expectedFactors[i].SmallerFactor, factors[i].SmallerFactor);
				Assert.AreEqual(expectedFactors[i].LargerFactor, factors[i].LargerFactor);
			}
		}
	}
}
