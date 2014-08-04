using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace PSS.DriveshaftCalculations.Calculations
{
    class Accelerations
    {
        //constants for converting between radians.
        const double RPM_TO_RAD_PER_SEC = 2 * Math.PI / 60;
        const double DEG_TO_RAD = Math.PI / 180;

        internal static double AngularSpeed(double rpm)
        {
            return rpm * RPM_TO_RAD_PER_SEC;
        }

        internal static double InertialDrive(double inertialDriveAngle, double rpm)
        {
            double radPerSec = rpm * RPM_TO_RAD_PER_SEC;

            return Math.Pow(radPerSec, 2) *
                   Math.Pow(Math.Sin(inertialDriveAngle * DEG_TO_RAD), 2); 
        }

        internal static double InertialCoast(double inertialCoastAngle, double rpm)
        {
            double radPerSec = rpm * RPM_TO_RAD_PER_SEC;

            return Math.Pow(radPerSec, 2) *
                   Math.Pow(Math.Sin(inertialCoastAngle * DEG_TO_RAD), 2);
        }

        internal static double Torsional(double torsionalAngle, double rpm)
        {
            double radPerSec = rpm * RPM_TO_RAD_PER_SEC;

            return Math.Pow(radPerSec, 2) *
                   Math.Pow(Math.Sin(torsionalAngle * DEG_TO_RAD), 2);
        }

        
    }
}
