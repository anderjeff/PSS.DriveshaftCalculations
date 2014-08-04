using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Diagnostics;
using System.Xml;


using PSS.DriveshaftCalculations.DriveshaftParts;

namespace PSS.DriveshaftCalculations.Data
{
    public class DataLoader
    {        
        /// <summary>
        /// The path to the location of the data file.
        /// </summary>
        public static string DataFilePath
        {
            get
            {
                if (File.Exists(Properties.Settings.Default.DataFilePath))
                {
                    return Properties.Settings.Default.DataFilePath;
                }
                else
                {
                    return  System.Deployment.Application.ApplicationDeployment.CurrentDeployment.DataDirectory + "\\DataFiles\\TubeSizes.xml";
                }
            }
        }

        /// <summary>
        /// The path to the location of old logged data files.
        /// </summary>
        public static string LoggedDataPath
        {
            get
            {
                if (Directory.Exists(Properties.Settings.Default.LoggedDataFileDirectory))
                {
                    return Properties.Settings.Default.LoggedDataFileDirectory;
                }
                else
                {
                    //note caller is checking if the directory exists.
                    return string.Empty;
                }
            }
        }

        internal static List<DriveshaftSeries> AllDriveshaftSeries()
        {
            try
            {
                List<DriveshaftSeries> allSeries = new List<DriveshaftSeries>();

                //kind of fudged the 1000, 1100 and 1280 series numbers, hard to find values.
                allSeries.Add(new EnglishDriveshaftSeries("1000", 420, 310, 21, 160, 420, DriveshaftSeries.TypeOfSeries.RoundBearing, 6000));
                allSeries.Add(new EnglishDriveshaftSeries("1100", 670, 331, 25, 191, 670, DriveshaftSeries.TypeOfSeries.RoundBearing, 6000));
                allSeries.Add(new EnglishDriveshaftSeries("1280", 1250, 570, 31, 250, 1250, DriveshaftSeries.TypeOfSeries.RoundBearing, 6000));

                //rest come from from Spicer Catalog.
                allSeries.Add(new EnglishDriveshaftSeries("1310", 1100, 1100, 62, 466, 1600, DriveshaftSeries.TypeOfSeries.RoundBearing, 6000));
                allSeries.Add(new EnglishDriveshaftSeries("1350", 1790, 1580, 94, 707, 2260, DriveshaftSeries.TypeOfSeries.RoundBearing, 5000));
                allSeries.Add(new EnglishDriveshaftSeries("1410", 2160, 1580, 110, 851, 2700, DriveshaftSeries.TypeOfSeries.RoundBearing, 5000));
                allSeries.Add(new EnglishDriveshaftSeries("1480", 2890, 1800, 130, 1119, 3330, DriveshaftSeries.TypeOfSeries.RoundBearing, 5000));
                allSeries.Add(new EnglishDriveshaftSeries("1550", 3720, 2280, 170, 1401, 4400, DriveshaftSeries.TypeOfSeries.RoundBearing, 5000));
                allSeries.Add(new EnglishDriveshaftSeries("1610", 5740, 3450, 240, 2360, 6500, DriveshaftSeries.TypeOfSeries.RoundBearing, 4500));
                allSeries.Add(new EnglishDriveshaftSeries("1710", 7610, 4570, 330, 3176, 8000, DriveshaftSeries.TypeOfSeries.RoundBearing, 4500));
                allSeries.Add(new EnglishDriveshaftSeries("1760", 10150, 4570, 360, 3527, 10200, DriveshaftSeries.TypeOfSeries.RoundBearing, 4500));
                allSeries.Add(new EnglishDriveshaftSeries("1810", 11060, 5850, 430, 4144, 12000, DriveshaftSeries.TypeOfSeries.RoundBearing, 4500));
                allSeries.Add(new EnglishDriveshaftSeries("1880", 16210, 10380, 500, 4842, 16000, DriveshaftSeries.TypeOfSeries.RoundBearing, 3000));

                //from Spicer catalog, and Off-Highway driveshafts book, Italcardano website.
                allSeries.Add(new EnglishDriveshaftSeries("2C", 590, 590, 64, 479, 1106, DriveshaftSeries.TypeOfSeries.WingBearing, 6000));
                //had to guess at 3C..., so not using for now.
                //allSeries.Add(new EnglishDriveshaftSeries("3C", 850, 850, 100, 800, 2200));
                allSeries.Add(new EnglishDriveshaftSeries("4C", 1110, 900, 140, 1033, 2434, DriveshaftSeries.TypeOfSeries.WingBearing, 5000));
                allSeries.Add(new EnglishDriveshaftSeries("5C", 1950, 1570, 190, 1475, 4130, DriveshaftSeries.TypeOfSeries.WingBearing, 5000));
                allSeries.Add(new EnglishDriveshaftSeries("6C", 2510, 2370, 230, 1918, 5310, DriveshaftSeries.TypeOfSeries.WingBearing, 5000));
                allSeries.Add(new EnglishDriveshaftSeries("7C", 4200, 3880, 290, 2508, 7892, DriveshaftSeries.TypeOfSeries.WingBearing, 4500));
                allSeries.Add(new EnglishDriveshaftSeries("8C", 6270, 6270, 390, 3762, 11432, DriveshaftSeries.TypeOfSeries.WingBearing, 4500));
                allSeries.Add(new EnglishDriveshaftSeries("8.5C", 10330, 7190, 520, 5015, 14973, DriveshaftSeries.TypeOfSeries.WingBearing, 4500));
                allSeries.Add(new EnglishDriveshaftSeries("9C", 13720, 11700, 710, 6859, 20209, DriveshaftSeries.TypeOfSeries.WingBearing, 4500));
                allSeries.Add(new EnglishDriveshaftSeries("10C", 19180, 12640, 990, 9588, 29281, DriveshaftSeries.TypeOfSeries.WingBearing, 3000));
                allSeries.Add(new EnglishDriveshaftSeries("11C", 19910, 12640, 1050, 10178, 30683, DriveshaftSeries.TypeOfSeries.WingBearing, 2500));

                //pieced the 15C together.  Got MEL from Itatalcardano catalog and 
                // Off-Highway Driveshafts catalog from Dana, added 50 to the 11C industrial 
                // rating and Max net power.  Took MOH rating from Endurance torque on All-Power 
                // bearing life spreadsheet.
                allSeries.Add(new EnglishDriveshaftSeries("15C", 19960, 15830, 1200, 14120, 75400, DriveshaftSeries.TypeOfSeries.WingBearing, 2500));

                allSeries.Add(new MetricDriveshaftSeries("687.15", 0.7, 2.4, 3.1, 0.000179, DriveshaftSeries.TypeOfSeries.Metric, 4500));
                allSeries.Add(new MetricDriveshaftSeries("687.20", 1.0, 3.5, 4.6, 0.000539, DriveshaftSeries.TypeOfSeries.Metric, 4500));
                allSeries.Add(new MetricDriveshaftSeries("687.25", 1.6, 5.0, 6.5, 0.00179, DriveshaftSeries.TypeOfSeries.Metric, 4500));
                allSeries.Add(new MetricDriveshaftSeries("687.30", 1.9, 6.5, 8.5, 0.00259, DriveshaftSeries.TypeOfSeries.Metric, 4500));
                allSeries.Add(new MetricDriveshaftSeries("687.35", 2.9, 10.0, 13.0, 0.0128, DriveshaftSeries.TypeOfSeries.Metric, 4500));
                allSeries.Add(new MetricDriveshaftSeries("687.40", 4.4, 14.0, 18.2, 0.0422, DriveshaftSeries.TypeOfSeries.Metric, 4500));
                allSeries.Add(new MetricDriveshaftSeries("687.45", 5.1, 17.0, 22.1, 0.104, DriveshaftSeries.TypeOfSeries.Metric, 4500));
                allSeries.Add(new MetricDriveshaftSeries("687.55", 7.3, 25.0, 32.5, 0.236, DriveshaftSeries.TypeOfSeries.Metric, 4500));
                allSeries.Add(new MetricDriveshaftSeries("687.65", 11.0, 35.0, 45.5, 0.837, DriveshaftSeries.TypeOfSeries.Metric, 4000));
                allSeries.Add(new MetricDriveshaftSeries("587.50", 13.0, 43.0, 55.9, 1.76, DriveshaftSeries.TypeOfSeries.Metric, 4000));
                allSeries.Add(new MetricDriveshaftSeries("587.55", 18.0, 52.0, 67.6, 7.6, DriveshaftSeries.TypeOfSeries.Metric, 4000));
                allSeries.Add(new MetricDriveshaftSeries("587.60", 23.0, 57.0, 74.1, 24.8, DriveshaftSeries.TypeOfSeries.Metric, 4000));
                allSeries.Add(new MetricDriveshaftSeries("390.60", 23.0, 60.0, 78.0, 24.8, DriveshaftSeries.TypeOfSeries.Metric, 4000));
                allSeries.Add(new MetricDriveshaftSeries("390.65", 36.0, 90.0, 117.0, 70.2, DriveshaftSeries.TypeOfSeries.Metric, 3500));
                allSeries.Add(new MetricDriveshaftSeries("390.70", 53.0, 130.0, 169.0, 238.0, DriveshaftSeries.TypeOfSeries.Metric, 3500));
                allSeries.Add(new MetricDriveshaftSeries("390.75", 75.0, 190.0, 247.0, 618.0, DriveshaftSeries.TypeOfSeries.Metric, 3500));
                allSeries.Add(new MetricDriveshaftSeries("390.80", 102.0, 255.0, 331.5, 1563.0, DriveshaftSeries.TypeOfSeries.Metric, 3500));

                return allSeries;
            }
            catch (Exception)
            {
                throw;
            }
        }


        internal static List<Tube> AllTubeSizes()
        {
            try
            {
                List<Tube> allTubes = new List<Tube>();
                
                XDocument xDoc = null;
                xDoc = XDocument.Load(DataFilePath);

                XElement eleTubes = xDoc.Element("root").Element("tubes");

                IEnumerable<XElement> lstTubes = eleTubes.Elements("tube");

                allTubes = (from t
                            in lstTubes
                            select new Tube()
                            {
                                OuterDia = double.Parse(t.Attribute("od").Value),
                                Wall = double.Parse(t.Attribute("wall").Value)

                            }).ToList<Tube>();

                return allTubes;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Controls whether or not the data file is read only.
        /// </summary>
        /// <param name="isReadOnly"></param>
        public static void SetDataFileAccess(bool isReadOnly)
        {
            if (isReadOnly)
            {
                File.SetAttributes(DataFilePath, FileAttributes.ReadOnly);

            }
            else
            {
                File.SetAttributes(DataFilePath, FileAttributes.Normal);
            }
        }


    }
}
