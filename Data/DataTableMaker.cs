using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.ComponentModel;

using PSS.DriveshaftCalculations.Paperwork;
using PSS.DriveshaftCalculations.DutyCycle;
using PSS.DriveshaftCalculations.DriveshaftParts;
using PSS.DriveshaftCalculations.Collections;
using PSS.DriveshaftCalculations.Analysis;
using PSS.DriveshaftCalculations.Calculations;


namespace PSS.DriveshaftCalculations.Data
{
    class DataTableMaker
    {
        /// <summary>
        /// Creates a data table for the DriveshaftApplicationData class.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static DataTable Create(DriveshaftApplicationData data)
        {
            //set up table and schema.
            DataTable tmpTable = new DataTable("Paperwork");
            tmpTable.Columns.Add(new DataColumn("DesignCode", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("ShaftLocation", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("PartNumber", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("Customer", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("ContactName", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("ApplicationDescription", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("ModelNumber", typeof(System.String)));

            if (data != null)
            {
                //create data row.
                DataRow rw = tmpTable.NewRow();
                rw["DesignCode"] = data.PartNumber as string ?? string.Empty;
                rw["ShaftLocation"] = data.PartNumber as string ?? string.Empty;
                rw["PartNumber"] = data.PartNumber as string ?? string.Empty;
                rw["Customer"] = data.Customer as string ?? string.Empty;
                rw["ContactName"] = data.CustomerContact as string ?? string.Empty;
                rw["ApplicationDescription"] = data.ApplicationDescription as string ?? string.Empty;
                rw["ModelNumber"] = data.ModelNumber as string ?? string.Empty;

                //add row to table.
                tmpTable.Rows.Add(rw); 
            }

            return tmpTable;
        }

        public static DataTable Create(DutyCycleCollection data, string series, int hours)
        {
            DataTable tmpTable = new DataTable("DutyCycle");
            tmpTable.Columns.Add(new DataColumn("UniversalJoint", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("ConditionName", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("PercentOfTime", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("Horsepower", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("Torque", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("Rpm", typeof(System.Int16)));
            tmpTable.Columns.Add(new DataColumn("JointAngle", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("ServiceFactor", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("DesiredLife", typeof(System.Int16)));
            tmpTable.Columns.Add(new DataColumn("ActualLife", typeof(System.Int16)));
            tmpTable.Columns.Add(new DataColumn("AngleFactor", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("EquivalentTorque", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("ShockLoad", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("AngleSpeed", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("ConditionDescription", typeof(System.String)));

            if (data != null)
            {
                foreach (DutyCycleConditionCollection dccc in data.JointDutyCycles)
                {
                    foreach (DutyCycleCondition dcc in dccc)
                    {
                        DataRow rw = tmpTable.NewRow();
                        rw["UniversalJoint"] = series;
                        rw["ConditionName"] = dcc.Name;
                        rw["PercentOfTime"] = dcc.PercentOfTime;
                        rw["Horsepower"] = PowerCalculations.Horsepower(dcc.OperatingCondition.Torque, dcc.OperatingCondition.Rpm);
                        rw["Torque"] = dcc.OperatingCondition.Torque;
                        rw["Rpm"] = dcc.OperatingCondition.Rpm;
                        rw["JointAngle"] = dcc.OperatingCondition.Angle;
                        rw["ServiceFactor"] = dcc.OperatingCondition.ServiceFactor;
                        rw["DesiredLife"] = dcc.OperatingCondition.DesiredLife;
                        rw["ActualLife"] = hours;
                        rw["AngleFactor"] = dcc.OperatingCondition.AngleFactor;
                        rw["EquivalentTorque"] = dcc.OperatingCondition.EquivalentTorque;
                        rw["ShockLoad"] = dcc.OperatingCondition.ShockLoad;
                        rw["AngleSpeed"] = dcc.OperatingCondition.SpeedAngle;
                        rw["ConditionDescription"] = dcc.DescriptionOfCondition;

                        tmpTable.Rows.Add(rw);
                    }
                }
            }
            

            return tmpTable;
        }

        public static DataTable Create(EnglishDriveshaftSeries data)
        {
            DataTable tmpTable = new DataTable("EnglishUniversalJoints");
            tmpTable.Columns.Add(new DataColumn("UniversalJoint", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("MinimumElasticLimit", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("IndustrialRating", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("MohRating", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("BearingCapacity", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("MaxSeriesSpeed", typeof(System.Int16)));

            if (data.Name != null)
            {
                DataRow rw = tmpTable.NewRow();
                rw["UniversalJoint"] = data.Name;
                rw["MinimumElasticLimit"] = data.MinElasticLimit;
                rw["IndustrialRating"] = data.IndustrialRating;
                rw["MohRating"] = data.MOH_Rating;
                rw["BearingCapacity"] = data.BearingCapacity;
                rw["MaxSeriesSpeed"] = data.MaxAllowableSpeed;

                tmpTable.Rows.Add(rw);
            }

            return tmpTable;
        }

        public static DataTable Create(MetricDriveshaftSeries data)
        {
            DataTable tmpTable = new DataTable("MetricUniversalJoints");
            tmpTable.Columns.Add(new DataColumn("UniversalJoint", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("YieldTorque", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("FunctionalLimitTorque", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("ReversingFatigueTorque", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("PulsatingFatigueTorque", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("BearingCapacityFactor", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("MaxSeriesSpeed", typeof(System.Int16)));

            if (data.Name != null)
            {
                DataRow rw = tmpTable.NewRow();
                rw["UniversalJoint"] = data.Name;
                rw["YieldTorque"] = data.MinElasticLimit;
                rw["FunctionalLimitTorque"] = data.FunctionalLimitTorque;
                rw["ReversingFatigueTorque"] = data.ReversingFatigueTorque;
                rw["PulsatingFatigueTorque"] = data.PulsatingFatigueTorque;
                rw["BearingCapacityFactor"] = data.BearingCapacityFactor;
                rw["MaxSeriesSpeed"] = data.MaxAllowableSpeed;

                tmpTable.Rows.Add(rw);
            }

            return tmpTable;
        }

        public static DataTable Create(CriticalSpeedCollection data)
        {
            DataTable tmpTable = new DataTable("CriticalSpeed");
            tmpTable.Columns.Add(new DataColumn("TubeSize", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("Length", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("CriticalSpeed", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("MSOS", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("HighLimitHalfCritical", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("HalfCritical", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("LowLimitHalfCritical", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("ActualSpeed", typeof(System.Int16)));

            foreach (CriticalSpeed cs in data.SavedCriticalSpeeds)
            {
                DataRow rw = tmpTable.NewRow();
                rw["TubeSize"] = cs.TubeSize;
                rw["Length"] = cs.Length;
                rw["CriticalSpeed"] = cs.Value;
                rw["MSOS"] = cs.MaxSafeOperatingSpeed;
                rw["HighLimitHalfCritical"] = cs.ErrorLimitHigh;
                rw["HalfCritical"] = cs.HalfCritical;
                rw["LowLimitHalfCritical"] = cs.ErrorLimitLow;
                rw["ActualSpeed"] = cs.Rpm;

                tmpTable.Rows.Add(rw);
            }

            return tmpTable;
        }

        public static DataTable Create(DriveshaftLayoutCollection data)
        {
            DataTable tmpTable = new DataTable("Accelerations");
            tmpTable.Columns.Add(new DataColumn("Phasing", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("Rpm", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("InertiaDrive", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("InertiaCoast", typeof(System.Double)));
            tmpTable.Columns.Add(new DataColumn("Torsional", typeof(System.Double)));

            foreach (DriveshaftLayout layout in data.SavedLayouts)
            {
                DataRow rw = tmpTable.NewRow();
                rw["Phasing"] = layout.ToString();
                rw["Rpm"] = layout.Rpm;
                rw["InertiaDrive"] = layout.InertialDrive;
                rw["InertiaCoast"] = layout.InertialCoast;
                rw["Torsional"] = layout.Torsional;

                tmpTable.Rows.Add(rw);
            }

            return tmpTable;
        }

        public static DataTable Create(BindingList<SelectedUjoint> uJoints)
        {
            DataTable tmpTable = new DataTable("SelectedUJoints");
            tmpTable.Columns.Add(new DataColumn("UjointName", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("Series", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("DriveshaftPartNumber", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("BearingLife", typeof(System.Double)));

            foreach (SelectedUjoint sj in uJoints)
            {
                DataRow rw = tmpTable.NewRow();
                rw["UjointName"] = sj.UjointName;
                rw["Series"] = sj.SeriesName;
                rw["DriveshaftPartNumber"] = sj.DriveshaftPartNumber;
                rw["BearingLife"] = sj.BearingLife;

                tmpTable.Rows.Add(rw);
            }

            return tmpTable;
        }

        public static DataTable Create(BindingList<SelectedDriveshaft> driveshafts)
        {
            DataTable tmpTable = new DataTable("SelectedDriveshafts");
            tmpTable.Columns.Add(new DataColumn("PartNumber", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("TubeSize", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("Phasing", typeof(System.String)));
            tmpTable.Columns.Add(new DataColumn("DriveshaftName", typeof(System.String)));

            foreach (SelectedDriveshaft sd in driveshafts)
            {
                DataRow rw = tmpTable.NewRow();
                rw["PartNumber"] = sd.PartNumber;
                rw["TubeSize"] = sd.TubeSize;
                rw["Phasing"] = sd.Phasing;
                rw["DriveshaftName"] = sd.DriveshaftName;

                tmpTable.Rows.Add(rw);
            }

            return tmpTable;
        }
    }
}
