using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Riddles.Optimization
{
	public class Player {
		public int PlayerId { get; set; }
		public string Name { get; set; }
		public int ExpectedPoints { get; set; }
	}

	public class SnakeDraftSolver
	{
		public SnakeDraftSolver()
		{

		}

		public int FindOptimalDraftPosition<TPositionEnum>(
			int numTeams,
			Dictionary<int, Dictionary<TPositionEnum, int>> positionsNeededByTeam,
			Dictionary<TPositionEnum, List<Player>> playersAvailableByPosition) where TPositionEnum : System.Enum
		{
			var rostersByTeam = new Dictionary<int, List<Player>> { };
			var optimalDraft = this.FindOptimalDraftInternal(
				numTeams,
				0,
				positionsNeededByTeam,
				playersAvailableByPosition,
				Enumerable.Range(0, numTeams).ToDictionary(t => t, t => new List<Player>()),
				new Dictionary<string, Dictionary<int, List<Player>>> { },
				new Dictionary<string, string> { }
			);
			var scores = optimalDraft.Keys
				.Select(t => new { teamId = t, score = optimalDraft[t].Sum(p => p.ExpectedPoints) }).ToList();
			var bestScore = scores.OrderByDescending(s => s.score).First().teamId;
			return bestScore;
		}

		private Dictionary<int, List<Player>> FindOptimalDraftInternal<TPositionEnum>(
			int numTeams,
			int currentDraftPosition,
			Dictionary<int, Dictionary<TPositionEnum, int>> positionsNeededByTeam,
			Dictionary<TPositionEnum, List<Player>> playersAvailableByPosition,
			Dictionary<int, List<Player>> rostersByTeam,
			Dictionary<string, Dictionary<int, List<Player>>> memo,
			Dictionary<string, string> debugMemo) where TPositionEnum : System.Enum
		{
			var currentDraftRepresentation = this.GetCurrentDraftStateRepresentation(rostersByTeam);
			if (memo.ContainsKey(currentDraftRepresentation)) {
				return memo[currentDraftRepresentation];
			}	
			var currentTeam = this.GetCurrentTeamFromDraftPosition(currentDraftPosition, numTeams);
			var positionsNeeded = positionsNeededByTeam[currentTeam].Keys
				.Where(k => positionsNeededByTeam[currentTeam][k] > 0).ToList();
			
			if (positionsNeeded.Count == 0)
			{
				memo[currentDraftRepresentation] = new Dictionary<int, List<Player>>();
				for(int i=0; i<numTeams; i++)
				{
					memo[currentDraftRepresentation][i] = new List<Player>();
				}
				debugMemo[currentDraftRepresentation] = "";
				return memo[currentDraftRepresentation];
			}
			var bestPosition = positionsNeeded.First();
			var bestScore = 0;
			foreach(var position in positionsNeeded)
			{
				var bestPlayer = playersAvailableByPosition[position].OrderByDescending(p => p.ExpectedPoints).First();
				rostersByTeam[currentTeam].Add(bestPlayer);
				playersAvailableByPosition[position].Remove(bestPlayer);
				positionsNeededByTeam[currentTeam][position]--;

				var remainingDraft = this.FindOptimalDraftInternal(
					numTeams,
					currentDraftPosition + 1,
					positionsNeededByTeam,
					playersAvailableByPosition,
					rostersByTeam,
					memo,
					debugMemo
				);

				var score = (rostersByTeam[currentTeam]?.Sum(t => t.ExpectedPoints) ?? 0)
					+ (remainingDraft[currentTeam]?.Sum(t => t.ExpectedPoints) ?? 0);
				if(score > bestScore)
				{
					bestScore = score;
					bestPosition = position;
				}

				rostersByTeam[currentTeam].Remove(bestPlayer);
				playersAvailableByPosition[position].Add(bestPlayer);
				positionsNeededByTeam[currentTeam][position]++;
			}
			var bestPick = playersAvailableByPosition[bestPosition].OrderByDescending(p => p.ExpectedPoints).First();
			
			rostersByTeam[currentTeam].Add(bestPick);
			var nextDraftRepresentation = this.GetCurrentDraftStateRepresentation(rostersByTeam);
			var optimalDraft = memo[nextDraftRepresentation];
			var optimalDraftCopy = this.CopyDraft(optimalDraft);
			optimalDraftCopy[currentTeam].Add(bestPick);
			memo[currentDraftRepresentation] = optimalDraftCopy;
			debugMemo[currentDraftRepresentation] = this.GetCurrentDraftStateRepresentation(optimalDraftCopy);
			rostersByTeam[currentTeam].Remove(bestPick);
			return optimalDraftCopy;
		}

		private int GetCurrentTeamFromDraftPosition(int currentDraftPosition, int numTeams) {
			var mod = (currentDraftPosition % (2 * numTeams));
			return mod < numTeams
				? mod
				: (2 * numTeams) - mod - 1;
		}

		private string GetCurrentDraftStateRepresentation(Dictionary<int, List<Player>> currentDraft)
		{
			var teamInfo = currentDraft.Keys.OrderBy(k => k)
				.Select(k => $"{k}:{String.Join(",", currentDraft[k].Select(p => p.PlayerId))}");
			return String.Join("|", teamInfo);
		}

		private Dictionary<int, List<Player>> CopyDraft(Dictionary<int, List<Player>> draft)
		{
			Dictionary<int, List<Player>> copyDraft = new Dictionary<int, List<Player>>();
			foreach(var team in draft.Keys)
			{
				List<Player> players = new List<Player>();
				foreach(var player in draft[team])
				{
					players.Add(new Player
					{
						Name = player.Name,
						PlayerId = player.PlayerId,
						ExpectedPoints = player.ExpectedPoints
					});
				}
				copyDraft[team] = players;
			}
			return copyDraft;
		}
	}
}
