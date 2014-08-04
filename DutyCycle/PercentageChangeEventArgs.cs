using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSS.DriveshaftCalculations.DutyCycle
{
    public class PercentageChangeEventArgs : EventArgs
    {
        public PercentageChangeEventArgs(string propertyName, double oldValue, double newValue)
        {
            PropertyName = propertyName;
            OldValue = oldValue;
            NewValue = newValue;
        }

        public string PropertyName { get; set; }
        public double OldValue { get; set; }
        public double NewValue { get; set; }
    }
}
