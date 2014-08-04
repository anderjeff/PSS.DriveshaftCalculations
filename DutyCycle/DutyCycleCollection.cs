using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;

using PSS.Common;


namespace PSS.DriveshaftCalculations.DutyCycle
{
    /// <summary>
    /// Represents a collection of DutyCycleConditionCollections.
    /// </summary>
    [Serializable]
    public class DutyCycleCollection : IEnumerable, IPersistXml
    {
        public DutyCycleCollection()
        {
            _jointDutyCycles = new BindingList<DutyCycleConditionCollection>();
            _jointDutyCycles.RaiseListChangedEvents = true;
        }

        private BindingList<DutyCycleConditionCollection> _jointDutyCycles;
        /// <summary>
        /// A collection of DutyCycleConditionCollections.
        /// </summary>
        public BindingList<DutyCycleConditionCollection> JointDutyCycles
        {
            get { return _jointDutyCycles; }
            set { _jointDutyCycles = value; }
        }

        public IEnumerator GetEnumerator()
        {
            return _jointDutyCycles.GetEnumerator();
        }

        public void Load(System.Xml.XmlElement ele)
        {
            JointDutyCycles = new BindingList<DutyCycleConditionCollection>();
            List<DutyCycleConditionCollection> temps = (List<DutyCycleConditionCollection>)XML.Open_List<DutyCycleConditionCollection>(ele, "JointDutyCycles", new List<DutyCycleConditionCollection>());
            foreach(DutyCycleConditionCollection temp in temps)
            {
                JointDutyCycles.Add(temp);
            }            
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save_List<DutyCycleConditionCollection>(ele, "JointDutyCycles", JointDutyCycles.ToList<DutyCycleConditionCollection>());
        }
    }
}
