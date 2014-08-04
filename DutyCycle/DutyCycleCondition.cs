using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PSS.DriveshaftCalculations.DriveshaftParts;
using PSS.Common;

namespace PSS.DriveshaftCalculations.DutyCycle
{
    /// <summary>
    /// Represents a single condition for a single u-joint in a duty cycle.
    /// </summary>
    [Serializable]
    public class DutyCycleCondition : INotifyPropertyChanged, IPersistXml
    {
        public DutyCycleCondition()
        {
        }

        public DutyCycleCondition(OperatingCondition opCond)
        {
            //get a deep copy of the operating condition, in case it changes.
            OperatingCondition = (OperatingCondition)opCond.Clone();
        }

        private string _name;
        /// <summary>
        /// The user specified name for this condition.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set
            {
                if (_name != value)
                {
                    _name = value;
                    NotifyPropertyChanged("Name");
                }                
            }
        }

        private double _percentOfTime;
        /// <summary>
        /// The percent of time of the full 100% duty cycle 
        /// that this condition applies
        /// </summary>
        [DisplayName("Percent")]
        public double PercentOfTime
        {
            get
            {
                return _percentOfTime;
            }

            set
            {
                if (!PercentIsLocked)
                {
                    if (_percentOfTime != value)
                    {
                        //want the user to be able to type a percent as a whole
                        //number, not as a decimal value. ex 25.2 instead of .252
                        OnPropertyChanging("PercentOfTime", _percentOfTime, value);
                        NotifyPropertyChanged("PercentOfTime");
                    }
                }
            }
        }

        private bool _percentIsLocked;
        /// <summary>
        /// Lock the percent of time when this is true.
        /// </summary>
        [DisplayName("Lock")]
        public bool PercentIsLocked
        {
            get { return _percentIsLocked; }
            set
            {
                if (_percentIsLocked != value)
                {
                    _percentIsLocked = value;
                    NotifyPropertyChanged("PercentIsLocked");
                }
            }
        }

        private OperatingCondition _operatingCondition;
        /// <summary>
        /// The operating condition this Duty Cycle is linked to.
        /// </summary>
        [Browsable(false)]
        public OperatingCondition OperatingCondition
        {
            get { return _operatingCondition; }
            set
            {
                if (_operatingCondition != value)
                {
                    _operatingCondition = value;
                    NotifyPropertyChanged("OperatingCondition");
                }                
            }
        }

        private string _descriptionOfCondition;
        /// <summary>
        /// Provides a detailed description of this particular condition.
        /// </summary>
        [Browsable(false)]
        public string DescriptionOfCondition
        {
            get { return _descriptionOfCondition; }
            set
            {
                if (_descriptionOfCondition != value)
                {
                    _descriptionOfCondition = value;
                    NotifyPropertyChanged("DescriptionOfCondition");
                }               
            }
        }

        [field: NonSerialized]
        public event EventHandler PercentChanging;
        protected void OnPropertyChanging(string propertyName, double oldVal, double newVal)
        {
            EventHandler pc = PercentChanging;
            if (pc != null)
            {
                pc(this, new PercentageChangeEventArgs(propertyName, oldVal, newVal));
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

        //Allows the listener to set the private field.
        public void SetPercentOfTime(double value)
        {
            if (!PercentIsLocked)
            {
                _percentOfTime = value / 100;

            }
        }




        #region IPersistXmlMethods
       
        public void Load(System.Xml.XmlElement ele)
        {
            _descriptionOfCondition = (string)XML.Open_str(ele, "DescriptionOfCondition", string.Empty);
            _name = (string)XML.Open_str(ele, "Name", string.Empty);
            _percentIsLocked = (bool)XML.Open_bln(ele, "PercentIsLocked", false);
            _percentOfTime = (double)XML.Open_dbl(ele, "PercentOfTime", 0.0);

            OperatingCondition = (OperatingCondition)XML.Open_IPersistXml(ele, "OperatingCondition", null);
        }

        public void Save(System.Xml.XmlElement ele)
        
        {
            XML.Save(ele, "DescriptionOfCondition", DescriptionOfCondition);
            XML.Save(ele, "Name", Name);
            XML.Save(ele, "PercentIsLocked", PercentIsLocked);
            XML.Save(ele, "PercentOfTime", PercentOfTime);

            XML.Save(ele, "OperatingCondition", OperatingCondition);
        } 

        #endregion
    }
}
