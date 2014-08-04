using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;

using PSS.DriveshaftCalculations.DriveshaftParts;
using PSS.Common;

namespace PSS.DriveshaftCalculations.Collections
{
    [Serializable]
    public class DriveshaftLayoutCollection : IEnumerable, IPersistXml
    {
        public DriveshaftLayoutCollection()
        {
            _savedLayouts = new BindingList<DriveshaftLayout>();
            _savedLayouts.RaiseListChangedEvents = true;
        }

        private BindingList<DriveshaftLayout> _savedLayouts;
        /// <summary>
        /// A collection of driveshaft layouts.
        /// </summary>
        public BindingList<DriveshaftLayout> SavedLayouts
        {
            get { return _savedLayouts; }
            set { _savedLayouts = value; }
        }

        private DriveshaftLayout _selectedLayout;
        /// <summary>
        /// The layout selected from this driveshaft layout collection.
        /// </summary>
        public DriveshaftLayout SelectedLayout
        {
            get { return _selectedLayout; }
            set { _selectedLayout = value; }
        }
        
        public IEnumerator GetEnumerator()
        {
            return _savedLayouts.GetEnumerator();
        }

        #region MyRegion
       
        public void Load(System.Xml.XmlElement ele)
        {
            SavedLayouts = new BindingList<DriveshaftLayout>();
            SavedLayouts.RaiseListChangedEvents = true;

            List<DriveshaftLayout> tempLayout = (List<DriveshaftLayout>)XML.Open_List<DriveshaftLayout>(ele, "SavedLayouts", new List<DriveshaftLayout>());
            foreach (DriveshaftLayout layout in tempLayout)
            {
                SavedLayouts.Add(layout);
            }

            DriveshaftLayout layoutTemp = (DriveshaftLayout)XML.Open_IPersistXml(ele, "SelectedLayout", new DriveshaftLayout());

            //so the object is the same one as in the list.
            SelectedLayout = (from layout in SavedLayouts
                              where layout.Equals(layoutTemp)
                              select layout).FirstOrDefault();
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save_List<DriveshaftLayout>(ele, "SavedLayouts", SavedLayouts.ToList<DriveshaftLayout>());
            XML.Save(ele, "SelectedLayout", SelectedLayout);
        }
 
        #endregion
    }
}
