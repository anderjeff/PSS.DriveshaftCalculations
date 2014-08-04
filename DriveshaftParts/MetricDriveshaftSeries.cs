using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PSS.Common;


namespace PSS.DriveshaftCalculations.DriveshaftParts
{
    /// <summary>
    /// Represents a series that is calculated using metric standards.
    /// </summary>
    [Serializable]
    public class MetricDriveshaftSeries : DriveshaftSeries, IPersistXml
    {
        public MetricDriveshaftSeries()
        {
        }

        public MetricDriveshaftSeries(string name, double revrsFatgTorq, double funcLimTorq,
                                      double mel, double brgCapFact, TypeOfSeries seriesType,
                                      int maxRpm)
        {
            Name = name;
            ReversingFatigueTorque = revrsFatgTorq;

            //per formula in GWB catalog.
            PulsatingFatigueTorque = 1.4 * ReversingFatigueTorque;

            FunctionalLimitTorque = funcLimTorq;
            MinElasticLimit = mel;
            BearingCapacityFactor = brgCapFact;
            SeriesType = seriesType;
            MaxAllowableSpeed = maxRpm;
        }
        /// <summary>
        /// The torque the driveshaft can withstand permenantly with reversing loads.
        /// </summary>
        public double ReversingFatigueTorque { get; set; }

        /// <summary>
        /// The torque the driveshaft can withstand permenantly with pulsating loads.
        /// </summary>
        public double PulsatingFatigueTorque { get; set; }

        /// <summary>
        /// Max short duration torque.
        /// </summary>
        public double FunctionalLimitTorque { get; set; }

        //relative capacity of the series.
        public double BearingCapacityFactor { get; set; }

        protected override string CustomWarningMessage(DriveshaftSeries.SafetyWarning safetyWarning)
        {
            switch (safetyWarning)
            {
                case (SafetyWarning.MelExceeded):
                    return "Warning-MEL Exceeded";
                case (SafetyWarning.FunctionalTorqueLimitExceeded):
                    return "Warning-Functional Limit Exceeded";
                case (SafetyWarning.ShockExceedsPulsatingFatigueTorque):
                    return "Warning-Pulsating Fatigue Torque Exceeded";
                case (SafetyWarning.ShockExceedsReversingFatigueTorque):
                    return "Warning-Reversing Fatigue Torque Exceeded";
                case (SafetyWarning.MaxSeriesRpmExceeded):
                    return "Warning-Max RPM Exceeded";
                case (SafetyWarning.HoursTooLow):
                    return "Life Lower Than Desired";
                default:
                    return "Series OK";
            }
        }

        public override void Load(System.Xml.XmlElement ele)
        {
            base.Load(ele);

            BearingCapacityFactor = (double)XML.Open_dbl(ele, "BearingCapacityFactor", 0.0);
            FunctionalLimitTorque = (double)XML.Open_dbl(ele, "FunctionalLimitTorque", 0.0);
            PulsatingFatigueTorque = (double)XML.Open_dbl(ele, "PulsatingFatigueTorque", 0.0);
            ReversingFatigueTorque = (double)XML.Open_dbl(ele, "ReversingFatigueTorque", 0.0);
        }

        public override void Save(System.Xml.XmlElement ele)
        {
            base.Save(ele);

            XML.Open_dbl(ele, "BearingCapacityFactor", BearingCapacityFactor);
            XML.Open_dbl(ele, "FunctionalLimitTorque", FunctionalLimitTorque);
            XML.Open_dbl(ele, "PulsatingFatigueTorque", PulsatingFatigueTorque);
            XML.Open_dbl(ele, "ReversingFatigueTorque", ReversingFatigueTorque);
        }

    }
}
