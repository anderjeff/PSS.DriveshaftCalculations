using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;

using PSS.DriveshaftCalculations.Calculations;
using PSS.Common;

namespace PSS.DriveshaftCalculations.Collections
{
    [Serializable]
    public class CriticalSpeedCollection : IEnumerable, IPersistXml
    {
        public CriticalSpeedCollection()
        {
            _savedCs = new BindingList<CriticalSpeed>();
            _savedCs.RaiseListChangedEvents = true;
        }

        private BindingList<CriticalSpeed> _savedCs;
        /// <summary>
        /// A list of critical speed items.
        /// </summary>
        public BindingList<CriticalSpeed> SavedCriticalSpeeds
        {
            get { return _savedCs; }
            set { _savedCs = value; }
        }

        public IEnumerator GetEnumerator()
        {
            return _savedCs.GetEnumerator();
        }

        #region IPersistXmlMethods

        public void Load(System.Xml.XmlElement ele)
        {
            SavedCriticalSpeeds = new BindingList<CriticalSpeed>();
            SavedCriticalSpeeds.RaiseListChangedEvents = true;

            //work around for loading the binding list.
            List<CriticalSpeed> savedCriticalSpeeds = (List<CriticalSpeed>)XML.Open_List<CriticalSpeed>(
                ele, "SavedCriticalSpeeds", new List<CriticalSpeed>());
            foreach (CriticalSpeed cs in savedCriticalSpeeds)
            {
                SavedCriticalSpeeds.Add(cs);
            }
        }

        public void Save(System.Xml.XmlElement ele)
        {
            List<CriticalSpeed> csTemp = SavedCriticalSpeeds.ToList<CriticalSpeed>();

            XML.Save_List<CriticalSpeed>(ele, "SavedCriticalSpeeds", csTemp);
        } 

        #endregion
    }
}
