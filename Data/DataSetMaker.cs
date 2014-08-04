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
    public class DataSetMaker
    {
        public static DataSet AnalysisReportDataSet(DriveshaftLayoutCollection dlc, CriticalSpeedCollection crsc,
                                                    MetricDriveshaftSeries mds, EnglishDriveshaftSeries eds,
                                                    DutyCycleCollection dcc, string series, int hours,
                                                    DriveshaftApplicationData dad, BindingList<SelectedDriveshaft> driveshafts,
                                                    BindingList<SelectedUjoint> ujoints)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add(DataTableMaker.Create(crsc));
                ds.Tables.Add(DataTableMaker.Create(eds));
                ds.Tables.Add(DataTableMaker.Create(mds));
                ds.Tables.Add(DataTableMaker.Create(dcc, series, hours));
                ds.Tables.Add(DataTableMaker.Create(dad));
                ds.Tables.Add(DataTableMaker.Create(dlc));
                ds.Tables.Add(DataTableMaker.Create(driveshafts));
                ds.Tables.Add(DataTableMaker.Create(ujoints));

                return ds;
            }
            catch (NullReferenceException nrex)
            {
                string problem = nrex.Source;
                throw new Exception(problem);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
