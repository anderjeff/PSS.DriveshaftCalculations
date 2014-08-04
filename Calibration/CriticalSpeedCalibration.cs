using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PSS.DriveshaftCalculations.DriveshaftParts;
using PSS.DriveshaftCalculations.Calculations;


namespace PSS.DriveshaftCalculations.Calibration
{
    public class CriticalSpeedCalibration : ICalibration
    {
        public CriticalSpeedCalibration()
        {
            //create a tube to use in the critical speed verification.
            Tube selTube = new Tube(3.50, .120, 60.00);
            //create a critical speed object.
            _cs = new CriticalSpeed(selTube, 3000);

            PrepareString();
        }

        private CriticalSpeed _cs;

        string Formula
        {
            get
            {
                string formula = string.Format("{0:n0} * √(od² + id²) / length²", _cs.critSpdConst);
                return formula;
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

        private void PrepareString()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("***VERIFICATION OF CRITICAL SPEED CALCULATION***");
                sb.AppendLine();
                sb.AppendLine(string.Format("Performed: {0:M/d/yyyy h:mm:ss tt}", DateTime.Now));

                sb.AppendLine();

                sb.AppendLine(string.Format("PARAMETERS:"));   
                sb.AppendLine(string.Format("Tube OD:\t\t{0:n3}", _cs.Tube.OuterDia));
                sb.AppendLine(string.Format("Tube ID:\t\t{0:n3}", _cs.Tube.InnerDia));
                sb.AppendLine(string.Format("Length:\t\t{0:n3}", _cs.Tube.TubeLength));
                
                //insert a blank line.
                sb.AppendLine();

                sb.AppendLine(string.Format("Critical Speed\t= {0}", Formula));
                sb.AppendLine(string.Format("\t\t= {0} * √({1:n3}² + {2:n3}²) / {3:n3}²", 
                                            _cs.critSpdConst, 
                                            _cs.Tube.OuterDia, 
                                            _cs.Tube.InnerDia, 
                                            _cs.Tube.TubeLength));
                sb.AppendLine(string.Format("\t\t= {0:n0} rpm", _cs.Value));

                sb.AppendLine();
                sb.AppendLine("The value should equal 6,251 rpm, verify with calculator if desired.");
                sb.AppendLine();
                CalibrationResult = sb.ToString();
            }
            catch (Exception)
            {                
                throw;
            }
        }



        
    }
}
