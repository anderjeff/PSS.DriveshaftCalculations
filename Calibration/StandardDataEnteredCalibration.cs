using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PSS.DriveshaftCalculations.Calculations;

using PSS.DriveshaftCalculations.DriveshaftParts;


namespace PSS.DriveshaftCalculations.Calibration
{
    public class StandardDataEnteredCalibration
    {
        public StandardDataEnteredCalibration(List<DriveshaftSeries> selectedSeries)
        {
            try
            {
                SetHeaderText();
                GetStandardData(selectedSeries);
            }
            catch (Exception)
            {                
                throw;
            }
        }

      
        /// <summary>
        /// A string showing the result of the calibration.
        /// </summary>
        public string CalibrationResult { get; set; }

        private void SetHeaderText()
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("***VERIFICATION OF SAVED SERIES DATA***");
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

        private void GetStandardData(List<DriveshaftSeries> selectedSeries)
        {
            try
            {
                //there may be a lot of info, so use string builder.
                StringBuilder sb = new StringBuilder();

                //break up into round, wing and metric.
                List<DriveshaftSeries> roundBearing = (from rb in selectedSeries
                                                       where rb.SeriesType == DriveshaftSeries.TypeOfSeries.RoundBearing
                                                       orderby rb.MinElasticLimit
                                                       select rb).ToList<DriveshaftSeries>();

                List<DriveshaftSeries> wingBearing = (from wb in selectedSeries
                                                      where wb.SeriesType == DriveshaftSeries.TypeOfSeries.WingBearing
                                                      orderby wb.MinElasticLimit
                                                      select wb).ToList<DriveshaftSeries>();

                List<DriveshaftSeries> metricBearing = (from ms in selectedSeries
                                                       where ms.SeriesType == DriveshaftSeries.TypeOfSeries.Metric
                                                       orderby ms.MinElasticLimit
                                                       select ms).ToList<DriveshaftSeries>();

                if (roundBearing.Count > 0)                
                {
                    sb.AppendLine("--ROUND BEARING SERIES INFORMATION--");
                    sb.AppendLine("Located at: G:\\Engineering\\Design\\Standards\\IJ900-02.pdf");
                    sb.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                                                "Series",
                                                "T-Ind",
                                                "T-MOH",
                                                "Max HP",
                                                "T-d",
                                                "MEL"));

                    foreach (DriveshaftSeries roundSeries in roundBearing)
                    {
                        //cast
                        EnglishDriveshaftSeries rs = (EnglishDriveshaftSeries)roundSeries;                 
                        
                        sb.AppendLine(string.Format("{0}\t{1:n0}\t{2:n0}\t{3:n0}\t{4:n0}\t{5:n0}",
                                                    rs.Name,
                                                    rs.IndustrialRating,
                                                    rs.MOH_Rating,
                                                    rs.MaxNetHorsepower,
                                                    rs.BearingCapacity,
                                                    rs.MinElasticLimit));
                    }

                    //add a blank line.
                    sb.AppendLine();
                }

                if (wingBearing.Count > 0)
                {
                    sb.AppendLine("--WING BEARING SERIES INFORMATION--");
                    sb.AppendLine("Located at: G:\\Engineering\\Design\\Standards\\IJ900-02.pdf");
                    sb.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                                                "Series",
                                                "T-Ind",
                                                "T-MOH",
                                                "Max HP",
                                                "T-d",
                                                "MEL"));

                    foreach (DriveshaftSeries wingSeries in wingBearing)
                    {
                        //cast
                        EnglishDriveshaftSeries ws = (EnglishDriveshaftSeries)wingSeries;

                        sb.AppendLine(string.Format("{0}\t{1:n0}\t{2:n0}\t{3:n0}\t{4:n0}\t{5:n0}",
                                                    ws.Name,
                                                    ws.IndustrialRating,
                                                    ws.MOH_Rating,
                                                    ws.MaxNetHorsepower,
                                                    ws.BearingCapacity,
                                                    ws.MinElasticLimit));
                    }

