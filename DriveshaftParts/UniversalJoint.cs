using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PSS.Common;

namespace PSS.DriveshaftCalculations.DriveshaftParts
{
    /// <summary>
    /// Represents a u-joint with a name and angle.
    /// </summary>
    [Serializable]
    public class UniversalJoint : INotifyPropertyChanged, 
                                  ICloneable, 
                                  IEquatable<UniversalJoint>, IPersistXml
    {
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged("Name"); 
                }
            }
        }

        private double _jointAngle;
        [DisplayName("Joint Angle")]
        public double JointAngle
        {
            get
            {
                return _jointAngle;
            }
            set
            {
                if (_jointAngle != value)
                {
                    _jointAngle = value;
                    NotifyPropertyChanged("JointAngle"); 
                }
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
        /// Returns a deep copy of a universal joint.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            try
            {
                UniversalJoint copy = new UniversalJoint();
                copy.Name = string.Copy(this.Name);
                copy.JointAngle = this.JointAngle;

                return copy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override bool Equals(object obj)
        {
            UniversalJoint uj = obj as UniversalJoint;
            if (uj != null)
            {
                return this.Equals(uj);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(UniversalJoint other)
        {
            return this.JointAngle.Equals(other.JointAngle) &&
                   this.Name.Equals(other.Name);
        }

        #region IPersistXmlMethods
        
        public void Load(System.Xml.XmlElement ele)
        {
            JointAngle = (double)XML.Open_dbl(ele, "JointAngle", 0.0);
            Name = (string)XML.Open_str(ele, "Name", string.Empty);
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "JointAngle", JointAngle);
            XML.Save(ele, "Name", Name);
        }
 
        #endregion
    }
}
