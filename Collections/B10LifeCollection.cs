using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using PSS.DriveshaftCalculations.Calculations;
using PSS.DriveshaftCalculations.DriveshaftParts;
using PSS.DriveshaftCalculations.DutyCycle;
using PSS.Common;

namespace PSS.DriveshaftCalculations.Collections
{
    [Serializable]
    public class B10LifeCollection : IEnumerable, IPersistXml
    {
        /// <summary>
        /// A B10 life collection for a duty cycle.
        /// </summary>
        /// <param name="allSeries">Every driveshaft series.</param>
        /// <param name="dtyCollection">A collection of duty cycle objects.</param>
        public B10LifeCollection(DutyCycleConditionCollection dtyCollection)
        {
            //bearing life objects.
            _lifeCollection = new List<B10Life>();

            //this is what gets the duty cycle.  The other constructor will not get inside
            //this because the DutyCycleConditionCollection was set to null;
            if (dtyCollection != null)
            {
                GetLifeForDutyCycle(new DriveshaftSeriesCollection(), dtyCollection);
            }
        }

        /// <summary>
        /// A B10 life collection for a single conditition.
        /// </summary>
        /// <param name="allSeries">Every driveshaft series.</param>
        /// <param name="opCond">The single operating condition.</param>
        public B10LifeCollection(DriveshaftSeriesCollection allSeries, OperatingCondition opCond)
            : this(DutyCycleConditionCollection.Empty())
        {
            GetLifeForSingleCondition(allSeries, opCond);
        }

        private List<B10Life> _lifeCollection;
        /// <summary>
        /// A collection of B10 lives of different driveshaft series.
        /// </summary>
        public List<B10Life> LifeCollection
        {
            get { return _lifeCollection; }
            set { _lifeCollection = value; }
        }
        
        public IEnumerator GetEnumerator()
        {
            return _lifeCollection.GetEnumerator();
        }

        private void GetLifeForSingleCondition(DriveshaftSeriesCollection allSeries, OperatingCondition opCond)
        {
            foreach (DriveshaftSeries series in allSeries)
            {
                B10Life b10 = new B10Life(opCond, series);
                _lifeCollection.Add(b10);
            }
        }

        public void GetLifeForDutyCycle(DriveshaftSeriesCollection allSeries,
                                        DutyCycleConditionCollection dutyCycle)
        {
            foreach (DriveshaftSeries series in allSeries)
            {
                B10Life b10 = new B10Life(dutyCycle, series);
                _lifeCollection.Add(b10);
            }
        }


        #region IPersistXml

        public void Load(System.Xml.XmlElement ele)
        {
            LifeCollection = (List<B10Life>)XML.Open_List<B10Life>(ele, "LifeCollection", new List<B10Life>());
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save_List<B10Life>(ele, "LifeCollection", LifeCollection);
        }
 
        #endregion
    }
}
