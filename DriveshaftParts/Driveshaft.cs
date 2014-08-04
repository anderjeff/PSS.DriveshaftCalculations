using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PSS.Common;

namespace PSS.DriveshaftCalculations.DriveshaftParts
{
    /// <summary>
    /// Represents a driveshaft with a name and a phasing.
    /// </summary>
    [Serializable]
    public class Driveshaft : INotifyPropertyChanged, ICloneable, IEquatable<Driveshaft>, IPersistXml
    {
        private string _name;
        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }


        private bool _isCrossed;
        [DisplayName("Crossed Phasing")]
        public bool IsCrossed
        {
            get { return _isCrossed; }
            set
            {
                _isCrossed = value;
                NotifyPropertyChanged("IsCrossed");
            }
        }


        /// <summary>
        /// A value used to multiply the angles for torsional and inertial calculations.
        /// </summary>
        [Browsable(false)]
        public int Multiplier
        {
            get
            {
                if (IsCrossed)
                {
                    return 1;
                }
                else
                {
                    return -1;
                }
            }
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        /// <summary>
        /// Returns a deep cop
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            try
            {
                Driveshaft copy = new Driveshaft();
                copy.Name = String.Copy(this.Name);
                copy.IsCrossed = this.IsCrossed;

                //since they are all value members.
                return copy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override bool Equals(object obj)
        {
            Driveshaft drvShft = obj as Driveshaft;
            if (drvShft != null)
            {
                return this.Equals(drvShft);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(Driveshaft other)
        {
            return this.IsCrossed.Equals(other.IsCrossed) &&
                   this.Name.Equals(other.Name);
        }

        #region IPersistXmlMethods
        
        public void Load(System.Xml.XmlElement ele)
        {
            IsCrossed = (bool)XML.Open_bln(ele, "IsCrossed", false);
            Name = (string)XML.Open_str(ele, "Name", string.Empty);
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "IsCrossed", IsCrossed);
            XML.Save(ele, "Name", Name);
        }
 
        #endregion
    }
}
