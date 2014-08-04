using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSS.DriveshaftCalculations.Calculations
{
    public class PowerCalculations
    {
        public static double Torque(double hp, int speed)
        {
            try
            {
                if (speed > 0)
                {
                    return hp * 5252.113 / speed;
                }
                else
                {
                    return double.MaxValue;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static double Horsepower(double torque, int speed)
        {
            try
            {
                double hp = torque * speed / 5252.113;

                if (hp > (double)Decimal.MaxValue)
                {
                    return (double)Decimal.MaxValue;
                }
                else
                {
                    return hp;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
