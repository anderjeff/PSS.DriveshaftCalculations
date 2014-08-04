using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSS.DriveshaftCalculations.Calculations
{
    public class AngleFactor
    {
        public static double Value(double angle)
        {
            try
            {
                double retVal;
                if (angle > 3)
                {
                    //from the Driveshaft program graph.
                    retVal = Math.Pow((.0096 * angle), 3) -
                             Math.Pow((.477 * angle), 2) +
                             11.105 * angle +
                             70.712;

                    if (retVal / 100 < 1)
                    {
                        return 1;
                    }
                    else
                    {
                        return retVal / 100;
                    }
                }
                else
                {
                    return 1;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
