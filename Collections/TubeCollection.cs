using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using PSS.DriveshaftCalculations.DriveshaftParts;
using PSS.DriveshaftCalculations.Data;


namespace PSS.DriveshaftCalculations.Collections
{
    public class TubeCollection : IEnumerable
    {
        public TubeCollection()
        {
            try
            {
                _savedTubes = new List<Tube>();
                LoadTubeData();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<Tube> _savedTubes;
        /// <summary>
        /// A list of tube objects.
        /// </summary>
        public List<Tube> SavedTubes
        {
            get { return _savedTubes; }
            set { _savedTubes = value; }
        }

        public IEnumerator GetEnumerator()
        {
            return _savedTubes.GetEnumerator();
        }

        //gets all tubes saved in the data file.
        private void LoadTubeData()
        {
            try
            {
                _savedTubes = DataLoader.AllTubeSizes();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
