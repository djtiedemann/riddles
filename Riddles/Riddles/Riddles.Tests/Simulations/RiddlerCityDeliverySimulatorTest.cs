using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Riddles.Geometry.Core;
using Riddles.Simulations;

namespace Riddles.Tests.Simulations
{
    /* https://fivethirtyeight.com/features/can-you-make-a-speedy-delivery/
     * 
     * You can calculate the correct answer with calculus. This runs simulations
     * as a way to double check that result. For standard Manhattan distance, the answer is
     * 4/PI and for Manhattan distance with diagonals, the answer is 2*(sqrt(2) - 1)*4/PI
     * 
     * First, the city is a circle so when calculating the added distance travelled by the scooter,
     * you need to find the average extra distance across all possible theta values between
     * 0 and 2*PI. Note, radius doesn't matter because it's a ratio.
     * 
     * Note, that in each case, there is symmetry in each quadrant, so can just calculate 
     * the average distance from all thetas from 0 to PI/4. So we can integrate the 
     * function for manhattan distance and manhattan distance with diagonals and divide by
     * the range (PI/4).
     * 
     * The manhattan distance function is: cos(theta) + sin(theta). Note that in the cardinal
     * directions this is just 1 and it's highest value is at PI/4.
     * 
     * Using trigonometry, you can find that the manhattan distance with diagonals is:
     * cos(theta) + (sqrt(2) - 1) * sin(theta) in the case where cos(theta) > sin(theta)
     * and
     * sin(theta) + (sqrt(2) - 1) * cos(theta) in the case where sin(theta) > cos(theta)
     * 
     * Because of symmetry, we only need to consider the first case in the range of 0 to PI/4
     */
    public class RiddlerCityDeliverySimulatorTest
    {
        //[TestCase(1_000_000)]
        public void FindAverageManhattanDistance(int numIterations)
        {
            var random = new Random();
            var geometryUtilities = new GeometryUtilities();
            var runningTotal = 0.0;
            for(int i=0; i<numIterations; i++)
            {
                var theta = random.NextDouble() * Math.PI * 2;
                runningTotal += geometryUtilities.CalculateManhattanDistance(1, theta);
            }
            var answer = runningTotal / numIterations;
        }

        //[TestCase(1_000_000)]
        public void FindAverageManhattanDistanceWithDiagonals(int numIterations) 
        {
            var random = new Random();
            var geometryUtilities = new GeometryUtilities();
            var runningTotal = 0.0;
            for (int i = 0; i < numIterations; i++)
            {
                var theta = random.NextDouble() * Math.PI * 2;
                runningTotal += geometryUtilities.CalculateManhattanDistanceWithDiagonals(1, theta);
            }
            var answer = runningTotal / numIterations;
        }
    }
}
