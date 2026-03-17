using System;
using System.Collections.Generic;
using System.Text;

namespace WellnessTracker
{
    public class Goal
    {
            public Goal(Metric metric, int goalValue, DateTime deadline) 
            {
                Metric = metric;
                GoalValue = goalValue;
                CurrentValue = 0;
                Deadline = deadline;
            }

        public string ID { get; set; } = Guid.NewGuid().ToString();

        public Metric Metric { get; set; }

        public int GoalValue { get; set; }

        public int CurrentValue { get; set; }

        public DateTime Deadline { get; set; }

        public List<string> ActivityIDs { get; set; } = new List<string>();

        public override string ToString()
        {
            return $"- {Deadline.ToShortDateString()} (Current: {CurrentValue}/{GoalValue}{Metric})";
        }
    }
}
