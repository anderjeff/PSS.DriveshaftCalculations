using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

using PSS.DriveshaftCalculations.Calculations;

namespace PSS.DriveshaftCalculations.Collections
{
    /// <summary>
    /// A loaded collection of service factors for various applications.
    /// </summary>
    public class ServiceFactorCollection : IEnumerable
    {
        public ServiceFactorCollection()
        {
            _items = new List<ServiceFactor>();
            LoadServiceFactors();
        }

        private List<ServiceFactor> _items;

        public List<ServiceFactor> Items
        {
            get { return _items; }
            set { _items = value; }
        }

        public IEnumerator GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        private void LoadServiceFactors()
        {
            _items.Add(new ServiceFactor("Centrifugal Pumps", ServiceFactor.LoadCondition.ContinuousLoad));
            _items.Add(new ServiceFactor("Generators", ServiceFactor.LoadCondition.ContinuousLoad));
            _items.Add(new ServiceFactor("Conveyors", ServiceFactor.LoadCondition.ContinuousLoad));
            _items.Add(new ServiceFactor("Ventilators", ServiceFactor.LoadCondition.ContinuousLoad));

            //with frequent starts and stops.
            _items.Add(new ServiceFactor("Centrifugal Pumps", ServiceFactor.LoadCondition.LightShockLoad));
            _items.Add(new ServiceFactor("Generators", ServiceFactor.LoadCondition.LightShockLoad));
            _items.Add(new ServiceFactor("Conveyors", ServiceFactor.LoadCondition.LightShockLoad));
            _items.Add(new ServiceFactor("Ventilators", ServiceFactor.LoadCondition.LightShockLoad));
            _items.Add(new ServiceFactor("Machine Tools", ServiceFactor.LoadCondition.LightShockLoad));
            _items.Add(new ServiceFactor("Printing Machines", ServiceFactor.LoadCondition.LightShockLoad));
            _items.Add(new ServiceFactor("Wood Handling Machines", ServiceFactor.LoadCondition.LightShockLoad));
            _items.Add(new ServiceFactor("Paper and Textile Machines", ServiceFactor.LoadCondition.LightShockLoad));

            _items.Add(new ServiceFactor("Multi Cylinder Pumps", ServiceFactor.LoadCondition.MediumShockLoad));
            _items.Add(new ServiceFactor("Multi Cylinder Compressors", ServiceFactor.LoadCondition.MediumShockLoad));
            _items.Add(new ServiceFactor("Large Ventilators", ServiceFactor.LoadCondition.MediumShockLoad));
            _items.Add(new ServiceFactor("Marine Transmissions", ServiceFactor.LoadCondition.MediumShockLoad));
            _items.Add(new ServiceFactor("Calendars", ServiceFactor.LoadCondition.MediumShockLoad));
            _items.Add(new ServiceFactor("Transport Rolling Tables", ServiceFactor.LoadCondition.MediumShockLoad));
            _items.Add(new ServiceFactor("Rod and Bar Mills", ServiceFactor.LoadCondition.MediumShockLoad));
            _items.Add(new ServiceFactor("Small Pitch Rolls", ServiceFactor.LoadCondition.MediumShockLoad));
            _items.Add(new ServiceFactor("Small Tube Mills", ServiceFactor.LoadCondition.MediumShockLoad));
            _items.Add(new ServiceFactor("Locomotive Primary Drives", ServiceFactor.LoadCondition.MediumShockLoad));
            _items.Add(new ServiceFactor("Heavy Paper and Textile Mills", ServiceFactor.LoadCondition.MediumShockLoad));
            _items.Add(new ServiceFactor("Irrigation Pumps", ServiceFactor.LoadCondition.MediumShockLoad));
            _items.Add(new ServiceFactor("Blowers", ServiceFactor.LoadCondition.MediumShockLoad));

            _items.Add(new ServiceFactor("One Cylinder Compressors", ServiceFactor.LoadCondition.HeavyShockLoad));
            _items.Add(new ServiceFactor("One Cylinder Pumps", ServiceFactor.LoadCondition.HeavyShockLoad));
            _items.Add(new ServiceFactor("Mixers", ServiceFactor.LoadCondition.HeavyShockLoad));
            _items.Add(new ServiceFactor("Crane Travel Drives", ServiceFactor.LoadCondition.HeavyShockLoad));
            _items.Add(new ServiceFactor("Bucket Wheel Reclaimers", ServiceFactor.LoadCondition.HeavyShockLoad));
            _items.Add(new ServiceFactor("Pressers", ServiceFactor.LoadCondition.HeavyShockLoad));
            _items.Add(new ServiceFactor("Rotary Drill Rigs", ServiceFactor.LoadCondition.HeavyShockLoad));
            _items.Add(new ServiceFactor("Locomotive Secondary Drives", ServiceFactor.LoadCondition.HeavyShockLoad));
            _items.Add(new ServiceFactor("Continuous Working Roller Tables", ServiceFactor.LoadCondition.HeavyShockLoad));
            _items.Add(new ServiceFactor("Medium Section Mills", ServiceFactor.LoadCondition.HeavyShockLoad));
            _items.Add(new ServiceFactor("Continuous Slabbing and Booming Mills", ServiceFactor.LoadCondition.HeavyShockLoad));
            _items.Add(new ServiceFactor("Continuous Heavy Tube Mills", ServiceFactor.LoadCondition.HeavyShockLoad));
            _items.Add(new ServiceFactor("Blowers - Heavy Duty", ServiceFactor.LoadCondition.HeavyShockLoad));

            _items.Add(new ServiceFactor("Breast Roller Drives", ServiceFactor.LoadCondition.ExtremeShockLoad));
            _items.Add(new ServiceFactor("Wrapper Roller Drives", ServiceFactor.LoadCondition.ExtremeShockLoad));
            _items.Add(new ServiceFactor("Reversing Working Roller Tables", ServiceFactor.LoadCondition.ExtremeShockLoad));
            _items.Add(new ServiceFactor("Reversing Slabbing and Booming Mills", ServiceFactor.LoadCondition.ExtremeShockLoad));
            _items.Add(new ServiceFactor("Scale Breakers", ServiceFactor.LoadCondition.ExtremeShockLoad));
            _items.Add(new ServiceFactor("Vibration Conveyors", ServiceFactor.LoadCondition.ExtremeShockLoad));
        }
    }
}
