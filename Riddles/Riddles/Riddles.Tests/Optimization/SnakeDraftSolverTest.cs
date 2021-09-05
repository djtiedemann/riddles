using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Optimization;

namespace Riddles.Tests.Optimization
{
	public enum Position
	{
		Quarterback = 1,
		RunningBack = 2,
		WideReceiver = 3
	}

	public class SnakeDraftSolverTest
	{
		private Dictionary<int, Dictionary<Position, List<Player>>> availablePlayersByTestCase
			= new Dictionary<int, Dictionary<Position, List<Player>>>
			{
				{1,  new Dictionary<Position, List<Player>> {
					{
						Position.Quarterback,
						new List<Player> {
							new Player { Name = "Patrick Mahomes", PlayerId = 1, ExpectedPoints = 400 },
							new Player { Name = "Josh Allen", PlayerId = 2, ExpectedPoints = 350 },
						}
					},
					{
						Position.RunningBack,
						new List<Player> {
							new Player { Name = "Christian McCaffrey", PlayerId = 4, ExpectedPoints = 300 },
							new Player { Name = "Dalvin Cook", PlayerId = 5, ExpectedPoints = 225 },
						}
					},
					}
				},
				{2,  new Dictionary<Position, List<Player>> {
					{
						Position.Quarterback,
						new List<Player> {
							new Player { Name = "Patrick Mahomes", PlayerId = 1, ExpectedPoints = 400 },
							new Player { Name = "Josh Allen", PlayerId = 2, ExpectedPoints = 350 },
							new Player { Name = "Kyler Murray", PlayerId = 3, ExpectedPoints = 300 },
						}
					},
					{
						Position.RunningBack,
						new List<Player> {
							new Player { Name = "Christian McCaffrey", PlayerId = 4, ExpectedPoints = 300 },
							new Player { Name = "Dalvin Cook", PlayerId = 5, ExpectedPoints = 225 },
							new Player { Name = "Derrick Henry", PlayerId = 6, ExpectedPoints = 200 },
						}
					},
					{
						Position.WideReceiver,
						new List<Player> {
							new Player { Name = "Davante Adams", PlayerId = 7, ExpectedPoints = 250 },
							new Player { Name = "Tyreek Hill", PlayerId = 8, ExpectedPoints = 225 },
							new Player { Name = "Stephon Diggs", PlayerId = 9, ExpectedPoints = 175 },
						}
					},
				}
			}
		};
		private Dictionary<int, Dictionary<Position, int>> teamNeedsByTestCaseId = new Dictionary<int, Dictionary<Position, int>> {
			{1,  new Dictionary<Position, int> {
					{ Position.Quarterback, 1}, { Position.RunningBack, 1} } },
			{2,  new Dictionary<Position, int> {
					{ Position.Quarterback, 1}, { Position.RunningBack, 1}, { Position.WideReceiver, 1} } }
		};

		[TestCase(1, 1, 2, 0)]
		[TestCase(2, 2, 3, 1)]
		public void TestFindOptimalDraftPosition(int playersAvailableTestCaseId, int teamNeedsTestCaseId, int numTeams, int expectedOptimalTeam)
		{
			var snakeDraftSolver = new SnakeDraftSolver();
			var availablePlayers = availablePlayersByTestCase[playersAvailableTestCaseId];
			var needsByTeam = this.GetTeamNeedsByTeam(teamNeedsByTestCaseId[teamNeedsTestCaseId], numTeams);
			var optimalPosition = snakeDraftSolver.FindOptimalDraftPosition(numTeams, needsByTeam, availablePlayers);
			Assert.AreEqual(expectedOptimalTeam, optimalPosition);
		}

		private Dictionary<int, Dictionary<Position, int>> GetTeamNeedsByTeam(Dictionary<Position, int> teamNeeds, int numTeams) {
			var teamNeedsByTeam = new Dictionary<int, Dictionary<Position, int>>();
			for (int teamId = 0; teamId < numTeams; teamId++) {
				var teamNeedsCopy = new Dictionary<Position, int>();
				foreach(var position in teamNeeds.Keys)
				{
					teamNeedsCopy[position] = teamNeeds[position];
				}
				teamNeedsByTeam[teamId] = teamNeedsCopy;
			}
			return teamNeedsByTeam;
		}
	}
}
