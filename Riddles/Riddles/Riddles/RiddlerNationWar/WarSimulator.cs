using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.RiddlerNationWar
{
	public class WarSimulator
	{
		private int _numCastles;
		private int _numTroops;
		public WarSimulator(int numCastles, int numTroops)
		{
			this._numCastles = numCastles;
			this._numTroops = numTroops;
		}

		public IEnumerable<WarSimulationResultForTesting> SimulateWars(List<SimulationEntry> submissions)
		{
			if (!this.ValidateSubmissions(submissions))
			{
				throw new InvalidOperationException("Not all submissions are valid");
			}
			return this.EvaluateSubmissions(submissions);
		}

		public IEnumerable<WarSimulationResult> SimulateWars(List<List<int>> submissions)
		{
			var sumbissionsTransformed = submissions.Select(s => new SimulationEntry { Id = null, IsTestSubmission = false, TroopAllocation = s }).ToList();
			var result = this.EvaluateSubmissions(sumbissionsTransformed);
			return result.Select(r => new WarSimulationResult { Submission = r.Submission, NumWins = r.NumWins });
		}

		private IEnumerable<WarSimulationResultForTesting> EvaluateSubmissions(List<SimulationEntry> submissions)
		{
			var results = new List<WarSimulationResultForTesting>();
			for (int submission1 = 0; submission1 < submissions.Count; submission1++)
			{
				var wins = 0;
				for (int submission2 = 0; submission2 < submissions.Count; submission2++)
				{
					if (submission1 != submission2)
					{
						(var playerScore, var opponentScore) = this.PlayGame(submissions[submission1].TroopAllocation, 
							submissions[submission2].TroopAllocation);
						if(playerScore > opponentScore)
						{
							wins += 1;
						}
					}
				}
				results.Add(new WarSimulationResultForTesting { 
					Submission = submissions[submission1].TroopAllocation, 
					NumWins = wins,
					IsTestSubmission = submissions[submission1].IsTestSubmission,
					Id = submissions[submission1].Id
				});
			}
			return results.OrderByDescending(r => r.NumWins);
		}

		public (double playerScore, double opponentScore) PlayGame(List<int> playerSubmission, List<int> opponentSubmission)
		{
			double playerScore = 0;
			double opponentScore = 0;
			for (int castle=0; castle < playerSubmission.Count; castle++)
			{
				double pointsForCastle = castle + 1;
				if(playerSubmission[castle] > opponentSubmission[castle])
				{
					playerScore += pointsForCastle;
				}
				if(opponentSubmission[castle] > playerSubmission[castle])
				{
					opponentScore += pointsForCastle;
				}
				if(playerSubmission[castle] == opponentSubmission[castle])
				{
					playerScore += (pointsForCastle / 2.0);
					opponentScore += (pointsForCastle / 2.0);
				}
			}
			return (playerScore, opponentScore);
		}

		public bool ValidateSubmissions(List<List<int>> submissions)
		{
			var sumbissionsTransformed = submissions.Select(s => new SimulationEntry { Id = null, IsTestSubmission = false, TroopAllocation = s }).ToList();
			return this.ValidateSubmissions(sumbissionsTransformed);
		}

		public bool ValidateSubmissions(List<SimulationEntry> submissions)
		{
			foreach(var submission in submissions)
			{
				if(submission.TroopAllocation.Count != this._numCastles)
				{
					return false;
				}
				var sum = submission.TroopAllocation.Sum(x => x);
				if(sum != this._numTroops)
				{
					return false;
				}
			}
			return true;
		}

		public class SimulationEntry
		{
			public int? Id { get; set; }
			public bool IsTestSubmission { get; set; }
			public List<int> TroopAllocation { get; set; }
		}
		public class WarSimulationResult
		{
			public List<int> Submission { get; set; }
			public double NumWins { get; set; }
		}

		public class WarSimulationResultForTesting
		{
			public List<int> Submission { get; set; }
			public double NumWins { get; set; }
			public int? Id { get; set; }
			public bool IsTestSubmission { get; set; }
		}
	}
}
