using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections;

using PSS.DriveshaftCalculations.DriveshaftParts;
using PSS.DriveshaftCalculations.Paperwork;
using PSS.Common;

namespace PSS.DriveshaftCalculations.Collections
{
    /// <summary>
    /// Represents a collection of selected driveshafts.
    /// </summary>
    [Serializable]
    public class SelectedDriveshaftCollection : IEnumerable, IPersistXml
    {
        public SelectedDriveshaftCollection()
        {
            _savedSelectedDriveshafts = new BindingList<SelectedDriveshaft>();
        }

        public SelectedDriveshaftCollection(DriveshaftLayout layout, CriticalSpeedCollection csColl)
            :this()
        {
            try
            {
                Layout = layout;
                _csColl = csColl;

                //populates the _items list.
                ProcessLayout();
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void ProcessLayout()
        {
            try
            {
                foreach (Driveshaft d in Layout.Driveshafts)
                {
                    //Create a selected driveshaft object 
                    //and add it to the collection.
                    SelectedDriveshaft sd = new SelectedDriveshaft(d, _csColl);                    
                    _savedSelectedDriveshafts.Add(sd);
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        //used for selecting tested tube sizes.
        private CriticalSpeedCollection _csColl;

        [Browsable(false)]
        public DriveshaftLayout Layout { get; set; }

        private BindingList<SelectedDriveshaft> _savedSelectedDriveshafts;
        /// <summary>
        /// A list of driveshafts that will become the selected driveshafts.
        /// </summary>
        public BindingList<SelectedDriveshaft> SavedSelectedDriveshafts
        {
            get { return _savedSelectedDriveshafts; }
            set { _savedSelectedDriveshafts = value; }
        }
        
        public IEnumerator GetEnumerator()
        {
            return _savedSelectedDriveshafts.GetEnumerator();
        }

        public void Load(System.Xml.XmlElement ele)
        {
            SavedSelectedDriveshafts = new BindingList<SelectedDriveshaft>();
            List<SelectedDriveshaft> selDrvshfts = (List<SelectedDriveshaft>)XML.Open_List<SelectedDriveshaft>(ele, "SavedSelectedDriveshafts", new List<SelectedDriveshaft>());

            foreach (SelectedDriveshaft selDr in selDrvshfts)
            {
                SavedSelectedDriveshafts.Add(selDr);
            }

            _csColl = (CriticalSpeedCollection)XML.Open_IPersistXml(ele, "_csColl", new CriticalSpeedCollection());
            Layout = (DriveshaftLayout)XML.Open_IPersistXml(ele, "Layout", new DriveshaftLayout());
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save_List<SelectedDriveshaft>(ele, "SavedSelectedDriveshafts", SavedSelectedDriveshafts.ToList<SelectedDriveshaft>());
            XML.Save(ele, "_csColl", _csColl);
            XML.Save(ele, "Layout", Layout);
        }
    }
}
