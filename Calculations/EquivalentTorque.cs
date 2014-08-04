using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSS.DriveshaftCalculations.Calculations
{
    /// <summary>
    /// Determines the equivalent torque
    /// </summary>
    public class EquivalentTorque
    {
        public static double Value(double powerFactor, double angleFactor, double lifeFactor, double nominalTorque)
        {
            try
            {
                return nominalTorque * powerFactor * angleFactor * lifeFactor;
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
