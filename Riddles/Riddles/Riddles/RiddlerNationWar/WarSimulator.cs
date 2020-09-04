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

		public void SimulateWars(List<List<int>> submissions)
		{
			if (!this.ValidateSubmissions(submissions))
			{
				throw new InvalidOperationException("Not all submissions are valid");
			}

		}

		//private List<Tuple<List<int>, double>> EvaluateSubmissions(List<List<int>> submissions)
		//{
		//	for(int submission1 = 0; submission1 < submissions.Count; submission1++)
		//	{
		//		for(int submission2 = 0; submission2 < submissions.Count; submission2++)
		//		{
		//			if(submission1 != submission2)
		//			{

		//			}
		//		}
		//	}
		//}

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
			foreach(var submission in submissions)
			{
				if(submission.Count != this._numCastles)
				{
					return false;
				}
				var sum = submission.Sum(x => x);
				if(sum != this._numTroops)
				{
					return false;
				}
			}
			return true;
		}
	}
}
