using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PSS.Common;

namespace PSS.DriveshaftCalculations.Calculations
{
    /// <summary>
    /// Power factor is a design factor that depends on the source of the driving power. 
    /// It is used to determine the equivalent torque on the driveshaft.
    /// </summary>
    [Serializable]
    public class PowerFactor : IPersistXml
    {
        public enum Fp
        {
            Electric,
            Gasoline,
            Diesel
        }

        private Fp _factor;
        /// <summary>
        /// The factor enum for this object
        /// </summary>
        public Fp Factor
        {
            get { return _factor; }
            set { _factor = value; }
        }

        /// <summary>
        /// These values come from page 3, table 1 of the IJ900 product catalog, 
        /// G:\Engineering\Design\Standards\IJ900-02.pdf
        /// </summary>
        public double Value
        {
            get
            {
                if (_factor == PowerFactor.Fp.Electric)
                {
                    return 1.0;
                }
                else if (_factor == PowerFactor.Fp.Gasoline)
                {
                    return 1.20;
                }
                else
                {
                    return 1.25;
                }
            }
        }



        #region IPersistXmlMethods

        public void Load(System.Xml.XmlElement ele)
        {
            Factor = (PowerFactor.Fp)XML.Open_Enum<PowerFactor.Fp>(ele, "Factor", Fp.Electric);
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save_Enum<PowerFactor.Fp>(ele, "Factor", Factor);
        } 

        #endregion
    }
}
