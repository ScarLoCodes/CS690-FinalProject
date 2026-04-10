using System;
using System.Collections.Generic;
using System.Text;

namespace WellnessTracker
{
    internal interface IDataManager
    {
        public Dictionary<string, Activity> Activities { get; set; }
        public Dictionary<string, Goal> Goals { get; set; }

        public Dictionary<string, string> Reminders { get; set; }

        public string AddActivity(string name, int value, Metric metric);

        public bool DeleteActivity(string id);

        public void UpdateProgress(string goalId, string activityId);

        public void AddGoal(Metric metric, int goalValue, DateTime deadline, Goal.RecurringType type);

        public bool DeleteGoal(string id);

        public void UpdateGoal(string id, int goalValue, DateTime deadline, bool ClearActivities);

        public void UpdateGoalDeadline();

        public List<Goal> CheckDeadlines();


        public void AddReminder(string message);

        public bool DeleteReminder(string id);


        public void ClearData();


        public string PrintGoals();


        public string PrintReminders();


        public string PrintActivities();

    }
}
