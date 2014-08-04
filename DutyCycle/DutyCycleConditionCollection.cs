using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.ComponentModel;

using PSS.DriveshaftCalculations.Collections;
using PSS.DriveshaftCalculations.DriveshaftParts;
using PSS.DriveshaftCalculations.Analysis;
using PSS.Common;

namespace PSS.DriveshaftCalculations.DutyCycle
{
    /// <summary>
    /// Represents the collection of DutyCycleCondition objects that 
    /// make up the duty cycle.
    /// </summary>
    [Serializable]
    public class DutyCycleConditionCollection : IEnumerable, INotifyPropertyChanged, IPersistXml
    {
        public DutyCycleConditionCollection()
        {
            //instantiate the list.
            _conditions = new BindingList<DutyCycleCondition>();

            //we want to know as conditions are added or removed so the 
            //listeners can adjust the duty cycle accordingly.
            _conditions.RaiseListChangedEvents = true;

            _conditions.ListChanged += new ListChangedEventHandler(DutyCycleConditionCountChanged);

            //arbritary value.
            _minimumHours = 5000;

            _conditionToExclude = new DutyCycleCondition();
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


        //use this to exclude a condition from being changed, without
        //locking it.
        private DutyCycleCondition _conditionToExclude;

        private BindingList<DutyCycleCondition> _conditions;
        /// <summary>
        /// A list of operating conditions.
        /// </summary>
        public BindingList<DutyCycleCondition> Conditions
        {
            get { return _conditions; }
            set { _conditions = value; }
        }

        private Analysis.AnalysisResult _selectedSeriesAnalysisResult;
        /// <summary>
        /// The analysis result for the selected series.
        /// </summary>
        public Analysis.AnalysisResult SelectedSeriesAnalysisResult
        {
            get { return _selectedSeriesAnalysisResult; }
            set 
            {
                if (_selectedSeriesAnalysisResult != value)
                {
                    _selectedSeriesAnalysisResult = value;
                    NotifyPropertyChanged("SelectedSeriesAnalysisResult");
                }                
            }
        }        

        private double _minimumHours;
        /// <summary>
        /// The minimum number of hours this duty cycle condition collection is expected
        ///  to live for.
        /// </summary>
        public double MinimumHours
        {
            get { return _minimumHours; }
            set
            {
                if (_minimumHours != value)
                {
                    _minimumHours = value;
                    foreach (DutyCycleCondition dcc in _conditions)
                    {
                        dcc.OperatingCondition.DesiredLife = (int)value;
                    }

                    NotifyPropertyChanged("MinimumHours");
                }                
            }
        }

        private string _jointName;
        /// <summary>
        /// The name of the universal joint this collection is tied to.
        /// </summary>
        public string JointName
        {
            get { return _jointName; }
            set 
            {
                if (_jointName != value)
                {
                    _jointName = value;
                    NotifyPropertyChanged("JointName");                
                }            
            }
        }
        
        /// <summary>
        /// Keeps track of the number of conditions added.
        /// </summary>
        public int ConditionCounter { get; set; }
       
        //Required by IEnumerator.
        public IEnumerator GetEnumerator()
        {
            return _conditions.GetEnumerator();
        }

        public static DutyCycleConditionCollection Empty()
        {
            return null;
        }

        public void DutyCycleConditionCountChanged(object sender, ListChangedEventArgs e)
        {
            SetDefaultConditions();
        }

        public void AddNewCondition(DutyCycleCondition condition)
        {
            try
            {
                condition.PercentChanging += new EventHandler(OnConditionChanged);
                Conditions.Add(condition);

                //this will set the initial percentage of the new condition.
                SetDefaultConditions();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void RemoveExistingCondition(DutyCycleCondition condition)
        {
            try
            {
                if (!RemovalNeedsToBeStopped(condition))
                {                  
                    Conditions.Remove(condition);
                    SetDefaultConditions();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void SetDefaultConditions()
        {
            try
            {
                List<DutyCycleCondition> locked = LockedConditions();
                List<DutyCycleCondition> unlocked = UnlockedConditions();

                double pctLocked = PercentageLocked(locked);

                double excludedPct;
                if (_conditionToExclude != null)
                {
                    excludedPct = _conditionToExclude.PercentOfTime * 100;
                }
                else
                {
                    excludedPct = 0;
                }

                double pctUnlocked = 100 - pctLocked - excludedPct;

                //the number of unlocked conditions where change is desirable.
                int nbrUnlkWithChgDesired = 0;

                if (_conditionToExclude != null && _conditionToExclude.Name != null)
                {
                    nbrUnlkWithChgDesired = 1;
                }

                if (unlocked.Count > 0)
                {                    
                    foreach (DutyCycleCondition dcc in unlocked)
                    {
                        if (dcc != _conditionToExclude)
                        {
                            int divideBetween = unlocked.Count - nbrUnlkWithChgDesired;

                            //this will avoid the divide by zero.
                            if (divideBetween == 0)
                            {
                                divideBetween = 1;
                            }

                            //suspend this for now...
                            dcc.PercentChanging -= new EventHandler(OnConditionChanged);

                            //divide the remaining percentages by whatever is still unlocked.
                            dcc.SetPercentOfTime(pctUnlocked / divideBetween);

                            //now re-add it.
                            dcc.PercentChanging += new EventHandler(OnConditionChanged); 
                        }
                    }

                    //arbritarily set to a new condition, so it's not equal to the condition to exclude.
                    _conditionToExclude = new DutyCycleCondition();
                }
                else if (locked.Count > 0)
                {
                    CreateDummyCondition(locked[0]);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Creates a dummy condition becuase the user has tried to delete all the 
        /// unlocked conditions and only leave locked conditions that don't total
        /// up to 100%.
        /// </summary>
        private void CreateDummyCondition(DutyCycleCondition dc)
        {
            try
            {
                OperatingCondition opConClone = (OperatingCondition)dc.OperatingCondition.Clone();
                DutyCycleCondition dcClone = new DutyCycleCondition(opConClone);

                dcClone.Name = string.Format("Condition {0}", ConditionCounter);

                Conditions.Add(dcClone);
            }
            catch (Exception)
            {                
                throw;
            }
        }

        private void OnConditionChanged(object sender, EventArgs e)
        
        {
            try
            {
                DutyCycleCondition condition = (DutyCycleCondition)sender;
                PercentageChangeEventArgs eCast = (PercentageChangeEventArgs)e;

                if (OkToUpdateProperty(condition, eCast))
                {
                    condition.SetPercentOfTime(eCast.NewValue);

                    _conditionToExclude = condition;

                    //dont ever lock the percent automatically, nothing but trouble.
                    //condition.PercentIsLocked = true;

                    ChangeUnlockedToRemainder(condition);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void ChangeUnlockedToRemainder(DutyCycleCondition conditionToExclude)
        {
            List<DutyCycleCondition> locked = LockedConditions();
            List<DutyCycleCondition> unlocked = UnlockedConditions();

            double exclCondPct = 0;
            double exclCondCount = 0;

            if (conditionToExclude.PercentIsLocked == false)
            {
                exclCondCount = 1;
                exclCondPct = conditionToExclude.PercentOfTime * 100;
            }


            double pctLocked = PercentageLocked(locked);
            double pctUnlocked = 100 - pctLocked - exclCondPct;

            double divideBetween = unlocked.Count - exclCondCount;

            //if there is only one unlocked condition, we cannot change it's value
            // or will loose ability to add percents up to 100%
            if (unlocked.Count > 1)
            {
                foreach (DutyCycleCondition dcUnlocked in unlocked)
                {
                    //we just changed the condition to exclude, so we don't want to change it again.
                    if (dcUnlocked != conditionToExclude)
                    {
                        double pct = pctUnlocked / divideBetween;
                        dcUnlocked.SetPercentOfTime(pct); 
                    }
                }
            }          
            
        }

        private bool OkToUpdateProperty(DutyCycleCondition dccToUpdate, PercentageChangeEventArgs e)
        {
            try
            {
                bool isOk = false;

                List<DutyCycleCondition> locked = LockedConditions();
                List<DutyCycleCondition> unlocked = UnlockedConditions();

                double pctLocked = PercentageLocked(locked);
                double pctUnlocked = 100 - pctLocked;
                double newValAsPct = e.NewValue;

                if (unlocked.Count > 1)
                {
                    if (newValAsPct <= pctUnlocked)
                    {
                        isOk = true;
                    }
                }

                return isOk;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //A list of all the locked conditions currently in the Conditions list.
        private List<DutyCycleCondition> LockedConditions()
        {
            List<DutyCycleCondition> lockedConditions = (from lc in Conditions
                                                         where lc.PercentIsLocked == true
                                                         select lc).ToList<DutyCycleCondition>();
            return lockedConditions;
        }

        //A list of all the unlocked conditions currently in the Conditions list.
        private List<DutyCycleCondition> UnlockedConditions()
        {
            List<DutyCycleCondition> unlockedConditions = (from lc in Conditions
                                                           where lc.PercentIsLocked == false
                                                           select lc).ToList<DutyCycleCondition>();
            return unlockedConditions;
        }

        private double PercentageLocked(List<DutyCycleCondition> conditions)
        {
            double pct = (from c in conditions
                          where c.PercentIsLocked = true
                          select c.PercentOfTime).Sum();
            return pct * 100;
        }

        /// <summary>
        /// Checks to see if removing the duty cycle condtion is allowed or not.
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public bool RemovalNeedsToBeStopped(DutyCycleCondition c)
        {
            if (UnlockedConditions().Count > 1)
            {
                //this won't let the user remove the last unlocked condition.
                return false;
            }
            else
            {
                return true;
            }
        }

        #region IPersistXmlMethods

        public void Load(System.Xml.XmlElement ele)
        {
            ConditionCounter = (int)XML.Open_int(ele, "ConditionCounter", 0);
            //_conditionToExclude = (DutyCycleCondition)XML.Open_IPersistXml(ele, "_conditionToExclude", null);
            JointName = (string)XML.Open_str(ele, "JointName", string.Empty);
            SelectedSeriesAnalysisResult = (AnalysisResult)XML.Open_IPersistXml(ele, "SelectedSeriesAnalysisResult", null);
            
            _conditions = new BindingList<DutyCycleCondition>();
            _conditions.RaiseListChangedEvents = true;

            List<DutyCycleCondition> temps = (List<DutyCycleCondition>)XML.Open_List<DutyCycleCondition>(ele, "Conditions", new List<DutyCycleCondition>());
            foreach (DutyCycleCondition temp in temps)
            {
                _conditions.Add(temp);
            }

            //now start letting listeners know when the list changes.
            _conditions.ListChanged += new ListChangedEventHandler(DutyCycleConditionCountChanged);

            MinimumHours = (double)XML.Open_dbl(ele, "MinimumHours", 5000);
        }

        public void Save(System.Xml.XmlElement ele)
        {
            XML.Save(ele, "ConditionCounter", ConditionCounter);
            //XML.Save(ele, "_conditionToExclude", _conditionToExclude);
            XML.Save(ele, "JointName", JointName);
            XML.Save(ele, "SelectedSeriesAnalysisResult", SelectedSeriesAnalysisResult);
            XML.Save_List(ele, "Conditions", Conditions.ToList<DutyCycleCondition>());       
            XML.Save(ele, "MinimumHours", MinimumHours);
        } 

        #endregion
    }
}
