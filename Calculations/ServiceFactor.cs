using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PSS.DriveshaftCalculations.Calculations
{
    public class ServiceFactor
    {
        public ServiceFactor(string appDescrip, LoadCondition lc)
        {
            _applicationDescription = appDescrip;
            _condition = lc;

            SetValues(_applicationDescription, _condition);
        }

        private string _applicationDescription;
        /// <summary>
        /// A description of the application.
        /// </summary>
        public string ApplicationDescription
        {
            get { return _applicationDescription; }
            set { _applicationDescription = value; }
        }

        private LoadCondition _condition;
        /// <summary>
        /// The category of load,
        /// </summary>
        public LoadCondition Condition
        {
            get { return _condition; }
            set { _condition = value; }
        }

        private double _minValue;
        /// <summary>
        /// The minimum service factor value.
        /// </summary>
        public double MinValue
        {
            get { return _minValue; }
            set { _minValue = value; }
        }

        private double _maxValue;
        /// <summary>
        /// The maximum service factor value.
        /// </summary>
        public double MaxValue
        {
            get { return _maxValue; }
            set { _maxValue = value; }
        }

        public enum LoadCondition
        {
            ContinuousLoad,
            LightShockLoad,
            MediumShockLoad,
            HeavyShockLoad,
            ExtremeShockLoad
        }

        private void SetValues(string appDescrip, LoadCondition lc)
        {
            switch (lc)
            {
                case LoadCondition.ContinuousLoad:
                    _minValue = 1.2;
                    _maxValue = 1.5;
                    break;
                case LoadCondition.LightShockLoad:
                    _minValue = 1.5;
                    _maxValue = 2.0;
                    break;
                case LoadCondition.MediumShockLoad:
                    _minValue = 2.5;
                    _maxValue = 2.5;
                    break;
                case LoadCondition.HeavyShockLoad:
                    _minValue = 3.0;
                    _maxValue = 3.0;
                    break;
                case LoadCondition.ExtremeShockLoad:
                    _minValue = 4.0;
                    _maxValue = 6.0;
                    break;
            }

        }
    }
}
