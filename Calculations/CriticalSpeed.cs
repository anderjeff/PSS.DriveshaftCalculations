using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PSS.DriveshaftCalculations.DriveshaftParts;
using PSS.Common;

namespace PSS.DriveshaftCalculations.Calculations
{
    [Serializable]
    public class CriticalSpeed : ICloneable, IPersistXml
    {
        public CriticalSpeed()
        {
        }

        public CriticalSpeed(Tube tube, int speed)
        {
            _tube = tube;
            _speed = speed;
        }

        //assumes round steel shaft with modulus of elasticity of 29 million
        // and a density of 0.281 lb/in^3, see page 267 of SAE AE-7
        internal int critSpdConst = 4705000;

        private Tube _tube;
        [Browsable(false)]
        public Tube Tube 
        {
            get 
            {
                return _tube;
            }
            set
            {
                _tube = value;
            }
        }

        private int _speed;
        [Browsable(false)]
        public int Speed
        {
            get 
            {
                return _speed;
            }
            set
            {
                _speed = value;
            }
        }

        /// <summary>
        /// The value of the critical speed.
        /// </summary>
        [DisplayName("Critical Speed")]
        public Double Value
        {
            get
            {
                try
                {
                    if (_tube.TubeLength > 0)
                    {
                        return critSpdConst *
                               Math.Sqrt(Math.Pow(_tube.OuterDia, 2) + Math.Pow(_tube.InnerDia, 2)) /
                               Math.Pow(_tube.TubeLength, 2);
                    }
                    else
                    {
                        //infinity, sort of...
                        return Double.MaxValue;
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the half critical speed.
        /// </summary>
        [DisplayName("Half Critical")]
        public double HalfCritical
        {
            get { return Value / 2; }
        }

        /// <summary>
        /// Gets the 42% critical speed.
        /// </summary>
        [DisplayName("42% Critical")]
        public double ErrorLimitLow
        {
            get { return Value * .42; }
        }

        /// <summary>
        /// Gets the 58% critical speed
        /// </summary>
        [DisplayName("58% Critical")]
        public double ErrorLimitHigh
        {
            get { return Value * .58; }
        }

        /// <summary>
        /// Gets the max safe operating speed, 0.75 times the critical speed.
        /// </summary>
        [DisplayName("Max Safe Speed")]
        public double MaxSafeOperatingSpeed
        {
            get { return Value * .75; }
        }

        [DisplayName("Tube Size")]
        public string TubeSize
        {
            get
            {
                return _tube.ToString();
            }
        }

        /// <summary>
        /// Gets all the imput info in a single string.
        /// </summary>
        public string TubeSizeLengthSpeed
        {
            get
            {
                return string.Format("{0} x {1:n2} @ {2:n0} rpm", 
                                     _tube.ToString(), _tube.TubeLength, Rpm);
            }
        }
        public double Length
        {
            get
            {
                return _tube.TubeLength;
            }
        }

        public int Rpm
        {
            get
            {
                return _speed;
            }
        }


        public object Clone()
        {
            CriticalSpeed copy = new CriticalSpeed();
            copy._tube = (Tube)this._tube.Clone();
            copy._speed = this._speed;

            return copy;
        }


        #region IPersistXmlMethods

        public void Load(System.Xml.XmlElement ele)
        {
            Tube = (Tube)XML.Open_IPersistXml(ele, "Tube", null);
            Speed = (int)XML.Open_int(ele, "Speed", 0);
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "Tube", Tube);
            XML.Save(ele, "Speed", Speed);
        } 

        #endregion
    }
}
