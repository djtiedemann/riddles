using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class UniquePathSolver
	{
		public int UniquePathsWithObstacles(int[,] obstacleGrid)
		{
			int[,] numDistinctPaths = new int[obstacleGrid.GetLength(0), obstacleGrid.GetLength(1)];
			numDistinctPaths[0, 0] = 1;
			Queue<Tuple<int, int>> statesToProcess = new Queue<Tuple<int, int>> { };
			statesToProcess.Enqueue(Tuple.Create(0, 0));
			while (statesToProcess.Count > 0)
			{
				var currentState = statesToProcess.Dequeue();
				var nextStateRight = Tuple.Create(currentState.Item1, currentState.Item2 + 1);
				var nextStateDown = Tuple.Create(currentState.Item1 + 1, currentState.Item2);
				if (nextStateRight.Item2 < obstacleGrid.GetLength(1))
				{
					if (obstacleGrid[nextStateRight.Item1, nextStateRight.Item2] == 1)
					{
						if (numDistinctPaths[nextStateRight.Item1, nextStateRight.Item2] == 0)
						{
							statesToProcess.Enqueue(nextStateRight);
						}
						numDistinctPaths[nextStateRight.Item1, nextStateRight.Item2] += numDistinctPaths[currentState.Item1, currentState.Item2];
					}
				}
				if (nextStateDown.Item1 < obstacleGrid.GetLength(0))
				{
					if (obstacleGrid[nextStateDown.Item1, nextStateDown.Item2] == 1)
					{
						if (numDistinctPaths[nextStateDown.Item1, nextStateDown.Item2] == 0)
						{
							statesToProcess.Enqueue(nextStateDown);
						}
						numDistinctPaths[nextStateDown.Item1, nextStateDown.Item2] += numDistinctPaths[currentState.Item1, currentState.Item2];
					}
				}
			}
			return numDistinctPaths[obstacleGrid.GetLength(0) - 1, obstacleGrid.GetLength(1) - 1];
		}
	}
}
