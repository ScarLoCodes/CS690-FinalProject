using System;
using System.Collections.Generic;
using System.Text;

namespace WellnessTracker
{
    public class DataManager : IDataManager
    {
        public Dictionary<string, Activity> Activities { get; set; } = new Dictionary<string, Activity>();
        public Dictionary<string, Goal> Goals { get; set; } = new Dictionary<string, Goal>();
        public Dictionary<string, string> Reminders { get; set; } = new Dictionary<string, string>();

        public string AddActivity(string name, int value, Metric metric)
        {
            Activity entry = new Activity(name, value, metric); 
            Activities.Add(entry.ID, entry);
            return entry.ID;
        }

        public bool DeleteActivity(string id)
        {
            var query =
                from goal in Goals
                where goal.Value.ActivityIDs.Contains(id)
                select goal;

            foreach (var goal in query)
            {
                goal.Value.CurrentValue -= Activities[id].Value;
                goal.Value.ActivityIDs.Remove(id);
            }

                return Activities.Remove(id);
        }

        public void UpdateProgress(string goalId, string activityId)
        {
            var goal = Goals.ContainsKey(goalId) ? Goals[goalId] : null;
            var activity = Activities.ContainsKey(activityId) ? Activities[activityId] : null;
            if (goal != null && activity != null)
            {
                goal.CurrentValue += activity.Value;
                goal.ActivityIDs.Add(activityId);
            }
        }

        public void AddGoal(Metric metric, int goalValue, DateTime deadline, Goal.RecurringType type)
        {
            Goal goal = new Goal(metric, goalValue, deadline, type);
            Goals.Add(goal.ID, goal);
        }
    
        public bool DeleteGoal(string id)
        {
            var goal = Goals.ContainsKey(id) ? Goals[id] : null;
            if (goal != null)
            {
                foreach (var activityId in goal.ActivityIDs)
                {
                    var activity = Activities.ContainsKey(activityId) ? Activities[activityId] : null;
                    if (activity != null)
                    {
                        goal.CurrentValue -= activity.Value;
                        Activities.Remove(activityId);
                    }
                }
                return Goals.Remove(id);
            }
            return false;
        }

        public void UpdateGoal(string id, int goalValue, DateTime deadline, bool ClearActivities)
        {
            var goal = Goals.ContainsKey(id) ? Goals[id] : null;
            if (goal != null)
            {
                goal.GoalValue = goalValue;
                goal.Deadline = deadline;
            }
        }

        public void UpdateGoalDeadline()
        {

            //Remove expired non-recurring goals
            var expiredGoals =
                from goal in Goals
                where goal.Value.Recurring == Goal.RecurringType.None && goal.Value.Deadline <= DateTime.Now
                select goal;
            expiredGoals.ToList().ForEach(g => DeleteGoal(g.Value.ID));

            //Update recurring goals and collect expired activities
            var expiredActivities = new List<string>();
            foreach (var goal in Goals)
            {
                if(goal.Value.Deadline <= DateTime.Now)
                {
                    goal.Value.PeriodDeadlineUpdate();

                    expiredActivities.AddRange(goal.Value.ActivityIDs);
                }
            }

            //clean up activities
            foreach (var activityId in expiredActivities)
            {
                DeleteActivity(activityId);
            }
        }

        public List<Goal> CheckDeadlines()
        {
            List<Goal> list = new List<Goal>();
            foreach (var goal in Goals)
            {
                if (goal.Value.Deadline <= DateTime.Now)
                {
                    list.Add(goal.Value);
                }
            }
            return list;
        }

        public void AddReminder(string message)
        {
            Reminders.Add(Guid.NewGuid().ToString(), message);
        }

        public bool DeleteReminder(string id)
        {
            return Reminders.Remove(id);
        }

        public void ClearData()
        {
            Activities.Clear();
            Goals.Clear();
            Reminders.Clear();
        }

        public string PrintGoals()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var goal in Goals)
            {
                sb.AppendLine(goal.Value.ToString());
            }
            return sb.ToString();
        }

        public string PrintReminders()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var reminder in Reminders)
            {
                sb.AppendLine(reminder.Value);
            }
            return sb.ToString();
        }

        public string PrintActivities()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var activity in Activities)
            {
                sb.AppendLine(activity.ToString());
            }
            return sb.ToString();
        }
    }
}
