using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PSS.DriveshaftCalculations.Calculations;
using System.ComponentModel;
using PSS.Common;


namespace PSS.DriveshaftCalculations.DriveshaftParts
{
    /// <summary>
    /// Defines a set of conditions that a particular driveshaft series 
    /// is subjected to, and for which critical speed
    /// needs to be calculated.
    /// </summary>
    [Serializable]
    public class OperatingCondition : ICloneable, INotifyPropertyChanged, IPersistXml
    {
        public OperatingCondition()
        {
        }

        public OperatingCondition(double torque,
                                  int rpm,
                                  double powerFactor,
                                  double serviceFactor,
                                  double angleFactor)
        {
            Torque = torque;
            Rpm = rpm;
            PowerFactor = powerFactor;
            ServiceFactor = serviceFactor;
            AngleFactor = angleFactor;
        }

        private double _torque;
        public double Torque
        {
            get { return _torque; }
            set
            {
                _torque = value;
                NotifyPropertyChanged("Torque");
            }
        }

        public double EquivalentTorque
        {
            get
            {
                return this.Torque * this.AngleFactor * this.LifeFactor * this.PowerFactor;
            }
        }
        private int _rpm;
        public int Rpm
        {
            get { return _rpm; }
            set
            {
                _rpm = value;
                NotifyPropertyChanged("Rpm");
            }
        }

        private double _powerFactor;
        public double PowerFactor
        {
            get { return _powerFactor; }
            set
            {
                _powerFactor = value;
                NotifyPropertyChanged("PowerFactor");
            }
        }

        private double _serviceFactor;
        public double ServiceFactor
        {
            get { return _serviceFactor; }
            set
            {
                _serviceFactor = value;
                NotifyPropertyChanged("ServiceFactor");
            }
        }

        /// <summary>
        /// The worst case shock load for the system.
        /// </summary>
        public double ShockLoad
        {
            get { return Calculations.ShockLoad.Value(this._torque, this._serviceFactor); }

        }

        /// <summary>
        /// The speed x angle product.
        /// </summary>
        public double SpeedAngle
        {
            get { return this._rpm * this._angle; }
        }


        private double _angleFactor;
        public double AngleFactor
        {
            get { return _angleFactor; }
            set
            {
                _angleFactor = value;
                NotifyPropertyChanged("AngleFactor");
            }
        }

        private int _desiredLife;
        public int DesiredLife
        {
            get { return _desiredLife; }
            set
            {
                _desiredLife = value;
                NotifyPropertyChanged("DesiredLife");
            }
        }

        public double LifeFactor
        {
            get
            {
                return Calculations.LifeFactor.Value(_desiredLife);
            }
        }

        private double _angle;
        public double Angle
        {
            get { return _angle; }
            set
            {
                _angle = value;
                NotifyPropertyChanged("Angle");
            }
        }

        [field: NonSerialized] //to avoid an exception.
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }


        /// <summary>
        /// Sets a standard operating condition for this driveshaft series.
        /// </summary>
        /// <param name="series"></param>
        public void SetStandardCondition(DriveshaftSeries series)
        {
            try
            {
                if (series.SeriesType == DriveshaftSeries.TypeOfSeries.Metric)
                {
                    MetricDriveshaftSeries metSeries = (MetricDriveshaftSeries)series;
                }
                else
                {
                    EnglishDriveshaftSeries engSeries = (EnglishDriveshaftSeries)series;

                    this.Torque = engSeries.BearingCapacity;
                    this.Rpm = 100;
                    this.PowerFactor = 1.0;
                    this.ServiceFactor = 1.0;
                    this.AngleFactor = 1.0;
                    this.Angle = 3.0;
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        //return a copy of the current object.
        public object Clone()
        {

            //I think MemberwiseClone() would work here also, since all properties are
            //value types, just in a hurry so I didn't look into.

            OperatingCondition opCond = new OperatingCondition();
            opCond.Angle = this.Angle;
            opCond.AngleFactor = this.AngleFactor;
            opCond.DesiredLife = this.DesiredLife;
            opCond.PowerFactor = this.PowerFactor;
            opCond.Rpm = this.Rpm;
            opCond.ServiceFactor = this.ServiceFactor;
            opCond.Torque = this.Torque;

            return opCond;
        }
        
        public void Load(System.Xml.XmlElement ele)
        {
            Angle = (double)XML.Open_dbl(ele, "Angle", 0.0);
            AngleFactor = (double)XML.Open_dbl(ele, "AngleFactor", 0.0);
            DesiredLife = (int)XML.Open_int(ele, "DesiredLife", 0);
            PowerFactor = (double)XML.Open_dbl(ele, "PowerFactor", 0.0);
            Rpm = (int)XML.Open_int(ele, "Rpm", 0);
            ServiceFactor = (double)XML.Open_dbl(ele, "ServiceFactor", 0.0);
            Torque = (double)XML.Open_dbl(ele, "Torque", 0.0);           
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "Angle", Angle);
            XML.Save(ele, "AngleFactor", AngleFactor);
            XML.Save(ele, "DesiredLife", DesiredLife);
            XML.Save(ele, "PowerFactor", PowerFactor);
            XML.Save(ele, "Rpm", Rpm);
            XML.Save(ele, "ServiceFactor", ServiceFactor);
            XML.Save(ele, "Torque", Torque);
        }
    }
}
