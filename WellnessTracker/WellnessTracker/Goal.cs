using System;
using System.Collections.Generic;
using System.Text;

namespace WellnessTracker
{
    public class Goal
    {
        public enum RecurringType { None, Daily, Weekly, Monthly }

        public Goal(Metric metric, int goalValue, DateTime deadline, RecurringType type) 
        {
            Metric = metric;
            GoalValue = goalValue;
            CurrentValue = 0;
            Deadline = deadline;
            Recurring = type;
        }

        public string ID { get; set; } = Guid.NewGuid().ToString();

        public Metric Metric { get; set; }

        public RecurringType Recurring { get; set; }

        public int GoalValue { get; set; }

        public int CurrentValue { get; set; }

        public DateTime Deadline { get; set; }

        public List<string> ActivityIDs { get; set; } = new List<string>();

        public void PeriodDeadlineUpdate()
        {
            if (Recurring == Goal.RecurringType.None)
            {
                //Do not update for non-recurring type.
                return;
            }

            if (Recurring == Goal.RecurringType.Daily)
            {
                Deadline = DateTime.Now.Date.AddDays(1);
                
            }
            else if (Recurring == Goal.RecurringType.Weekly)
            {
                Deadline = DateTime.Now.Date.AddDays(7);
                
            }
            else if (Recurring == Goal.RecurringType.Monthly)
            {
                Deadline = DateTime.Now.Date.AddMonths(1);
                
            }
        }

        public override string ToString()
        {
            return $"{Deadline.ToShortDateString()} (Current: {CurrentValue}/{GoalValue}{Metric})";
        }
    }
}