                    //add a blank line.
                    sb.AppendLine();
                }
                
                if (metricBearing.Count > 0)
                {
                    sb.AppendLine("--METRIC BEARING SERIES INFORMATION--");
                    sb.AppendLine("Located at: G:\\Engineering\\Design\\Standards\\GWB_Ind_Katalog_de_3_04[1].pdf");
                    sb.AppendLine(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}",
                                                "Series",
                                                "T-Cs",
                                                "Lc",
                                                "T-DSch",
                                                "T-Dw",
                                                "MEL"));

                    foreach (DriveshaftSeries metricSeries in metricBearing)
                    {
                        //cast
                        MetricDriveshaftSeries ms = (MetricDriveshaftSeries)metricSeries;
                        double minElasticConverted = TorqueUnitConverter.ConvertToFootPounds(TorqueUnitConverter.StartingUnits.kNm, 
                            ms.MinElasticLimit );

                        sb.AppendLine(string.Format("{0}\t{1:n1}\t{2:#.######}\t{3:n1}\t{4:n1}\t{5:n1} ({6:n0} ft-lbs)",
                                                    ms.Name,
                                                    ms.FunctionalLimitTorque,
                                                    ms.BearingCapacityFactor,
                                                    ms.PulsatingFatigueTorque,
                                                    ms.ReversingFatigueTorque,
                                                    ms.MinElasticLimit,
                                                    minElasticConverted));
                    }

                    //add a blank line.
                    sb.AppendLine();
                }

                AddDefinitions(sb);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        //Add definitions of the various terms used.
        private void AddDefinitions(StringBuilder sb)
        {
            try
            {
                sb.AppendLine("--DEFINITIONS--");
                sb.AppendLine("\t1. Series - The driveshaft series name.");
                sb.AppendLine("\t2. T-Ind (Industrial Rating): The ISO rating for the universal joint bearing.");
                sb.AppendLine("\t     Equates to the load that can be applied to the bearing that will result in a B10 life ");
                sb.AppendLine("\t     of 1 million revolutions.");
                sb.AppendLine("\t3. T-MOH (MOH Rating): The maximum driveshaft torque that will allow infinite ");
                sb.AppendLine("\t     fatigue life of all driveshaft components. Usually equated to a driveshaft ");
                sb.AppendLine("\t     stall torque in equipment that produces low speed, high unidirectional loading.");
                sb.AppendLine("\t4. Max HP (Maximum Net Driveshaft Power): The power that can be transmitted by the ");
                sb.AppendLine("\t     driveshaft and achieve 5,000 hours of B-10 life with 3° angle. Can be used to size" );
                sb.AppendLine("\t     on/off-highway vehicle driveshaft, but with caution.  Extreme low gear rations can ");
                sb.AppendLine("\t     result in torques that will exceed the driveshaft yield strength.");
                sb.AppendLine("\t5. T-d (Bearing Capacity): The driveshaft torque that will acheive 5,000 hours of B-10 life ");
                sb.AppendLine("\t     at 100 rpm and 3°. This is the value used in the bearing life equation.");
                sb.AppendLine("\t6. MEL (Minimum Elastic Limit): The maximum torque the series can withstand without permenant ");
                sb.AppendLine("\t     deformation.");
                sb.AppendLine("\t7. T-Cs (Functional Limit Torque): Up to this maximum permissible torque a load may be applied ");
                sb.AppendLine("\t     to a cardan shaft for a limited frequency without the working capability being affected by ");
                sb.AppendLine("\t     permanent deformation of any cardan shaft functional area.  Up to 1,000 load changes (short ");
                sb.AppendLine("\t     time fatigue strength for finite life) are capable of being sustained with T-Cs. This does not ");
                sb.AppendLine("\t     result in any unpermissible effect on bearing life.");
                sb.AppendLine("\t8. Lc (Bearing Capacity Factor): The bearing capacity factor takes into consideration the dynamic ");
                sb.AppendLine("\t     service life C-dyn (see DIN/ISO 281) of the bearings and the joint geometry R.");
                sb.AppendLine("\t9. T-DSch (Pulsating Fatigue Torque): At this torque, the cardan shaft is permanently solid at ");
                sb.AppendLine("\t     pulsating loads. 1.4 * reversing fatigue torque.");
                sb.AppendLine("\t10. T-Dw (Reversing Fatigue Torque): At this torque, the cardan shaft is permanently solid at alternating ");
                sb.AppendLine("\t      loads. Check capacity of flange connection if this value reached.");

                CalibrationResult += sb.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
