using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PSS.DriveshaftCalculations.Analysis;
using PSS.DriveshaftCalculations.DutyCycle;
using PSS.Common;


namespace PSS.DriveshaftCalculations.Paperwork
{
    /// <summary>
    /// Represents a u-joint chosen as a final design selection.
    /// </summary>
    [Serializable]
    public class SelectedUjoint : INotifyPropertyChanged, IPersistXml
    {
        public SelectedUjoint()
        {
        }

        public SelectedUjoint(DutyCycleConditionCollection dcCondColl) : this()
        {
            ConditionCollection = dcCondColl;
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
        /// Resulting analysis object, tells us series information and hours.
        /// </summary>
        [Browsable(false)]
        public AnalysisResult SeriesAnalysis { get; set; }

        private DutyCycleConditionCollection _conditionCollection;
        /// <summary>
        /// The duty cycle condition collection for this universal joint.
        /// </summary>
        [Browsable(false)]        
        public DutyCycle.DutyCycleConditionCollection ConditionCollection 
        {
            get
            {
                return _conditionCollection;
            }
            set
            {
                if (_conditionCollection != value)
                {
                    _conditionCollection = value;
                    _conditionCollection.PropertyChanged += new PropertyChangedEventHandler(ConditionCollection_PropertyChanged); 
                }
            }
        }

        
        private string _seriesName;
        [DisplayName("Selected Series")]
        public string SeriesName
        {
            get { return _seriesName; }
            set 
            {
                if (_seriesName != value)
                {
                    _seriesName = value;
                    NotifyPropertyChanged("SeriesName");                
                }            
            }
        }
                
        private double _bearingLife;
        [DisplayName("Bearing Life")]
        public double BearingLife
        {
            get { return _bearingLife; }
            set 
            {
                if (_bearingLife != value)
                {
                    _bearingLife = value;
                    NotifyPropertyChanged("BearingLife");
                }
            }
        }

        private string _uJointName;
        /// <summary>
        /// The unique name of this u-joint.
        /// </summary>
        [DisplayName("U-Joint Name")]
        public string UjointName
        {
            get
            {
                return _uJointName;
            }
            set
            {
                if (_uJointName != value)
                {
                    _uJointName = value;
                    NotifyPropertyChanged("UjointName");
                }
            }
        }
        
        /// <summary>
        /// The driveshaft part number this universal joint belongs to.
        /// </summary>
        [DisplayName("Driveshaft P/N")]        
        public string DriveshaftPartNumber { get; set; }

        public void ConditionCollection_PropertyChanged(object sender, EventArgs e)
        {
            if (ConditionCollection.SelectedSeriesAnalysisResult != null)
            {
                BearingLife = ConditionCollection.SelectedSeriesAnalysisResult.Hours;
                SeriesName = ConditionCollection.SelectedSeriesAnalysisResult.SeriesName;
                SeriesAnalysis = ConditionCollection.SelectedSeriesAnalysisResult;
                UjointName = ConditionCollection.JointName; 
            }
        }


        #region MyRegion

        public void Load(System.Xml.XmlElement ele)
        {
            BearingLife = (double)XML.Open_dbl(ele, "BearingLife", 0.0);
            ConditionCollection = (DutyCycleConditionCollection)XML.Open_IPersistXml(ele, "ConditionCollection", new DutyCycleConditionCollection());
            DriveshaftPartNumber = (string)XML.Open_str(ele, "DriveshaftPartNumber", string.Empty);
            SeriesAnalysis = (AnalysisResult)XML.Open_IPersistXml(ele, "SeriesAnalysis", null);
            SeriesName = (string)XML.Open_str(ele, "SeriesName", string.Empty);
            UjointName = (string)XML.Open_str(ele, "UjointName", string.Empty);
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "BearingLife", BearingLife);
            XML.Save(ele, "ConditionCollection", ConditionCollection);
            XML.Save(ele, "DriveshaftPartNumber", DriveshaftPartNumber);
            XML.Save(ele, "SeriesAnalysis", SeriesAnalysis);
            XML.Save(ele, "SeriesName", SeriesName);
            XML.Save(ele, "UjointName", UjointName);
        } 

        #endregion
    }
}
