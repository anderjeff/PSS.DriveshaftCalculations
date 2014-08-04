using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PSS.Common;

namespace PSS.DriveshaftCalculations.DriveshaftParts
{
    [Serializable]
    public class DriveshaftLayout : ICloneable, INotifyPropertyChanged, IEquatable<DriveshaftLayout>, IPersistXml
    {
        public DriveshaftLayout()
        {
            _driveshafts = new BindingList<Driveshaft>();
            _driveshafts.RaiseListChangedEvents = true;

            _uJoints = new BindingList<UniversalJoint>();
            _uJoints.RaiseListChangedEvents = true;
        }

        public DriveshaftLayout(int qty, double rpm)
            : this()
        {
            try
            {
                //initialize driveshafts and u-joints.
                AddDriveshaftsToList(qty);

                //save the speed of the layout for the accelerations.
                _rpm = rpm;

                //sets the acceleration values.
                ProcessLayoutValues();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [Browsable(false)]
        [NonSerialized]
        public const double INTERTIAL_LIMIT_NORMAL = 1000;

        [Browsable(false)]
        [NonSerialized]
        public const double INERTIAL_LIMIT_MAX = 2000;

        [Browsable(false)]
        [NonSerialized]
        public const double TORSIONAL_LIMIT_NORMAL = 300;

        [Browsable(false)]
        [NonSerialized]
        public const double TORSIONAL_LIMIT_MAX = 500;


        //the inertial drive multiplier modifiers.
        private List<int> _driveModifiers;
        public List<int> DriveModifiers
        {
            get 
            {
                return _driveModifiers;
            }
        }

        //the inertial coast multiplier modifiers.
        private List<int> _coastModifiers;
        public List<int> CoastModifiers
        {
            get
            {
                return _coastModifiers;
            }
        }

        //Handles change events for a driveshaft layout.
        public delegate void DriveshaftLayoutChangedEventHandler(DriveshaftLayout layout);

        //individual properties change.
        [field: NonSerialized] //this attribute prevents an exception.
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        //raised when the layout changes.
        [field: NonSerialized] //this attribute prevents an exception if it serializes.
        public event DriveshaftLayoutChangedEventHandler LayoutChanged;

        private BindingList<Driveshaft> _driveshafts;
        /// <summary>
        /// A list of driveshaft objects.
        /// </summary>
        [Browsable(false)]
        public BindingList<Driveshaft> Driveshafts
        {
            get { return _driveshafts; }
            set { _driveshafts = value; }
        }

        private BindingList<UniversalJoint> _uJoints;
        /// <summary>
        /// A collection of u-joint objects.
        /// </summary>
        [Browsable(false)]
        public BindingList<UniversalJoint> UJoints
        {
            get { return _uJoints; }
            set { _uJoints = value; }
        }

        private double _rpm;
        /// <summary>
        /// The rpm of the layout.
        /// </summary>
        public double Rpm
        {
            get { return _rpm; }
            set
            {
                if (_rpm != value)
                {
                    _rpm = value;
                    ListValueChanged(this, new PropertyChangedEventArgs("Rpm"));
                    NotifyPropertyChanged("Rpm"); 
                }
            }
        }

        [Browsable(false)]
        public double AngularVelocity 
        {
            get
            {
                return Calculations.Accelerations.AngularSpeed(_rpm);
            }
        }

        private double _inertiaDriveAngle;
        [DisplayName("Drive Angle (°)")]
        [Browsable(false)]
        public double InertiaDriveAngle
        {
            get
            {
                return _inertiaDriveAngle;
            }
            set
            {
                if (_inertiaDriveAngle != value)
                {
                    _inertiaDriveAngle = value;
                    NotifyPropertyChanged("InertiaDriveAngle"); 
                }
            }
        }

        private double _inertiaCoastAngle;
        [DisplayName("Coast Angle (°)")]
        [Browsable(false)]
        public double InertiaCoastAngle
        {
            get
            {
                return _inertiaCoastAngle;
            }
            set
            {
                if (_inertiaCoastAngle != value)
                {
                    _inertiaCoastAngle = value;
                    NotifyPropertyChanged("InertiaCoastAngle"); 
                }
            }
        }

        private double _torsionalAngle;
        [DisplayName("Torsional Angle (°)")]
        [Browsable(false)]
        public double TorsionalAngle
        {
            get
            {
                return _torsionalAngle;
            }
            set
            {
                if (_torsionalAngle != value)
                {
                    _torsionalAngle = value;
                    NotifyPropertyChanged("TorsionalAngle"); 
                }
            }
        }

        [DisplayName("Drive (rad/sec^2)")]
        public double InertialDrive
        {
            get
            {
                return Calculations.Accelerations.InertialDrive(InertiaDriveAngle, _rpm);
            }
        }

        [DisplayName("Coast (rad/sec^2)")]
        public double InertialCoast
        {
            get
            {
                return Calculations.Accelerations.InertialCoast(InertiaCoastAngle, _rpm);
            }

        }

        [DisplayName("Torsional (rad/sec^2)")]
        public double Torsional
        {
            get
            {
                return Calculations.Accelerations.Torsional(TorsionalAngle, _rpm);
            }
        }

        [DisplayName("Type Of Phasing")]
        public string TypeOfPhasing
        {
            get
            {
                return this.ToString();
            }
        }


        private void ProcessLayoutValues()
        {
            try
            {
                //the plus and minus modifiers.
                _driveModifiers = new List<int>();
                _coastModifiers = new List<int>();

                //sets up the drive and coast modifiers.
                SetupModifiers(_driveshafts);

                //get the relavent angles.
                GetInertiaDriveAngle();
                GetInertiaCoastAngle();
                GetTorsionalAngle();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void AddDriveshaftsToList(int qty)
        {
            try
            {
                for (int i = 0; i < qty; i++)
                {
                    Driveshaft d = new Driveshaft();
                    d.Name = string.Format("Driveshaft {0}", i + 1);

                    //listen for changes
                    d.PropertyChanged += new PropertyChangedEventHandler(ListValueChanged);

                    _driveshafts.Add(d);
                }

                if (qty > 0)
                {
                    for (int j = 0; j <= qty; j++)
                    {
                        UniversalJoint uj = new UniversalJoint();
                        uj.Name = string.Format("Joint {0}", j + 1);
                        uj.JointAngle = 0;

                        //listen for changes.
                        uj.PropertyChanged += new PropertyChangedEventHandler(ListValueChanged);

                        _uJoints.Add(uj);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ListValueChanged(object sender, PropertyChangedEventArgs e)
        {
            ProcessLayoutValues();

            DriveshaftLayoutChangedEventHandler temp = this.LayoutChanged;
            if (temp != null)
            {
                LayoutChanged(this);
            }
        }


        private void SetupModifiers(BindingList<Driveshaft> driveshafts)
        {
            /*
             See Elbe catalog page 175 for an explanation. 
             1.  crossed means same fork position --[---[-- and a positive 1.
             2.  parallel (not crossed) means opposite fork position --[-----]--- and a negative 1.
             */

            try
            {
                int driveModIndex = 1;

                //look at it from the drive side.
                for (int i = 0; i < driveshafts.Count; i++)
                {
                    if (i == 0) //first time through, need to add two values.
                    {
                        //add for joint 1
                        _driveModifiers.Add(1);
                        _driveModifiers.Add(driveshafts[i].Multiplier);
                    }
                    else
                    {
                        _driveModifiers.Add(driveshafts[i].Multiplier * _driveModifiers[driveModIndex]);
                        driveModIndex++;
                    }
                }


                int coastModIndex = 1;

                //look at it from the coast side.
                for (int j = driveshafts.Count; j > 0; j--)
                {
                    if (j == driveshafts.Count)
                    {
                        _coastModifiers.Add(1);
                        _coastModifiers.Add(driveshafts[j - 1].Multiplier);
                    }
                    else
                    {
                        _coastModifiers.Add(driveshafts[j - 1].Multiplier * _coastModifiers[coastModIndex]);
                        coastModIndex++;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private void GetInertiaDriveAngle()
        {
            try
            {
                double inertiaDriveSqrd = 0;
                int counter = 0;
                int numJoints = _uJoints.Count;

                for (int i = 1; i < numJoints; i++)
                {
                    inertiaDriveSqrd += (numJoints - i) * _driveModifiers[counter] * Math.Pow(_uJoints[counter].JointAngle, 2);
                    counter++;
                }

                //from SAE AE-7 page 50.
                InertiaDriveAngle = Math.Sqrt(Math.Abs(inertiaDriveSqrd));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void GetInertiaCoastAngle()
        {
            try
            {
                double inertiaCoastSqrd = 0;
                int counter = 0;
                int numJoints = _uJoints.Count;

                for (int i = numJoints; i > 0; i--)
                {
                    inertiaCoastSqrd += (numJoints - (counter + 1)) *
                                        _coastModifiers[counter] *
                                        Math.Pow(_uJoints[numJoints - (counter + 1)].JointAngle, 2);

                    counter++;
                }

                //from SAE AE-7 page 50.
                InertiaCoastAngle = Math.Sqrt(Math.Abs(inertiaCoastSqrd));
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetTorsionalAngle()
        {
            try
            {
                double torsAngSqrd = 0;

                for (int i = 0; i < _uJoints.Count; i++)
                {
                    torsAngSqrd += Math.Pow(_uJoints[i].JointAngle, 2) * _driveModifiers[i];
                }

                TorsionalAngle = Math.Sqrt(Math.Abs(torsAngSqrd));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string PhaseType(bool isCrossed)
        {
            if (isCrossed)
            {
                return "Crossed";
            }
            else
            {
                return "Parallel";
            }
        }

        /// <summary>
        /// Returns a deep copy of a driveshaft layout object.
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            try
            {
                DriveshaftLayout copy = new DriveshaftLayout();

                foreach (Driveshaft d in Driveshafts)
                {
                    Driveshaft dCopy = (Driveshaft)d.Clone();
                    dCopy.PropertyChanged += new PropertyChangedEventHandler(ListValueChanged);
                    copy.Driveshafts.Add(dCopy);
                }

                foreach (UniversalJoint uj in UJoints)
                {
                    UniversalJoint ujCopy = (UniversalJoint)uj.Clone();
                    ujCopy.PropertyChanged += new PropertyChangedEventHandler(ListValueChanged);
                    copy.UJoints.Add(ujCopy);
                }

                copy.InertiaCoastAngle = this.InertiaCoastAngle;
                copy.InertiaDriveAngle = this.InertiaDriveAngle;
                copy.TorsionalAngle = this.TorsionalAngle;
                copy.Rpm = this.Rpm;

                return copy;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public override string ToString()
        {
            string temp = "";

            for (int i = 0; i < Driveshafts.Count; i++)
            {
                if (i == 0)
                {
                    temp += PhaseType(Driveshafts[i].IsCrossed);
                }
                else
                {
                    temp += string.Format("-{0}", PhaseType(Driveshafts[i].IsCrossed));
                }
            }

            return temp;
        }

        public override bool Equals(object obj)
        {
            DriveshaftLayout dLay = obj as DriveshaftLayout;
            if (dLay != null)
            {
                return this.Equals(dLay);
            }
            else
            {
                return false;
            }
        }

        public bool Equals(DriveshaftLayout other)
        {
            try
            {
                if (other == null)
                {
                    return false;
                }

                bool drvListTest = true;
                bool ujtListTest = true;

                for (int i = 0; i < this.Driveshafts.Count; i++)
                {
                    drvListTest = this.Driveshafts[i].Equals(other.Driveshafts[i]);
                    if (drvListTest == false)
                    {
                        //no need to check more, we found one not equal.
                        return false;
                    }                    
                }

                for (int j = 0; j < this.UJoints.Count; j++)
                {
                    ujtListTest = this.UJoints[j].Equals(other.UJoints[j]);
                    if (ujtListTest == false)
                    {
                        //no need to check more, we found one not equal.
                        return false;
                    }
                }                

                return this.InertiaCoastAngle.Equals(other.InertiaCoastAngle) &&
                       this.InertiaDriveAngle.Equals(other.InertiaDriveAngle) &&
                       this.InertialCoast.Equals(other.InertialCoast) &&
                       this.InertialDrive.Equals(other.InertialDrive) &&
                       this.Rpm.Equals(other.Rpm) &&
                       this.Torsional.Equals(other.Torsional) &&
                       this.TorsionalAngle.Equals(other.TorsionalAngle) &&
                       this.TypeOfPhasing.Equals(other.TypeOfPhasing) &&
                       drvListTest &&
                       ujtListTest &&
                       this.Driveshafts.Count.Equals(other.Driveshafts.Count) &&
                       this.UJoints.Count.Equals(other.UJoints.Count);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        #region IPersistXmlMethods
        
        public void Load(System.Xml.XmlElement ele)
        {
            //load the driveshafts.
            Driveshafts = new BindingList<Driveshaft>();
            Driveshafts.RaiseListChangedEvents = true;

            List<Driveshaft> tempDriveshafts = (List<Driveshaft>)XML.Open_List<Driveshaft>(ele, "Driveshafts", new List<Driveshaft>());
            foreach (Driveshaft d in tempDriveshafts)
            {
                d.PropertyChanged += new PropertyChangedEventHandler(ListValueChanged);
                Driveshafts.Add(d);
            }

            //load the u-joints.
            UJoints = new BindingList<UniversalJoint>();
            UJoints.RaiseListChangedEvents = true;

            List<UniversalJoint> tempUjoint = (List<UniversalJoint>)XML.Open_List<UniversalJoint>(ele, "UJoints", new List<UniversalJoint>());
            foreach (UniversalJoint uj in tempUjoint)
            {
                uj.PropertyChanged += new PropertyChangedEventHandler(ListValueChanged);
                UJoints.Add(uj);
            }

            //load the sign modifiers
            _driveModifiers = (List<int>)XML.Open_List_int(ele, "_driveModifiers", new List<int>());
            _coastModifiers = (List<int>)XML.Open_List_int(ele, "_coastModifiers", new List<int>());

            //Load the rest of the properties.
            Rpm = (double)XML.Open_dbl(ele, "Rpm", 0.0);
            InertiaDriveAngle = (double)XML.Open_dbl(ele, "InertiaDriveAngle", 0.0);
            InertiaCoastAngle = (double)XML.Open_dbl(ele, "InertiaCoastAngle", 0.0);
            TorsionalAngle = (double)XML.Open_dbl(ele, "TorsionalAngle", 0.0);
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save_List<Driveshaft>(ele, "Driveshafts", Driveshafts.ToList<Driveshaft>());
            XML.Save_List<UniversalJoint>(ele, "UJoints", UJoints.ToList<UniversalJoint>());
            XML.Save(ele, "_driveModifiers", _driveModifiers);
            XML.Save(ele, "_coastModifiers", _coastModifiers);
            XML.Save(ele, "Rpm", Rpm);
            XML.Save(ele, "InertiaDriveAngle", InertiaDriveAngle);
            XML.Save(ele, "InertiaCoastAngle", InertiaCoastAngle);
            XML.Save(ele, "TorsionalAngle", TorsionalAngle);
        }
 
        #endregion
    }
}
