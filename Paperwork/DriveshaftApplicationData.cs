using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PSS.DriveshaftCalculations.DutyCycle;
using PSS.DriveshaftCalculations.Collections;
using PSS.Common;

namespace PSS.DriveshaftCalculations.Paperwork
{
    [Serializable]
    public class DriveshaftApplicationData : INotifyPropertyChanged, IPersistXml
    {
        public DriveshaftApplicationData()
        {
            Changes = new BindingList<Change>();
            Changes.RaiseListChangedEvents = true;

            Documents = new BindingList<SupportingDocument>();
            Documents.RaiseListChangedEvents = true;

            SelectedUjoints = new BindingList<SelectedUjoint>();
            SelectedUjoints.RaiseListChangedEvents = true;

            SelectedDriveshafts = new SelectedDriveshaftCollection();
           
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private string _designCode;
        /// <summary>
        /// A unique number given to this application data, to identify the current design.
        /// </summary>
        public string DesignCode
        {
            get { return _designCode; }
            set 
            {
                if (_designCode != value)
                {
                    _designCode = value;
                    NotifyPropertyChanged("DesignCode");
                }            
            }
        }

        private string _shaftLocation;
        /// <summary>
        /// The location of this driveshaft, usually summarizing the two components it connects, 
        /// for example, Engine to Transmission.
        /// </summary>
        public string ShaftLocation
        {
            get { return _shaftLocation; }
            set
            {
                if (_shaftLocation != value)
                {
                    _shaftLocation = value;
                    NotifyPropertyChanged("ShaftLocation");
                }
            }
        }
        

        private string _partNumber;
        /// <summary>
        /// The part number this application was done for.
        /// </summary>
        public string PartNumber
        {
            get { return _partNumber; }
            set
            {
                if (_partNumber != value)
                {
                    _partNumber = value;
                    NotifyPropertyChanged("PartNumber");
                }                
            }
        }

        private string _customer;
        public string Customer
        {
            get { return _customer; }
            set
            {
                _customer = value;
                NotifyPropertyChanged("Customer");
            }
        }

        private string _customerContact;
        public string CustomerContact
        {
            get { return _customerContact; }
            set
            {
                _customerContact = value;
                NotifyPropertyChanged("CustomerContact");
            }
        }

        private string _modelNumber;
        public string ModelNumber
        {
            get { return _modelNumber; }
            set
            {
                _modelNumber = value;
                NotifyPropertyChanged("ModelNumber");
            }
        }

        private string _createdBy;
        /// <summary>
        /// The person that created the form.
        /// </summary>
        public string CreatedBy
        {
            get { return _createdBy; }
            set
            {
                _createdBy = value;
                NotifyPropertyChanged("CreatedBy");
            }
        }

        private string _lastSavedBy;
        /// <summary>
        /// The last person to save this form.
        /// </summary>
        public string LastSavedBy
        {
            get { return _lastSavedBy; }
            set
            {
                _lastSavedBy = value;
                NotifyPropertyChanged("LastSavedBy");
            }
        }

        private DateTime _lastModified;
        /// <summary>
        /// The time this was last modified.
        /// </summary>
        public DateTime LastModified
        {
            get { return _lastModified; }
            set
            {
                _lastModified = value;
                NotifyPropertyChanged("LastModified");
            }
        }

        private string _applicationDescription;

        public string ApplicationDescription
        {
            get { return _applicationDescription; }
            set
            {
                _applicationDescription = value;
                NotifyPropertyChanged("ApplicationDescription");
            }
        }


        /// <summary>
        /// A list of changes made to the application.
        /// </summary>
        public BindingList<Change> Changes { get; set; }

        /// <summary>
        /// A list of supporting documents.
        /// </summary>
        public BindingList<SupportingDocument> Documents { get; set; }

        /// <summary>
        /// A list of u-joints that have been selected in the final design.
        /// </summary>
        public BindingList<SelectedUjoint> SelectedUjoints { get; set; }

        /// <summary>
        /// The driveshaft collection that represents the final designs.
        /// </summary>
        public SelectedDriveshaftCollection SelectedDriveshafts { get; set; }

        /// <summary>
        /// Reattach the correct DutyCycleConditionCollection object to the selected UJoint.
        /// </summary>
        /// <param name="dtyCycColl"></param>
        public void RebindSelectedUJoints(DutyCycle.DutyCycleCollection dtyCycColl)
        {
            foreach (DutyCycleConditionCollection dccc in dtyCycColl)
            {
                foreach (SelectedUjoint selUj in SelectedUjoints)
                {
                    if (selUj.UjointName == dccc.JointName)
                    {
                        selUj.ConditionCollection = dccc;
                    }                
                }
            }
        }

        #region IPerisistXmlMethods
        
        public void Load(System.Xml.XmlElement ele)
        {
            CreatedBy = (string)XML.Open_str(ele, "CreatedBy", string.Empty);
            Customer = (string)XML.Open_str(ele, "Customer", string.Empty);
            CustomerContact = (string)XML.Open_str(ele, "CustomerContact", string.Empty);
            DesignCode = (string)XML.Open_str(ele, "DesignCode", string.Empty);
            LastModified = (DateTime)XML.Open_DateTime(ele, "LastModified", DateTime.Now);
            LastSavedBy = (string)XML.Open_str(ele, "LastSavedBy", string.Empty);
            ModelNumber = (string)XML.Open_str(ele, "ModelNumber", string.Empty);
            PartNumber = (string)XML.Open_str(ele, "PartNumber", string.Empty);
            ShaftLocation = (string)XML.Open_str(ele, "ShaftLocation", string.Empty);

            ApplicationDescription = (string)XML.Open_str(ele, "ApplicationDescription", string.Empty);
            Changes = new BindingList<Change>((List<Change>)XML.Open_List<Change>(ele, "Changes", new List<Change>()));
            Documents = new BindingList<SupportingDocument>((List<SupportingDocument>)XML.Open_List<SupportingDocument>(ele, "Documents", new List<SupportingDocument>()));
            SelectedUjoints = new BindingList<SelectedUjoint>((List<SelectedUjoint>)XML.Open_List<SelectedUjoint>(ele, "SelectedUjoints", new List<SelectedUjoint>()));
            SelectedDriveshafts = (SelectedDriveshaftCollection)XML.Open_IPersistXml(ele, "SelectedDriveshafts", new SelectedDriveshaftCollection());
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "CreatedBy", CreatedBy);
            XML.Save(ele, "Customer", Customer);
            XML.Save(ele, "CustomerContact", CustomerContact);
            XML.Save(ele, "DesignCode", DesignCode);
            XML.Save(ele, "LastModified", LastModified);
            XML.Save(ele, "LastSavedBy", LastSavedBy);
            XML.Save(ele, "ModelNumber", ModelNumber);
            XML.Save(ele, "PartNumber", PartNumber);
            XML.Save(ele, "ShaftLocation", ShaftLocation);

            XML.Save(ele, "ApplicationDescription", ApplicationDescription);
            XML.Save_List<Change>(ele, "Changes", Changes.ToList<Change>());
            XML.Save_List<SupportingDocument>(ele, "Documents", Documents.ToList<SupportingDocument>());
            XML.Save_List<SelectedUjoint>(ele, "SelectedUjoints", SelectedUjoints.ToList<SelectedUjoint>());
            XML.Save(ele, "SelectedDriveshafts", SelectedDriveshafts);
        } 

        #endregion
    }
}
