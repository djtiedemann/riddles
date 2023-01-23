using System;
using System.Collections.Generic;
using System.Text;

namespace Riddles.Simulations
{
    // https://fivethirtyeight.com/features/can-you-make-a-speedy-delivery/
    public class RiddlerCityDeliverySimulator
    {
        /* 
         * Can solve this analytically through integration, by integrating this
         * function from 0 to PI/4 (there is symmetry for each segment of PI/4).
         * Then you need to divide by density (PI/4). 
         * 
         * This holds true for the second part as well with the only difference being the 
         * function to integrate. In this case it is: cos(theta) + (Sqrt(2) - 1)*sin(theta).
         * This function can be found via trigonometry.
         */
        
    }
}
