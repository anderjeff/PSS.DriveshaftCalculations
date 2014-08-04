using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PSS.DriveshaftCalculations.Collections;
using PSS.DriveshaftCalculations.Calculations;
using PSS.DriveshaftCalculations.DriveshaftParts;
using PSS.Common;


namespace PSS.DriveshaftCalculations.Paperwork
{
    /// <summary>
    /// Represents a driveshaft chosen as a final design selection.
    /// </summary>
    [Serializable]
    public class SelectedDriveshaft : IPersistXml
    {
        public SelectedDriveshaft()
        {
        }

        public SelectedDriveshaft(Driveshaft d, CriticalSpeedCollection csColl) : this()
        {
            //so we use the setter of the public property to 
            //populate other object values.
            ObjDriveshaft = d;

            _criticalSpeedCollection = csColl;
        }

        [DisplayName("Driveshaft Name")]
        public string DriveshaftName { get; set; }

        private string _partNumber;
        [DisplayName("Part Number")]
        public string PartNumber 
        {
            get
            {
                return _partNumber;
            }
            set
            {
                _partNumber = value;
            }
        }

        /// <summary>
        /// The phasing of the selected driveshaft, can be parallel or crossed.
        /// </summary>
        public string Phasing { get; private set; }

        private string _tubeSize;
        [DisplayName("Tube Size")]
        [Browsable(false)]
        public string TubeSize 
        {
            get
            {
                return _tubeSize;
            }
            set
            {
                _tubeSize = value;
            }
        }

        private Driveshaft _objDriveshaft;
        /// <summary>
        /// The driveshaft object this SelectedDriveshaft uses.
        /// </summary>
        [Browsable(false)]
        public Driveshaft ObjDriveshaft
        {
            get { return _objDriveshaft; }
            set 
            {
                _objDriveshaft = value;

                //set this objects properties.
                this.DriveshaftName = _objDriveshaft.Name;

                if (_objDriveshaft.IsCrossed)
                {
                    this.Phasing = "Crossed";
                }
                else
                {
                    this.Phasing = "Parallel";
                }
            }
        }
        
        private CriticalSpeedCollection _criticalSpeedCollection;
        [Browsable(false)]
        public CriticalSpeedCollection CriticalSpeedCollection
        {
            get
            {
                return _criticalSpeedCollection;
            }
            set
            {
                _criticalSpeedCollection = value;
            }
        }

        #region IPersistXmlMethods

        public void Load(System.Xml.XmlElement ele)
        {
            CriticalSpeedCollection = (CriticalSpeedCollection)XML.Open_IPersistXml(ele, "CriticalSpeedCollection", new CriticalSpeedCollection());
            DriveshaftName = (string)XML.Open_str(ele, "DriveshaftName", string.Empty);
            ObjDriveshaft = (Driveshaft)XML.Open_IPersistXml(ele, "ObjDriveshaft", new Driveshaft());
            PartNumber = (string)XML.Open_str(ele, "PartNumber", string.Empty);
            TubeSize = (string)XML.Open_str(ele, "TubeSize", string.Empty);
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "CriticalSpeedCollection", CriticalSpeedCollection);
            XML.Save(ele, "DriveshaftName", DriveshaftName);
            XML.Save(ele, "ObjDriveshaft", ObjDriveshaft);
            XML.Save(ele, "PartNumber", PartNumber);
            XML.Save(ele, "TubeSize", TubeSize);
        } 

        #endregion
    }
}
