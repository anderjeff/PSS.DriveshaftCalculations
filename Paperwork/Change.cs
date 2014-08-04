using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PSS.Common;


namespace PSS.DriveshaftCalculations.Paperwork
{
    /// <summary>
    /// Represents a change to driveshaft analysis.
    /// </summary>
    [Serializable]
    public class Change : INotifyPropertyChanged, IPersistXml
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private DateTime _changeDate;
        [DisplayName("Date")]
        public DateTime ChangeDate
        {
            get
            {
                return _changeDate;
            }
            set
            {
                _changeDate = value;
                NotifyPropertyChanged("ChangeDate");
            }
        }

        private string _descriptionOfChange;
        [DisplayName("Description")]
        public string DescriptionOfChange
        {
            get
            {
                return _descriptionOfChange;
            }
            set
            {
                _descriptionOfChange = value;
                NotifyPropertyChanged("DescriptionOfChange");
            }
        }

        private string _changesMadeBy;
        [DisplayName("Changed By")]
        public string ChangeMadeBy
        {
            get
            {
                return _changesMadeBy;
            }
            set
            {
                _changesMadeBy = value;
                NotifyPropertyChanged("ChangeMadeBy");
            }
        }

        public void Load(System.Xml.XmlElement ele)
        {
            ChangeDate = (DateTime)XML.Open_DateTime(ele, "ChangeDate", DateTime.Now);
            ChangeMadeBy = (string)XML.Open_str(ele, "ChangeMadeBy", string.Empty);
            DescriptionOfChange = (string)XML.Open_str(ele, "DescriptionOfChange", string.Empty);
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "ChangeDate", ChangeDate);
            XML.Save(ele, "ChangeMadeBy", ChangeMadeBy);
            XML.Save(ele, "DescriptionOfChange", DescriptionOfChange);
        }
    }
}
