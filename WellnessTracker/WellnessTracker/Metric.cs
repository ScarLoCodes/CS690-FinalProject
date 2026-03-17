using System;
using System.Collections.Generic;
using System.Text;

namespace WellnessTracker
{
    public class Metric
    {
        public Metric(MetricType type) { Type = type; }
        public enum MetricType
        {
            Nutrution, Fitness, Hydration
        }

        public MetricType Type { get; set; }

        public string Unit()
        {
            switch(Type)
                {
                case MetricType.Nutrution:
                    return "cal";
                case MetricType.Fitness:
                    return "min";
                case MetricType.Hydration:
                    return "ml";
                default:
                    return "";
            }   
        }
    }
}
