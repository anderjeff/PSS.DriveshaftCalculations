using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PSS.Common;

namespace PSS.DriveshaftCalculations.DriveshaftParts
{
    [Serializable]
    public class DriveshaftSeries : IPersistXml
    {
        public DriveshaftSeries()
        {
            Warnings = new List<SafetyWarning>();
        }

        //standardized values.
        public string Name { get; set; }

        /// <summary>
        /// The maximum torque the series can handle without permenant deformation.
        /// </summary>
        public double MinElasticLimit { get; set; }

        /// <summary>
        /// The fastest this series is allowed to rotate.
        /// </summary>
        public int MaxAllowableSpeed { get; set; }

        /// <summary>
        /// The type of u-joint this series falls into, wing, round etc.
        /// </summary>
        public TypeOfSeries SeriesType { get; set; }

        /// <summary>
        /// Indicates that the driveshaft series is a safe option.
        /// </summary>
        public bool IsSafeOption { get; set; }

        /// <summary>
        /// A comment about the use of this series for a particular application.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// A warninig level system for the series.  Used to prioritize warnings in
        /// the case of a duty cycle where there are multiple warnings.
        /// </summary>
        public List<SafetyWarning> Warnings { get; set; }

        public string WarningMessage
        {
            get
            {
                if (Warnings.Count > 0)
                {
                    string msg = CustomWarningMessage(HighestPriorityMessage());
                    return msg;
                }
                else
                    return CustomWarningMessage(SafetyWarning.NoWarning);
            }
        }

        protected virtual string CustomWarningMessage(SafetyWarning safetyWarning)
        {
            return "";
        }

        private SafetyWarning HighestPriorityMessage()
        {
            //check for Warnings.Count > 0 before calling this.
            return (from warning in this.Warnings
                    select warning).Min();
        }

        /// <summary>
        /// The general type of u-joint series.
        /// </summary>   
        public enum TypeOfSeries
        {
            RoundBearing, //the old SAE formulas for bearing life from driveshaft program. 
            WingBearing, //the old SAE formulas for bearing life from driveshaft program.
            Metric, //GWB series
            PSL, //mimicks the SPL series
            DanaBearing //used because their equations are different.
        }

        //warnings put in order of precedence.
        public enum SafetyWarning
        {
            MelExceeded,
            FunctionalTorqueLimitExceeded,
            MaxPowerExceeded,
            ShockExceedsPulsatingFatigueTorque,
            ShockExceedsTorsionalRating,
            ShockExceedsReversingFatigueTorque,
            ShockExceedsMomentaryRating,
            MaxSeriesRpmExceeded,
            HoursTooLow,
            NoWarning
        }


        public virtual void Load(System.Xml.XmlElement ele)
        {
            Comment = (string)XML.Open_str(ele, "Comment", string.Empty);
            IsSafeOption = (bool)XML.Open_bln(ele, "IsSafeOption", false);
            MaxAllowableSpeed = (int)XML.Open_int(ele, "MaxAllowableSpeed", 0);
            MinElasticLimit = (double)XML.Open_dbl(ele, "MinElasticLimit", 0.0);
            Name = (string)XML.Open_str(ele, "Name", string.Empty);
            SeriesType = (TypeOfSeries)XML.Open_Enum<TypeOfSeries>(ele, "SeriesType", TypeOfSeries.RoundBearing);
            
            Warnings = new List<SafetyWarning>();
            List<int> temp = XML.Open_List_int(ele, "Warnings", new List<int>());
            foreach (int i in temp)
            {
                Warnings.Add((SafetyWarning)i);
            }
        }

        public virtual void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "Name", Name);
            XML.Save(ele, "Comment", Comment);
            XML.Save(ele, "IsSafeOption", IsSafeOption);
            XML.Save(ele, "MaxAllowableSpeed", MaxAllowableSpeed);
            XML.Save(ele, "MinElasticLimit", MinElasticLimit);

            List<int> tmp = new List<int>();
            foreach(SafetyWarning warning in Warnings)
            {
                tmp.Add((int)warning);
            }

            XML.Save(ele, "Warnings", tmp);
        }
    }
}
