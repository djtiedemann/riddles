using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class DungeonKnightPrincessSolver
	{
        public int CalculateMinimumHP(int[][] dungeon)
        {
            Dictionary<Tuple<int, int>, int> hitPointsForSquare =
                new Dictionary<Tuple<int, int>, int> { };
            var princessLocation = Tuple.Create(dungeon.Length - 1, dungeon[0].Length - 1);
            var locationsToProcess = new List<Tuple<int, int>> { princessLocation };
            hitPointsForSquare[princessLocation] = Math.Max(1, 1 - dungeon[princessLocation.Item1][princessLocation.Item2]);
            HashSet<Tuple<int, int>> locationsAlreadyProcessed = new HashSet<Tuple<int, int>> { princessLocation };            
            while (locationsToProcess.Count > 0) {
                var currentLocation = locationsToProcess[0];
                locationsToProcess.RemoveAt(0);
                var nextLocationInUpwardDirection = Tuple.Create(currentLocation.Item1-1, currentLocation.Item2);
                var nextLocationInLeftDirection = Tuple.Create(currentLocation.Item1, currentLocation.Item2 - 1);
                if(nextLocationInLeftDirection.Item2 >= 0 && !locationsAlreadyProcessed.Contains(nextLocationInLeftDirection))
				{
                    var minimumHpForSquare = this.CalculateMinimumHpForSquare(nextLocationInLeftDirection, dungeon, hitPointsForSquare);
                    if (nextLocationInLeftDirection.Item1 == 0 && nextLocationInLeftDirection.Item2 == 0) {
                        return minimumHpForSquare;
                    }
                    hitPointsForSquare[nextLocationInLeftDirection] = minimumHpForSquare;
                    locationsToProcess.Add(nextLocationInLeftDirection);
                    locationsAlreadyProcessed.Add(nextLocationInLeftDirection);
                }
                if(nextLocationInUpwardDirection.Item1 >= 0 && !locationsAlreadyProcessed.Contains(nextLocationInUpwardDirection))
				{
                    var minimumHpForSquare = this.CalculateMinimumHpForSquare(nextLocationInUpwardDirection, dungeon, hitPointsForSquare);
                    if (nextLocationInUpwardDirection.Item1 == 0 && nextLocationInUpwardDirection.Item2 == 0)
                    {
                        return minimumHpForSquare;
                    }
                    hitPointsForSquare[nextLocationInUpwardDirection] = minimumHpForSquare;
                    locationsToProcess.Add(nextLocationInUpwardDirection);
                    locationsAlreadyProcessed.Add(nextLocationInUpwardDirection);
                }
            }
            return -1;
        }

        public int CalculateMinimumHpForSquare(Tuple<int, int> currentLocation, int[][] dungeon, Dictionary<Tuple<int, int>, int> hitPointsForSquare)
		{
            var dungeonLocationInBottomDirection = Tuple.Create(currentLocation.Item1 + 1, currentLocation.Item2);
            var dungeonLocationInRightDirection = Tuple.Create(currentLocation.Item1, currentLocation.Item2 + 1);

            var hpNeededForRightDirection = dungeonLocationInRightDirection.Item2 >= dungeon[0].Length
                ? int.MaxValue : hitPointsForSquare[dungeonLocationInRightDirection];
            var hpNeededForBottomDirection = dungeonLocationInBottomDirection.Item1 >= dungeon.Length
                ? int.MaxValue : hitPointsForSquare[dungeonLocationInBottomDirection];
            var minHpNeededAfterThisSquare = Math.Min(hpNeededForRightDirection, hpNeededForBottomDirection);
            var minimumHpNeededByThisSquare = Math.Max(1, minHpNeededAfterThisSquare - dungeon[currentLocation.Item1][currentLocation.Item2]);
            return minimumHpNeededByThisSquare;
        }
    }
}
