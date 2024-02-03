using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Riddles.Probability.Application
{
    public class RedAndGreenBallInUrnSolver
    {
        // you have an urn containing n balls. You have between 0 and n red balls
        // and the rest are green.
        public double CalculateOddsOfSecondBallBeingRed(int numBalls)
        {
            double[] probabilityOfUrnWithNRedBalls = new double[numBalls+1];
            double probabilityOfDrawingARedBall = 0 ;
            for(int numRedBalls=0; numRedBalls<=numBalls; numRedBalls++)
            {
                // use Bayes Thereom
                var numGreenBalls = numBalls - numRedBalls;
                probabilityOfUrnWithNRedBalls[numRedBalls] =
                    /* prior probability of being in this state before drawing the red ball */
                    (1.0 / (numBalls + 1))
                    /* probability of drawing a red ball a priori */
                    / 0.5
                    /* probability of drawing a red ball in this state */
                    * ((double)numRedBalls / (double)numBalls);
                probabilityOfDrawingARedBall +=
                    probabilityOfUrnWithNRedBalls[numRedBalls] *
                    ((double)(numRedBalls - 1) / (double)(numBalls - 1));
                
            }
            var cumulativeProbability = probabilityOfUrnWithNRedBalls.ToList().Sum();
            return probabilityOfDrawingARedBall;
        }
    }
}
