using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PSS.DriveshaftCalculations.Calculations;
using PSS.Common;

namespace PSS.DriveshaftCalculations.DriveshaftParts
{
    [Serializable]
    public class Tube : ICloneable, INotifyPropertyChanged, IPersistXml
    {
        public Tube()
        {
        }

        public Tube(Double od, Double wall, Double length)
        {
            OuterDia = od;
            Wall = wall;
            TubeLength = length;
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

        public const Single MODULUS_OF_RIGIDITY = 11000000; //1.10E7 for steel.

        private double _outerDia;
        /// <summary>
        /// Outer diameter of the tube.
        /// </summary>
        public double OuterDia
        {
            get { return _outerDia; }
            set
            {
                _outerDia = value;
                NotifyPropertyChanged("OuterDia");
            }
        }

        private double _wall;
        /// <summary>
        /// Wall thickness of the tube.
        /// </summary>
        public double Wall
        {
            get
            {
                return _wall;
            }
            set
            {
                _wall = value;
                NotifyPropertyChanged("Wall");
            }
        }

        /// <summary>
        /// Inner diameter of the tube.  Calculated value from outer diameter and wall thickness.
        /// </summary>
        public double InnerDia
        {
            get { return OuterDia - 2 * Wall; }
        }

        private double _tubeLength;
        /// <summary>
        /// The length of the tube.
        /// </summary>
        public double TubeLength
        {
            get { return _tubeLength; }
            set
            {
                _tubeLength = value;
                NotifyPropertyChanged("TubeLength");
            }
        }

        public override string ToString()
        {
            return string.Format("{0:n2}-{1:n3}", OuterDia, Wall);
        }

        public object Clone()
        {
            Tube copy = new Tube();

            copy.OuterDia = this.OuterDia;
            copy.TubeLength = this.TubeLength;
            copy.Wall = this.Wall;

            return copy;
        }


        public void Load(System.Xml.XmlElement ele)
        {
            OuterDia = (double)XML.Open_dbl(ele, "OuterDia", 0.0);
            Wall = (double)XML.Open_dbl(ele, "Wall", 0.0);
            TubeLength = (double)XML.Open_dbl(ele, "TubeLength", 0.0);
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "OuterDia", OuterDia);
            XML.Save(ele, "Wall", Wall);
            XML.Save(ele, "TubeLength", TubeLength);
        }
    }
}
