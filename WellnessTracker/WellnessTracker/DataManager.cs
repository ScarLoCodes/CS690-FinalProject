using System;
using System.Collections.Generic;
using System.Text;

namespace WellnessTracker
{
    public class DataManager
    {
        public  Dictionary<string, Activity> Activities { get; set; } = new Dictionary<string, Activity>();
        public List<Goal> Goals { get; set; } = new List<Goal>();



    }
}
