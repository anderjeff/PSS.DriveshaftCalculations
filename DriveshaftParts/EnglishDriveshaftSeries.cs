using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PSS.Common;


namespace PSS.DriveshaftCalculations.DriveshaftParts
{
    /// <summary>
    /// Represents a series that is calculated in a standard English way, not 
    /// like a metric series.
    /// </summary>
    [Serializable]
    public class EnglishDriveshaftSeries : DriveshaftSeries, IPersistXml
    {
        public EnglishDriveshaftSeries()
        {
        }

        public EnglishDriveshaftSeries(string name, double industrialRating, double moh_rating,
                                       double maxNetHp, double bearingCapacity, double minElasticLimit,
                                       TypeOfSeries seriesType, int maxAllowSpeed)
        {
            Name = name;
            IndustrialRating = industrialRating;
            MOH_Rating = moh_rating;
            MaxNetHorsepower = maxNetHp;
            BearingCapacity = bearingCapacity;
            MinElasticLimit = minElasticLimit;
            SeriesType = seriesType;
            MaxAllowableSpeed = maxAllowSpeed;
        }

        /// <summary>
        /// The ISO rating form the universal joint bearing.  Equates to the load that
        /// can be applied to the bearing that will result in a B10 life of 1 million
        /// revolutions.  The bearing capacity is used in the bearing life calculation. 
        /// </summary>
        public double BearingCapacity { get; set; }

        /// <summary>
        /// The power that can be transmitted by the driveshaft and achieve 5000 hours of B10 
        /// life with 3° of universal joint angularity.  Can be used to size on/off-highway 
        /// vehicle driveshaft, but with caution.  Driveshaft yield strength still must not
        /// be exceeded. 
        /// </summary>
        public double MaxNetHorsepower { get; set; }

        /// <summary>
        /// The maximum driveshaft torque that will allow infinite fatigue life of all driveshaft
        /// components.  Usually equated to driveshaft stall torque in equipment that produces 
        /// low speed, high unidirectional loading.
        /// </summary>
        public double MOH_Rating { get; set; }

        /// <summary>
        /// The driveshaft torque that will achieve 5000 hours of B10 life at 100 RPM and 3°.
        /// </summary>
        public double IndustrialRating { get; set; }


        public double EnduranceTorque { get; set; }

        protected override string CustomWarningMessage(SafetyWarning safetyWarning)
        {
            switch (safetyWarning)
            {
                case (SafetyWarning.MelExceeded):
                    return "Warning-MEL Exceeded";
                case (SafetyWarning.MaxPowerExceeded):
                    return "Warning-Max Power Exceeded";
                case (SafetyWarning.ShockExceedsTorsionalRating):
                    return "Warning-Torsional Rating Exceeded";
                case (SafetyWarning.ShockExceedsMomentaryRating):
                    return "Warning-Momentary Rating Exceeded";
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

            BearingCapacity = (double)XML.Open_dbl(ele, "BearingCapacity", 0.0);
            EnduranceTorque = (double)XML.Open_dbl(ele, "EnduranceTorque", 0.0);
            IndustrialRating = (double)XML.Open_dbl(ele, "IndustrialRating", 0.0);
            MaxNetHorsepower = (double)XML.Open_dbl(ele, "MaxNetHorsepower", 0.0);
            MOH_Rating = (double)XML.Open_dbl(ele, "MaxNetHorsepower", 0.0);
        }

        public override void Save(System.Xml.XmlElement ele)
        {
            base.Save(ele);

            XML.Save(ele, "BearingCapacity", BearingCapacity);
            XML.Save(ele, "EnduranceTorque", EnduranceTorque);
            XML.Save(ele, "IndustrialRating", IndustrialRating);
            XML.Save(ele, "MaxNetHorsepower", MaxNetHorsepower);
            XML.Save(ele, "MOH_Rating", MOH_Rating);
        }



    }
}
