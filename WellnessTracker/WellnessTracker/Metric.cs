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
            Nutrition, Fitness, Hydration
        }

        public MetricType Type { get; set; }

        public override string ToString()
        {
            switch (Type)
            {
                case MetricType.Nutrition:
                    return "cal";
                case MetricType.Fitness:
                    return "min";
                case MetricType.Hydration:
                    return "ml";
                default:
                    return "???";
            }
        }

        public static Metric Nurtrition()
        {
            return new Metric(MetricType.Nutrition);
        }

        public static Metric Fitness()
        {
            return new Metric(MetricType.Fitness);
        }

        public static Metric Hydration()
        {
            return new Metric(MetricType.Hydration);
        }
    }
}
