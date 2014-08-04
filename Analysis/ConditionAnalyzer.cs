using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using PSS.DriveshaftCalculations.DriveshaftParts;
using PSS.DriveshaftCalculations.Collections;
using PSS.DriveshaftCalculations.Calculations;
using PSS.DriveshaftCalculations.DutyCycle;


namespace PSS.DriveshaftCalculations.Analysis
{
    /// <summary>
    /// Analyzes the conditions for a driveshaft series to determine if the series 
    /// is adequate for the given conditions.
    /// </summary>
    public class ConditionAnalyzer
    {
        #region Constructors

        /// <summary>
        /// Constructor used for a single condition.
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="lifeColl"></param>
        public ConditionAnalyzer(OperatingCondition condition, B10LifeCollection lifeColl)
        {
            _condition = condition;
            _lifeColl = lifeColl;
        }

        /// <summary>
        /// Constructor used for a duty cycle.
        /// </summary>
        /// <param name="dcColl"></param>
        /// <param name="lifeColl"></param>
        public ConditionAnalyzer(DutyCycleConditionCollection dccColl, B10LifeCollection lifeColl)
        {
            _dccColl = dccColl;
            _lifeColl = lifeColl;
        }

        #endregion

        #region Private fields
        private OperatingCondition _condition;
        private B10LifeCollection _lifeColl;
        private DutyCycleConditionCollection _dccColl;
        #endregion


        #region Private Methods

        /// <summary>
        /// Step though the analysis process for a metric driveshaft.
        /// </summary>
        /// <param name="series"></param>
        /// <param name="life"></param>
        /// <returns></returns>
        private AnalysisResult MetricDriveshaftAnalysis(MetricDriveshaftSeries series, B10Life life)
        {
            //step through the analysis process for a metric driveshaft.
            AnalysisResult result = new AnalysisResult(series, life.Hours);

            //all the metric units are kNm
            TorqueUnitConverter.StartingUnits startingUnits = TorqueUnitConverter.StartingUnits.LbFt;

            double torqueInKNm = TorqueUnitConverter.ConvertToKiloNewtonMeters(startingUnits, _condition.Torque);
            double shockInKNm = TorqueUnitConverter.ConvertToKiloNewtonMeters(startingUnits, _condition.ShockLoad);

            if (torqueInKNm >= series.MinElasticLimit)
            {
                series.Warnings.Add(DriveshaftSeries.SafetyWarning.MelExceeded);
            }
            else if (shockInKNm > series.FunctionalLimitTorque)
            {
                series.Warnings.Add(DriveshaftSeries.SafetyWarning.FunctionalTorqueLimitExceeded);
            }
            else if (shockInKNm > series.PulsatingFatigueTorque)
            {
                series.Warnings.Add(DriveshaftSeries.SafetyWarning.ShockExceedsPulsatingFatigueTorque);
            }
            else if (shockInKNm > series.ReversingFatigueTorque)
            {
                series.Warnings.Add(DriveshaftSeries.SafetyWarning.ShockExceedsReversingFatigueTorque);
            }
            else if (_condition.Rpm > series.MaxAllowableSpeed)
            {
                series.Warnings.Add(DriveshaftSeries.SafetyWarning.MaxSeriesRpmExceeded);
            }
            else if (life.Hours < _condition.DesiredLife)
            {
                series.Warnings.Add(DriveshaftSeries.SafetyWarning.HoursTooLow);
            }

            result.Comment = series.WarningMessage;

            return result;
        }


