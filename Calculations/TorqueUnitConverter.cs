using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSS.DriveshaftCalculations.Calculations
{
    public static class TorqueUnitConverter
    {
        public enum StartingUnits
        {
            kNm,
            Nm,
            InLb,
            LbFt
        }

        public static double ConvertToFootPounds(StartingUnits units, double value)
        {
            try
            {
                double retVal = 0;

                switch (units)
                {
                    case (StartingUnits.LbFt):
                        retVal = value * 1;
                        break;
                    case (StartingUnits.kNm):
                        retVal = value * 737.56215;
                        break;
                    case (StartingUnits.Nm):
                        retVal = value * .7375622;
                        break;
                    case (StartingUnits.InLb):
                        retVal = value / 12;
                        break;
                }

                return retVal;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static double ConvertToKiloNewtonMeters(StartingUnits units, double value)
        {
            try
            {
                double retVal = 0;

                switch (units)
                {
                    case (StartingUnits.LbFt):
                        retVal = value / 737.56215;
                        break;
                    case (StartingUnits.kNm):
                        retVal = value * 1;
                        break;
                    case (StartingUnits.Nm):
                        retVal = value / 1000;
                        break;
                    case (StartingUnits.InLb):
                        retVal = value / 8850.7458;
                        break;
                }

                return retVal;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
