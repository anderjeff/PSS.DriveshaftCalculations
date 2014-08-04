using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using PSS.DriveshaftCalculations.DriveshaftParts;
using PSS.DriveshaftCalculations.Data;

namespace PSS.DriveshaftCalculations.Collections
{
    [Serializable]
    public class DriveshaftSeriesCollection : IEnumerable
    {
        public DriveshaftSeriesCollection()
        {
            try
            {
                _savedSeries = new List<DriveshaftSeries>();
                LoadSeriesData();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<DriveshaftSeries> _savedSeries;
        /// <summary>
        /// A collection of driveshaft series objects.
        /// </summary>
        public List<DriveshaftSeries> SavedSeries
        {
            get { return _savedSeries; }
            set { _savedSeries = value; }
        }

        //Required by IEnumerable
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _savedSeries.GetEnumerator();
        }

        private void LoadSeriesData()
        {
            try
            {
                _savedSeries = DataLoader.AllDriveshaftSeries();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private DriveshaftSeries GetSeriesByName(string name)
        {
            DriveshaftSeries returnSeries = (from s in _savedSeries
                                             where s.Name == name
                                             select s).FirstOrDefault();
            return returnSeries;
        }

    }
}
