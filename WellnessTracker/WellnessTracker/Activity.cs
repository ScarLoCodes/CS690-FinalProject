using System;
using System.Collections.Generic;
using System.Text;

namespace WellnessTracker
{
    internal class Activity
    {
        public Activity(string name, int value, Metric metric) 
        {
            Name = name;
            Value = value;
            Metric = metric;
            Time = DateTime.Now;
        }
        public string ID { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; }

        public Metric Metric { get; set; }

        public int Value { get; set; }

        public DateTime Time { get; set; }

        public override string ToString()
        {
            return $"{Name}: {Value}{Metric.Unit()} at {Time}";
        }
    }
}
