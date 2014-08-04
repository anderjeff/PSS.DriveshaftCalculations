using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using PSS.Common;
using PSS.DriveshaftCalculations.DriveshaftParts;


namespace PSS.DriveshaftCalculations.Analysis
{
    /// <summary>
    /// Represents the results of a particular analysis on a driveshaft series.
    /// </summary>
    [Serializable]
    public class AnalysisResult : IPersistXml
    {
        public AnalysisResult()
        {
        }

        public AnalysisResult(DriveshaftSeries series, double hours)
        {
            Series = series;
            Hours = hours;
            IsPicked = false;
        }

        [Browsable(false)]
        public DriveshaftParts.DriveshaftSeries Series { get; set; }

        /// <summary>
        /// The common name of the driveshaft series.
        /// </summary>
        [DisplayName("Series")]
        public string SeriesName
        {
            get { return Series.Name; }
        }

        /// <summary>
        /// The bearing life of this series.
        /// </summary>
        public double Hours { get; set; }

        private string _comment;
        /// <summary>
        /// A comment about this result.
        /// </summary>
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }

        /// <summary>
        /// Indicates this is the selected series.
        /// </summary>
        [Browsable(false)]
        public bool IsPicked { get; set; }

        #region IPersistXml Methods
        public void Load(System.Xml.XmlElement ele)
        {
            Series = (DriveshaftSeries)XML.Open_IPersistXml(ele, "Series", new DriveshaftSeries());
            Hours = (double)XML.Open_dbl(ele, "Hours", 0.0);
            Comment = (string)XML.Open_str(ele, "Comment", string.Empty);
            IsPicked = (bool)XML.Open_bln(ele, "IsPicked", false);
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "Series", Series);
            XML.Save(ele, "Hours", Hours);
            XML.Save(ele, "Comment", Comment);
            XML.Save(ele, "IsPicked", IsPicked);
        } 
        #endregion
    }
}
