using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PSS.DriveshaftCalculations.DriveshaftParts;
using PSS.DriveshaftCalculations.DutyCycle;
using System.Collections;
using PSS.Common;

namespace PSS.DriveshaftCalculations.Calculations
{
    /// <summary>
    /// B10 Life is the hours of live that 90% of the universal joint bearings will
    /// achieve successfully.
    /// </summary>
    [Serializable]
    public class B10Life : IComparable, IPersistXml
    {
        public B10Life(OperatingCondition condition, DriveshaftSeries series)
        {
            try
            {
                _condition = condition;
                _objSeries = series;

                GetCalculatedHours(series);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Constructor used to find the B10 life for a duty cycle.
        /// </summary>
        /// <param name="dtyColl"></param>
        /// <param name="series"></param>
        public B10Life(DutyCycleConditionCollection dtyColl, DriveshaftSeries series)
        {
            try
            {
                Dictionary<DutyCycleCondition, double> hoursByCondition;
                hoursByCondition = new Dictionary<DutyCycleCondition, double>();

                foreach (DutyCycleCondition dCond in dtyColl)
                {
                    //set the global variables for use by the upcoming methods.
                    _condition = dCond.OperatingCondition;
                    _objSeries = series;

                    //sets the Hours property for this B10 life object.
                    GetCalculatedHours(series);

                    //add the B10 life for this particular condition.
                    hoursByCondition.Add(dCond, Hours);
                }

                //now take the values saved to the list and get the total calculated hours.
                ProcessDutyCycle(hoursByCondition);
            }
            catch (Exception)
            {
                throw;
            }
        }

        //let's not go overboard, this is 114 years.
        const double MAX_ALLOWABLE_LIFE = 1000000;

        //The operating condition to check.
        private OperatingCondition _condition;

        private DriveshaftSeries _objSeries;
        /// <summary>
        /// The driveshaft series we are getting the life for.
        /// </summary>
        [Browsable(false)]
        public DriveshaftSeries ObjSeries
        {
            get { return _objSeries; }
            set { _objSeries = value; }
        }

        /// <summary>
        /// The series name.
        /// </summary>
        public string Series
        {
            get
            {
                if (_objSeries != null)
                {
                    return _objSeries.Name;
                }
                else
                {
                    return null;
                }
            }
        }

        //B10 life in hours
        public double Hours { get; private set; }

        private void GetCalculatedHours(DriveshaftSeries series)
        {
            try
            {
                if (series.SeriesType == DriveshaftSeries.TypeOfSeries.Metric)
                {
                    Hours = CalculatedHours((MetricDriveshaftSeries)series);
                }
                else
                {
                    Hours = CalculatedHours((EnglishDriveshaftSeries)series);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        ///// <summary>
        ///// Calculated hours for English Driveshaft Series, this is the old way.
        ///// </summary>
        ///// <param name="series"></param>
        ///// <returns></returns>
        //private double CalculatedHours(EnglishDriveshaftSeries series)
        //{
        //    try
        //    {
        //        //max value because the denominator would be zero.                
        //        double hours = double.MaxValue;

        //        //shorten the variable names so the equation isn't a mile long.
        //        double tc = series.ContinuousTorque;
        //        double rpm = _condition.Rpm;
        //        double rpmCont = series.RpmStandard;
        //        double t = _condition.Torque;
        //        double ka = _condition.AngleFactor;
        //        double kp = _condition.PowerFactor;
        //        double ks = _condition.ServiceFactor;
        //        int hs = series.B10Standard;

        //        //Avoid a divide by zero.
        //        if (t > 0 && ka > 0 && kp > 0 && ks > 0)
        //        {
        //            hours = hs * Math.Pow((tc / ((Math.Pow((rpm / rpmCont), 0.3)) * (t * ka * kp * ks))), 3.33333333333);
        //        }

        //        return hours;
        //    }
        //    catch (Exception)
        //    {                
        //        throw;
        //    }
        //}

        /// <summary>
        /// Calculated hours for English Driveshaft Series, this is the new way.
        /// </summary>
        /// <param name="series"></param>
        /// <returns></returns>
        /// 
        private double CalculatedHours(EnglishDriveshaftSeries series)
        {
            try
            {
                //max value because the denominator would be zero.                
                double hours = double.MaxValue;

                //shorten the variable names so the equation isn't a mile long.               
                double td = series.BearingCapacity;
                double t = _condition.Torque;
                int speed = _condition.Rpm;
                double angle = _condition.Angle;

                //avoid the divide by zero.
                if (speed > 0 && angle > 0 && t > 0)
                {
                    double angleCorrected = angle;
                    if (angle >= 0 && angle < 3)
                    {
                        angleCorrected = 3;
                    }

                    double torqueRatio = td / t;
                    //note 3.33333333 is the 10/3 power.
                    hours = (1500000 / (speed * angleCorrected)) * Math.Pow(torqueRatio, 3.33333333);
                }

                if (hours > MAX_ALLOWABLE_LIFE)
                {
                    return MAX_ALLOWABLE_LIFE;
                }
                else
                {
                    return hours;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Calculated hours for metric series, from GWB catalog.
        /// </summary>
        /// <param name="series"></param>
        /// <returns></returns>
        private double CalculatedHours(MetricDriveshaftSeries series)
        {
            try
            {
                //max value because the denominator would be zero.
                double hours = double.MaxValue;

                double Lc = series.BearingCapacityFactor;
                double rpm = _condition.Rpm;
                double ang = _condition.Angle;

                //convert torque to kNm for the formula to work properly.
                double t = _condition.Torque * .0013558179;

                double pf = _condition.PowerFactor;

                if (pf > 0 && t > 0 && ang > 0 && rpm > 0)
                {
                    double num = Lc * Math.Pow(10, 10);
                    double denom = rpm * ang * Math.Pow(t, (10 / 3)) * pf;

                    //the 3.333333333 comes from 10/3;
                    hours = (Lc * Math.Pow(10, 10)) / (rpm * ang * Math.Pow(t, (3.33333333333)) * pf);
                }

                if (hours > MAX_ALLOWABLE_LIFE)
                {
                    return MAX_ALLOWABLE_LIFE;
                }
                else
                {
                    return hours;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ProcessDutyCycle(Dictionary<DutyCycleCondition, double> hoursByCondition)
        {
            //the denominator portion of the TotalLifeExpectancy equation.
            //See page 31 of SAE-AE7 Driveshaft Design Manual.
            double hrsDenom = 0;

            foreach (var condition in hoursByCondition)
            {
                //the condition.
                DutyCycleCondition dConn = (DutyCycleCondition)condition.Key;

                //the hours for this condition.
                double b10 = (double)condition.Value;

                //bearing life of 1 is meaningless anyway, because it would
                //never be designed to 1 hour.  So to avoid a divide by zero
                //exception, make it 1.
                if (b10 < 1)
                {
                    b10 = 1;
                }

                //the percent of time for this condition.
                double pctOfTime = dConn.PercentOfTime;

                //add to denominator sum.
                hrsDenom += (pctOfTime / b10);
            }

            if (hrsDenom == 0)
            {
                Hours = MAX_ALLOWABLE_LIFE;
            }
            else
            {
                if (1 / hrsDenom > MAX_ALLOWABLE_LIFE)
                {
                    Hours = MAX_ALLOWABLE_LIFE;
                }
                else
                {
                    Hours = 1 / hrsDenom;
                }
            }
        }

        public int CompareTo(object obj)
        {
            B10Life temp = obj as B10Life;
            if (temp != null)
            {
                return this.Hours.CompareTo(temp.Hours);
            }
            else
            {
                throw new ArgumentException("Parameter is not a B10Life object.");
            }
        }

        #region IPersistXmlMethods

        public void Load(System.Xml.XmlElement ele)
        {
            Hours = XML.Open_dbl(ele, "Hours", 0.0);
            ObjSeries = (DriveshaftSeries)XML.Open_IPersistXml(ele, "ObjSeries", new DriveshaftSeries());          
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "Hours", Hours);
            XML.Save(ele, "ObjSeries", ObjSeries);
        } 
        
        #endregion
    }
}
