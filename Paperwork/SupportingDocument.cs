using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PSS.Common;


namespace PSS.DriveshaftCalculations.Paperwork
{
    /// <summary>
    /// Represents a supporting document for use in design decisions.
    /// </summary>
    [Serializable]
    public class SupportingDocument : INotifyPropertyChanged, IPersistXml
    {
        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String info)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(info));
            }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set
            {
                _fileName = value;
                NotifyPropertyChanged("FileName");
            }
        }

        private string _fullPath;
        public string FullPath
        {
            get { return _fullPath; }
            set
            {
                _fullPath = value;
                NotifyPropertyChanged("FullPath");
            }
        }

        /// <summary>
        /// The file extension used by this file.
        /// </summary>
        public string FileExtension
        {
            get 
            {
                string extension = _fullPath.Split('.').Last();
                return extension; 
            }
        }
        
        public string DisplayText
        {
            get
            {
                return string.Format("{0} ({1})", FileName, FullPath);
            }
        }


        #region MyRegion

        public void Load(System.Xml.XmlElement ele)
        {
            FullPath = (string)XML.Open_str(ele, "FullPath", string.Empty);
            FileName = (string)XML.Open_str(ele, "FileName", string.Empty);
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "FullPath", FullPath);
            XML.Save(ele, "FileName", FileName);
        } 

        #endregion
    }
}
