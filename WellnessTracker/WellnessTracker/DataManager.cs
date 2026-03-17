using System;
using System.Collections.Generic;
using System.Text;

namespace WellnessTracker
{
    public class DataManager
    {
        public static Dictionary<string, Activity> Activities { get; set; } = new Dictionary<string, Activity>();
        public static Dictionary<string, Goal> Goals { get; set; } = new Dictionary<string, Goal>();

        public static Dictionary<string, string> Reminders { get; set; } = new Dictionary<string, string>();

        public static string AddActivity(string name, int value, Metric metric)
        {
            Activity entry = new Activity(name, value, metric); 
            Activities.Add(entry.ID, entry);
            return entry.ID;
        }

        public static bool DeleteActivity(string id)
        {
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

        public static void AddGoal(Metric metric, int goalValue, DateTime deadline)
        {
            Goal goal = new Goal(metric, goalValue, deadline);
            Goals.Add(goal.ID, goal);
        }
    
        public static bool DeleteGoal(string id)
        {
            var goal = Goals.ContainsKey(id) ? Goals[id] : null;
            if (goal != null)
            {
                return Goals.Remove(id);
            }
            return false;
        }

        public static void UpdateGoal(string id, int goalValue, DateTime deadline, bool ClearActivities)
        {
            var goal = Goals.ContainsKey(id) ? Goals[id] : null;
            if (goal != null)
            {
                goal.GoalValue = goalValue;
                goal.Deadline = deadline;
            }
        }

        public static void AddReminder(string message)
        {
            Reminders.Add(Guid.NewGuid().ToString(), message);
        }

        public static bool DeleteReminder(string id)
        {
            return Reminders.Remove(id);
        }

        public static string PrintGoals()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var goal in Goals)
            {
                sb.AppendLine(goal.Value.ToString());
            }
            return sb.ToString();
        }

        public static string PrintReminders()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var reminder in Reminders)
            {
                sb.AppendLine(reminder.Value);
            }
            return sb.ToString();
        }

        public static string PrintActivities()
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
