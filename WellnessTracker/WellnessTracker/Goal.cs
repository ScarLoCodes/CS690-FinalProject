using System;
using System.Collections.Generic;
using System.Text;

namespace WellnessTracker
{
    internal class Goal
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

        public override string ToString()
        {
            return $"Goal: {GoalValue}{Metric.Unit()} by {Deadline.ToShortDateString()} (Current: {CurrentValue}/{GoalValue}{Metric.Unit()})";
        }
    }
}