        /// <summary>
        /// Step through the analysis process for an english driveshaft.
        /// </summary>
        /// <param name="series"></param>
        private AnalysisResult EnglishDriveshaftAnalysis(EnglishDriveshaftSeries series, B10Life life)
        {
            //step through the analysis process for an english driveshaft.
            AnalysisResult result = new AnalysisResult(series, life.Hours);

            double torqueNominal = _condition.Torque;
            double maxTorque = Calculations.PowerCalculations.Torque(series.MaxNetHorsepower, _condition.Rpm);

            // 1. Make sure Minimum Elastic Limit is not exceeded.
            if (_condition.Torque >= series.MinElasticLimit)
            {
                series.Warnings.Add(DriveshaftSeries.SafetyWarning.MelExceeded);
            }
            // 2. Nominal torque cannot exceed max net hp for a given series.
            else if (_condition.Torque >= maxTorque)
            {
                series.Warnings.Add(DriveshaftSeries.SafetyWarning.MaxPowerExceeded);
            }
            // 3. Shock load cannot exceed the industrial rating.
            else if (_condition.ShockLoad > series.IndustrialRating)
            {
                series.Warnings.Add(DriveshaftSeries.SafetyWarning.ShockExceedsTorsionalRating);
            }
            // 4. Shock load cannot exceed MOH rating for mobile applications.
            else if (_condition.ShockLoad > series.MOH_Rating)
            {
                series.Warnings.Add(DriveshaftSeries.SafetyWarning.ShockExceedsMomentaryRating);
            }
            // 5. Must not exceed max allowable RPM for a given series.
            else if (_condition.Rpm > series.MaxAllowableSpeed)
            {
                series.Warnings.Add(DriveshaftSeries.SafetyWarning.MaxSeriesRpmExceeded);
            }
            // 6. Must get the desired hours.
            else if (life.Hours < _condition.DesiredLife)
            {
                series.Warnings.Add(DriveshaftSeries.SafetyWarning.HoursTooLow);
            }
            // 7. All tests passed, no warnings to add.

            //comment on the results.
            result.Comment = series.WarningMessage;

            return result;
        }

        private AnalysisResult SummarizeAnalysisResults(List<AnalysisResult> tempResults)
        {
            try
            {
                AnalysisResult tempResult = tempResults[0];

                for (int i = 1; i < tempResults.Count; i++)
                {
                    if (tempResults[i].Series.Warnings.Count > 0)
                    {
                        tempResult.Series.Warnings.AddRange(tempResults[i].Series.Warnings.ToArray());
                    }
                }

                return tempResult;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion



        #region Public Methods

        /// <summary>
        /// Analyzes a single condition.
        /// </summary>
        /// <returns>A list of analysis results.</returns>
        public List<AnalysisResult> Analysis()
        {
            try
            {
                List<AnalysisResult> results = new List<AnalysisResult>();

                foreach (B10Life life in _lifeColl.LifeCollection)
                {
                    DriveshaftSeries series = life.ObjSeries;
                    MetricDriveshaftSeries mSeries = series as MetricDriveshaftSeries;
                    EnglishDriveshaftSeries eSeries = series as EnglishDriveshaftSeries;

                    if (mSeries != null)
                    {
                        results.Add(MetricDriveshaftAnalysis(mSeries, life));
                    }
                    else if (eSeries != null)
                    {
                        results.Add(EnglishDriveshaftAnalysis(eSeries, life));
                    }
                }

                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Analyzes a duty cycle.
        /// </summary>
        /// <returns>A list of analysis results.</returns>
        public List<AnalysisResult> AnalysisOfDutyCycle()
        {
            try
            {
                ////used to create a relationship between each B10Life and the 
                ////AnalysisResults that will come from each OperatingCondition.
                //Dictionary<B10Life, List<AnalysisResult>> tempDict;
                //tempDict = new Dictionary<B10Life, List<AnalysisResult>>();

                //the ultimate list to return to the caller.
                List<AnalysisResult> results = new List<AnalysisResult>();

                foreach (B10Life life in _lifeColl.LifeCollection)
                {
                    if (life.ObjSeries != null)
                    {
                        List<AnalysisResult> tempResults = new List<AnalysisResult>();

                        DriveshaftSeries series = life.ObjSeries;
                        MetricDriveshaftSeries mSeries = series as MetricDriveshaftSeries;
                        EnglishDriveshaftSeries eSeries = series as EnglishDriveshaftSeries;

                        foreach (DutyCycleCondition cond in _dccColl)
                        {
                            //need to set the current condition.
                            _condition = cond.OperatingCondition;

                            if (mSeries != null)
                            {
                                tempResults.Add(MetricDriveshaftAnalysis(mSeries, life));
                            }
                            else if (eSeries != null)
                            {
                                tempResults.Add(EnglishDriveshaftAnalysis(eSeries, life));
                            }
                        }

                        AnalysisResult summaryResult = SummarizeAnalysisResults(tempResults);
                        results.Add(summaryResult);
                    }                    
                }

                return results;
            }
            catch (Exception)
            {
                throw;
            }
        }



        #endregion
    }
}
