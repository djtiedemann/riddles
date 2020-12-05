using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Probability;

namespace Riddles.Tests.Probability
{
	public class RandomBallPainterTest
	{
		[TestCase(1, 0, Description = "Single ball")]
		[TestCase(2, 1, Description = "Two balls")]
		[TestCase(3, 4, Description = "Three balls")]
		[TestCase(4, 9, Description = "Four balls")]
		public void TestGetExpectedNumberOfTurnsToPaintBalls(int numBalls, double? expectedValue)
		{
			double epsilon = 0.0001;
			var ballPainter = new RandomBallPainter();
			var expectedNumberOfTurnsToPaintAllBallsSameColor = ballPainter.GetExpectedNumberOfTurnsToPaintBalls(numBalls);
			if(expectedValue == null || expectedNumberOfTurnsToPaintAllBallsSameColor == null)
			{
				Assert.IsNull(expectedValue);
				Assert.IsNull(expectedNumberOfTurnsToPaintAllBallsSameColor);
			}
			else
			{
				Assert.LessOrEqual(Math.Abs(expectedValue.Value - expectedNumberOfTurnsToPaintAllBallsSameColor.Value), epsilon);
			}			
		}
	}
}
