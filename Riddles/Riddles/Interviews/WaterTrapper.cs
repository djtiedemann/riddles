using System;
using System.Collections.Generic;
using System.Text;

namespace Interviews
{
	public class WaterTrapper
	{
		public int Trap(int[] height)
		{
			Stack<Tuple<int, int>> mostRecentLeftWall = new Stack<Tuple<int, int>> { };
			int waterTrapped = 0;
			for (int i = 0; i < height.Length; i++)
			{
				if (height[i] > 0)
				{
					int previousHeightPopped = 0;
					while (mostRecentLeftWall.Count > 0) {
						var lastPartOfWall = mostRecentLeftWall.Peek();
						var incrementalWidth = i - (lastPartOfWall.Item2 + 1);
						var incrementalHeight = Math.Min(lastPartOfWall.Item1, height[i]) - previousHeightPopped;
						waterTrapped += incrementalHeight * incrementalWidth;
						previousHeightPopped = lastPartOfWall.Item1;						
						if(lastPartOfWall.Item1 > height[i])
						{
							break;
						}
						mostRecentLeftWall.Pop();
					}
					mostRecentLeftWall.Push(Tuple.Create(height[i], i));
				}
			}
			return waterTrapped;
		}
	}
}
