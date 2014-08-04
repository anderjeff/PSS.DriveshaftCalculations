using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSS.DriveshaftCalculations.Calculations
{
    /// <summary>
    /// The value of the maximum expected shock load for a driveshaft
    /// in a particular application.
    /// </summary>
    public class ShockLoad
    {
        public static double Value(double nominalTorque, double serviceFactor)
        {
            return nominalTorque * serviceFactor;
        }
    }
}
