using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


namespace PSS.DriveshaftCalculations.Calculations
{
    /// <summary>
    /// Represents the maximum low gear torque for a mobile application.
    /// </summary>
    class LowGearTorque : INotifyPropertyChanged
    {
        public LowGearTorque()
        {
            //initialize ratios.
            TorqueConverterStallRatio = 1.0f;
            TransferCaseRatio = 1.0f;
            TransLowRatioFwd = 1.0f;
        }

        public enum TransmissionType
        {
            Automatic,
            Manual
        }

        #region Properties
        private double _grossEngineTorque;
        /// <summary>
        /// The maximum engine torque output considering only losses from ancillaries
        /// such as fuel pump, oil pump and water pump.  Published engine torque values 
        /// are gross engine torque.
        /// </summary>
        public double GrossEngineTorque
        {
            get { return _grossEngineTorque; }
            set 
            {
                if (_grossEngineTorque != value)
                {
                    _grossEngineTorque = value;
                    OnPropertyChanged("GrossEngineTorque");                    
                }
            }
        }

        private double _transLowRatioFwd;
        /// <summary>
        /// The low gear ratio of the transmission in the forward direction.
        /// </summary>
        public double TransLowRatioFwd
        {
            get { return _transLowRatioFwd; }
            set 
            {
                if (_transLowRatioFwd != value)
                {
                    _transLowRatioFwd = value;
                    OnPropertyChanged("TransLowRatioFwd");                    
                }
            }
        }

        private TransmissionType _transType;
        /// <summary>
        /// Indicates a manual or automatic transmission. Also determines the
        /// transmission efficiency.
        /// </summary>
        public TransmissionType TransType
        {
            get { return _transType; }
            set 
            {
                if (_transType != value)
                {
                    _transType = value;
                    if (value == TransmissionType.Automatic)
                    {
                        TransEfficiency = 0.8f;
                    }
                    else
                    {
                        TransEfficiency = 0.85f;
                    }

                    OnPropertyChanged("TransType");
                }
            }
        }

        private float _transEfficiency;
        /// <summary>
        /// The efficiency of the transmission.
        /// </summary>
        public float TransEfficiency
        {
            get { return _transEfficiency; }
            private set { _transEfficiency = value; }
        }

        private double _tcStallRatio;
        /// <summary>
        /// If applicable, the stall ratio for the torque converter. Note only 
        /// automatic transmissions have a torque converter stall ration.
        /// </summary>
        public double TorqueConverterStallRatio
        {
            get { return _tcStallRatio; }
            set 
            {
                if (_tcStallRatio != value)
                {
                    _tcStallRatio = value;
                    OnPropertyChanged("TorqueConverterStallRatio");
                }
            }
        }

        private double _tcRatio;
        /// <summary>
        /// The gear ratio of the transfer case, 1.25:1 is written as 1.25.
        /// </summary>
        public double TransferCaseRatio
        {
            get { return _tcRatio; }
            set 
            {
                if (_tcRatio != value)
                {
                    _tcRatio = value;
                    OnPropertyChanged("TransferCaseRatio");
                }
            }
        }

        /// <summary>
        /// The efficiency of the transfer case, if applicable.
        /// </summary>
        public double TransferCaseEfficiency
        {
            get { return 0.95; }
        }

        private double _axleCapacity;
        /// <summary>
        /// The weight capacity of the axle, in pounds.
        /// </summary>
        public double AxleCapacity
        {
            get { return _axleCapacity; }
            set 
            {
                if (_axleCapacity != value)
                {
                    _axleCapacity = value;
                    OnPropertyChanged("AxleCapacity");
                }
            }
        }

        private double _tireRollingRadius;
        /// <summary>
        /// The rolling radius of the tire.
        /// </summary>
        public double TireRollingRadius
        {
            get { return _tireRollingRadius; }
            set 
            {
                if (_tireRollingRadius != value)
                {
                    _tireRollingRadius = value;
                    OnPropertyChanged("TireRollingRadius");
                }            
            }
        }

        private float _axleRatio;
        /// <summary>
        /// The ratio of the axle, 4.21:1 would be entered as 4.21.
        /// </summary>
        public float AxleRatio
        {
            get { return _axleRatio; }
            set 
            {
                if (_axleRatio != value)
                {
                    _axleRatio = value;
                    OnPropertyChanged("AxleRatio");
                }            
            }
        }

        private int _grossVehicleWeight;
        /// <summary>
        /// The gross weight of the vehicle.
        /// </summary>
        public int GrossVehicleWeight
        {
            get { return _grossVehicleWeight; }
            set 
            {
                if (_grossVehicleWeight != value)
                {
                    _grossVehicleWeight = value;
                    OnPropertyChanged("GrossVehicleWeight");
                }            
            }
        }

        private float _overallLowRatio;
        /// <summary>
        /// The overall low gear ratio combines transmission low gear ratio, 
        /// torque converter stall ratio, and transfer case ratio.
        /// </summary>
        public float OverallLowGearRatio
        {
            get { return _overallLowRatio; }
            set 
            {
                if (_overallLowRatio != value)
                {
                    _overallLowRatio = value;
                    OnPropertyChanged("OverallLowGearRatio");
                }            
            }
        }
        
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }

        #region Private Methods

        private void Calculate()
        {
            try
            {
                //Step 1: Low Gear Torque Calculation
                //Step 2: Wheel Slip Calculation
                //Step 3: Gradability Calculation
            }
            catch (Exception)
            {
                throw;
            }
        } 

        #endregion
    }
}
