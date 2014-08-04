using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PSS.DriveshaftCalculations.Calculations;
using PSS.DriveshaftCalculations.Collections;
using PSS.DriveshaftCalculations.DriveshaftParts;

namespace PSS.DriveshaftCalculations.Calibration
{
    public class BearingLifeCalibration : ICalibration
    {
        public BearingLifeCalibration(DriveshaftSeriesCollection drColl)
        {
            try
            {
                SetHeaderText();

                foreach (DriveshaftSeries series in drColl)
                {
                    PerformCalibration(series);                
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public string CalibrationResult { get; set; }

        private void SetHeaderText()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("***VERIFICATION OF BEARING LIFE CALCULATIONS***");
                sb.AppendLine();
                sb.AppendLine(string.Format("Performed: {0:M/d/yyyy h:mm:ss tt}", DateTime.Now));
                sb.AppendLine();

                CalibrationResult = sb.ToString();
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void PerformCalibration(DriveshaftSeries series)
        {
            try
            {
                OperatingCondition standardCondition = new OperatingCondition();
                standardCondition.SetStandardCondition(series);

                B10Life life = new B10Life(standardCondition, series);

                if (series.SeriesType != DriveshaftSeries.TypeOfSeries.Metric)
                {
                    EnglishDriveshaftSeries engSeries = (EnglishDriveshaftSeries)series;

                    if (life.Hours == 5000)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("");
                        sb.AppendLine(string.Format("{0} series passed calibration with the following parameters:", engSeries.Name));
                        sb.AppendLine(string.Format("\tTorque: {0:n0} lb-ft", standardCondition.Torque));
                        sb.AppendLine(string.Format("\tSpeed: {0:n0} rpm", standardCondition.Rpm));
                        sb.AppendLine(string.Format("\tAngle: {0}°", standardCondition.Angle));
                        sb.AppendLine(string.Format("\tAngle Factor: {0:n1}, Power Factor: {1:n1}, Service Factor: {2:n1}",
                                                        standardCondition.AngleFactor,
                                                        standardCondition.PowerFactor,
                                                        standardCondition.ServiceFactor));
                        sb.AppendLine("");
                        sb.AppendLine(string.Format("\tExpected Life: {0:n0} hours", 5000));
                        sb.AppendLine(string.Format("\tCalculated Life: {0:n0} hours", life.Hours));

                        sb.AppendLine("----------");
                        sb.AppendLine("----------");

                        CalibrationResult += sb.ToString();
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("");
                        sb.AppendLine(string.Format("{0} SERIES CALIBRATION FAILED, CHECK B-10 LIFE FORMULA FOR ACCURACY:", engSeries.Name));
                        sb.AppendLine(string.Format("\tTorque: {0:n0} lb-ft", standardCondition.Torque));
                        sb.AppendLine(string.Format("\tSpeed: {0:n0} rpm", standardCondition.Rpm));
                        sb.AppendLine(string.Format("\tAngle: {0}", standardCondition.Angle));
                        sb.AppendLine(string.Format("\tAngle Factor: {0:n1}, Power Factor: {1:n1}, Service Factor: {2:n1}",
                                                        standardCondition.AngleFactor,
                                                        standardCondition.PowerFactor,
                                                        standardCondition.ServiceFactor));
                        sb.AppendLine("");
                        sb.AppendLine(string.Format("\tExpected Life: {0:n0} hours", 5000));
                        sb.AppendLine(string.Format("\tCalculated Life: {0:n0} hours", life.Hours));

                        sb.AppendLine("----------");
                        sb.AppendLine("----------");

                        CalibrationResult += sb.ToString();
                    }
                }
                else 
                {
                    //metric series validation
                }
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
