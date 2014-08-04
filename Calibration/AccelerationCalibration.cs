using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PSS.DriveshaftCalculations.DriveshaftParts;


namespace PSS.DriveshaftCalculations.Calibration
{
    public class AccelerationCalibration : ICalibration
    {
        public AccelerationCalibration()
        {
            try
            {
                PerformCalibration();
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private string _calibrationResult;
        public string CalibrationResult
        {
            get
            {
                return _calibrationResult;
            }
            set
            {
                _calibrationResult = value;
            }
        }

        private void PerformCalibration()
        {
            try
            {
                DriveshaftLayout layout = ImaginaryLayout();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("***VERIFICATION OF ACCELERATION CALCULATIONS***");
                sb.AppendLine();
                sb.AppendLine(string.Format("Performed: {0:M/d/yyyy h:mm:ss tt}", DateTime.Now));
                sb.AppendLine();

                sb.AppendLine("PARAMETERS:");
                sb.AppendLine();
                sb.AppendLine(string.Format("RPM: {0:n0}", layout.Rpm));
                sb.AppendLine();
                sb.AppendLine("Name\t\tPhasing");
                sb.AppendLine(string.Format("{0}\tCrossed", layout.Driveshafts[0].Name));
                sb.AppendLine(string.Format("{0}\tParallel", layout.Driveshafts[1].Name));
                sb.AppendLine(string.Format("{0}\tParallel", layout.Driveshafts[2].Name));
                sb.AppendLine();

                sb.AppendLine("Name\t\tAngle");
                sb.AppendLine(string.Format("{0} (Θ1)\t{1:n2}", layout.UJoints[0].Name, layout.UJoints[0].JointAngle));
                sb.AppendLine(string.Format("{0} (Θ2)\t{1:n2}", layout.UJoints[1].Name, layout.UJoints[1].JointAngle));
                sb.AppendLine(string.Format("{0} (Θ3)\t{1:n2}", layout.UJoints[2].Name, layout.UJoints[2].JointAngle));
                sb.AppendLine(string.Format("{0} (Θ4)\t{1:n2}", layout.UJoints[3].Name, layout.UJoints[3].JointAngle));
                sb.AppendLine("");

                sb.AppendLine(string.Format("n = Number of U-joints = 4"));
                sb.AppendLine("");

                DisplayInertiaDriveAngle(sb, layout);
                sb.AppendLine("");

                DisplayInertiaCoastAngle(sb, layout);
                sb.AppendLine("");

                DisplayTorsionalAngle(sb, layout);
                sb.AppendLine("");

                DisplayVariables(sb);
                sb.AppendLine();

                DisplayInertialDrive(sb, layout);
                sb.AppendLine();

                DisplayInertialCoast(sb, layout);
                sb.AppendLine("");

                DisplayTorsional(sb, layout);
                sb.AppendLine("");

                sb.AppendLine("");
                sb.AppendLine("");

                CalibrationResult = sb.ToString();
            }
            catch (Exception)
            {                
                throw;
            }
        }

       
       
        
        private DriveshaftLayout ImaginaryLayout()
        {
            try
            {
                DriveshaftLayout layout = new DriveshaftLayout(3, 2500);
                
                //make the first one crossed.
                layout.Driveshafts[0].IsCrossed = true;

                //set u-joint angles.
                layout.UJoints[0].JointAngle = 5;
                layout.UJoints[1].JointAngle = 4;
                layout.UJoints[2].JointAngle = 3;
                layout.UJoints[3].JointAngle = 2;
                
                return layout;
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void DisplayInertiaDriveAngle(StringBuilder sb, DriveshaftLayout layout)
        {
            try
            {
                sb.AppendLine("Inertia Drive Angle =\t√((n-1)(Θ1)² ± (n-2)(Θ2)² ± (n-3)(Θ3)²)");
                sb.AppendLine(string.Format("\t\t\t=√(({0})(3)({1:n2})² + ({2})(2)({3:n2})² + ({4})(1)({5:n2})²)",
                                                    layout.DriveModifiers[0],
                                                    layout.UJoints[0].JointAngle,
                                                    layout.DriveModifiers[1],
                                                    layout.UJoints[1].JointAngle,
                                                    layout.DriveModifiers[2],
                                                    layout.UJoints[2].JointAngle));
                sb.AppendLine(string.Format("\t\t\t={0:n2}°", layout.InertiaDriveAngle));
                sb.AppendLine("\t\t\tExpected value is 9.90°, verify with calculator if desired.");
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void DisplayInertiaCoastAngle(StringBuilder sb, DriveshaftLayout layout)
        {
            try
            {
                sb.AppendLine("Inertia Coast Angle =\t√((n-1)(Θ4)² ± (n-2)(Θ3)² ± (n-3)(Θ2)²)");
                sb.AppendLine(string.Format("\t\t\t=√(({0})(3)({1:n2})² + ({2})(2)({3:n2})² + ({4})(1)({5:n2})²)",
                                                    layout.CoastModifiers[0],
                                                    layout.UJoints[3].JointAngle,
                                                    layout.CoastModifiers[1],
                                                    layout.UJoints[2].JointAngle,
                                                    layout.CoastModifiers[2],
                                                    layout.UJoints[1].JointAngle));
                sb.AppendLine(string.Format("\t\t\t={0:n2}°", layout.InertiaCoastAngle));
                sb.AppendLine("\t\t\tExpected value is 3.16°, verify with calculator if desired.");
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void DisplayTorsionalAngle(StringBuilder sb, DriveshaftLayout layout)
        {
            try
            {
                sb.AppendLine("Torsional Angle =\t\t√((Θ1)² ± (Θ2)² ± (Θ3)² ± (Θ4)²)");
                sb.AppendLine(string.Format("\t\t\t=√(({0})({1:n2})² + ({2})({3:n2})² + ({4})({5:n2})² + ({6})({7:n2})²)",
                                                    layout.DriveModifiers[0],
                                                    layout.UJoints[0].JointAngle,
                                                    layout.DriveModifiers[1],
                                                    layout.UJoints[1].JointAngle,
                                                    layout.DriveModifiers[2],
                                                    layout.UJoints[2].JointAngle,
                                                    layout.DriveModifiers[3],
                                                    layout.UJoints[3].JointAngle));

                sb.AppendLine(string.Format("\t\t\t={0:n2}°", layout.TorsionalAngle));
                sb.AppendLine("\t\t\tExpected value is 6.00°, verify with calculator if desired.");
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void DisplayVariables(StringBuilder sb)
        {
            try
            {
                sb.AppendLine("Θid = inertial drive angle");
                sb.AppendLine("Θic = inertial coast angle");
                sb.AppendLine("Θt = torsional angle");
                sb.AppendLine("ω = angular velocity in rad/sec");
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void DisplayInertialDrive(StringBuilder sb, DriveshaftLayout layout)
        {
            try
            {
                sb.AppendLine("Inertia Drive = \t\tω² * sin²Θid");
                sb.AppendLine(string.Format("\t\t\t= {0:n2}² * sin²({1:n2}°)", layout.AngularVelocity, layout.InertiaDriveAngle));
                sb.AppendLine(string.Format("\t\t\t= {0:n0} rad/sec²", layout.InertialDrive));
                sb.AppendLine("\t\t\tExpected value is 2,026 rad/sec², verify with calculator if desired.");

            }
            catch (Exception)
            {                
                throw;
            }
        }
        
        private void DisplayInertialCoast(StringBuilder sb, DriveshaftLayout layout)
        {
            try
            {
                sb.AppendLine("Inertia Coast = \t\tω² * sin²Θic");
                sb.AppendLine(string.Format("\t\t\t= {0:n2}² * sin²({1:n2}°)", layout.AngularVelocity, layout.InertiaCoastAngle));
                sb.AppendLine(string.Format("\t\t\t= {0:n0} rad/sec²", layout.InertialCoast));
                sb.AppendLine("\t\t\tExpected value is 209 rad/sec², verify with calculator if desired.");
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void DisplayTorsional(StringBuilder sb, DriveshaftLayout layout)
        {
            try
            {
                sb.AppendLine("Torsional = \t\tω² * sin²Θt");
                sb.AppendLine(string.Format("\t\t\t= {0:n2}² * sin²({1:n2}°)", layout.AngularVelocity, layout.TorsionalAngle));
                sb.AppendLine(string.Format("\t\t\t= {0:n0} rad/sec²", layout.Torsional));
                sb.AppendLine("\t\t\tExpected value is 749 rad/sec², verify with calculator if desired.");
            }
            catch (Exception)
            {                
                throw;
            }
        }


    }
}
