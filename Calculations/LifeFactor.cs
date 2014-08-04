using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSS.DriveshaftCalculations.Calculations
{
    /// <summary>
    /// The Life factor is a factor used to determine the equivalent torque and is based 
    /// on the desired amount of hours of life for the u-joint.
    /// </summary>
    public class LifeFactor
    {
        public static double Value(int hours)
        {
            try
            {
                double retVal = 1;

                retVal = Math.Pow(0.000000000000002 * hours, 3) -
                         Math.Pow(0.0000000005 * hours, 2) +
                         .00004 * hours +
                         0.8017;

                return retVal;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
