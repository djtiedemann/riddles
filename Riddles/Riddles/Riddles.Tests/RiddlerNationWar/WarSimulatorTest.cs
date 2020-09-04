using NUnit.Framework;
using Riddles.RiddlerNationWar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Tests.RiddlerNationWar
{
	public class WarSimulatorTest
	{

		private List<int> tooFewCastles = new List<int> { 100, 100, 100, 700 };
		private List<int> tooManyCastles = new List<int> { 100, 100, 100, 100, 100, 500 };
		private List<int> tooFewTroops = new List<int> { 100, 100, 100, 100, 100 };
		private List<int> tooManyTroops = new List<int> { 300, 300, 300, 300, 300 };
		private List<int> validDistribution1 = new List<int> { 200, 200, 200, 200, 200 };
		private List<int> validDistribution2 = new List<int> { 500, 0, 0, 0, 500 };
		private List<int> validDistribution3 = new List<int> { 0, 0, 1000, 0, 0 };

		[Test]
		public void TestValidation()
		{
			var warSimulator = new WarSimulator(5, 1000);
			var invalidSubmission1 = new List<List<int>> { tooFewCastles };
			var invalidSubmission2 = new List<List<int>> { tooManyCastles };
			var invalidSubmission3 = new List<List<int>> { tooFewTroops };
			var invalidSubmission4 = new List<List<int>> { tooManyTroops };
			var invalidSubmission5 = new List<List<int>> { tooManyTroops, tooFewTroops, tooManyCastles };
			var invalidSubmission6 = new List<List<int>> { tooFewCastles, validDistribution1, validDistribution2 };

			var validSubmission1 = new List<List<int>> { validDistribution3 };
			var validSubmission2 = new List<List<int>> { validDistribution1, validDistribution2, validDistribution3 };

			Assert.IsFalse(warSimulator.ValidateSubmissions(invalidSubmission1));
			Assert.IsFalse(warSimulator.ValidateSubmissions(invalidSubmission2));
			Assert.IsFalse(warSimulator.ValidateSubmissions(invalidSubmission3));
			Assert.IsFalse(warSimulator.ValidateSubmissions(invalidSubmission4));
			Assert.IsFalse(warSimulator.ValidateSubmissions(invalidSubmission5));
			Assert.IsFalse(warSimulator.ValidateSubmissions(invalidSubmission6));

			Assert.IsTrue(warSimulator.ValidateSubmissions(validSubmission1));
			Assert.IsTrue(warSimulator.ValidateSubmissions(validSubmission2));
		}

		[TestCase(new int [] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, new int [] { 10, 10, 5, 0, 0, 0, 0, 25, 25, 25 }, 26.5, 28.5, 10, 100)]
		[TestCase(new int[] { 0, 3, 0 }, new int[] { 0, 0, 3 }, 2, 4, 2.5, 3.5)]
		public void TestPlayGame(int[] playerSubmission, int[] opponentSubmission, double expectedPlayerScore, double expectedOpponentScore, int numCastles,
			int numTroops)
		{
			var warSimulator = new WarSimulator(numCastles, numTroops);

			(double playerScore, double opponentScore) = warSimulator.PlayGame(playerSubmission.ToList(), opponentSubmission.ToList());
			Assert.AreEqual(expectedPlayerScore, playerScore);
			Assert.AreEqual(expectedOpponentScore, opponentScore);
		}
	}
}
