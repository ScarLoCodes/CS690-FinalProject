using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace WellnessTracker
{
    public class FileExporter
    {
        public const string DefaultFilePath = "wellness_data.json";

        private class DataHolder
        {
            public Dictionary<string, Activity> Activities { get; set; }
            public Dictionary<string, Goal> Goals { get; set; }
            public Dictionary<string, string> Reminders { get; set; }

            public DataHolder(Dictionary<string, Activity> activities, Dictionary<string, Goal> goals, Dictionary<string, string> reminders)
            {
                Activities = activities;
                Goals = goals;
                Reminders = reminders;
            }
        }

        public static bool InitData()
        {
            if(!File.Exists(DefaultFilePath))
            {
                ExportData(DefaultFilePath);
                return false;
            } else
            {
                ImportData(DefaultFilePath);
                return true;
            }
        }

        public static void ExportData(string filePath)
        {
            var output = JsonConvert.SerializeObject(new DataHolder(DataManager.Activities, DataManager.Goals, DataManager.Reminders), Formatting.Indented);
            System.IO.File.WriteAllText(filePath, output);
        }

        public static void ImportData(string filePath)
        {
            var jsonData = System.IO.File.ReadAllText(filePath);
            var data = JsonConvert.DeserializeObject<DataHolder>(jsonData);
            if (data != null)
            {
                DataManager.Activities = data.Activities ?? new Dictionary<string, Activity>();
                DataManager.Goals = data.Goals ?? new Dictionary<string, Goal>();
                DataManager.Reminders = data.Reminders ?? new Dictionary<string, string>();
            }
        }

        public static void ExportReport(string filename)
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine("Wellness Tracker Report");
            report.AppendLine($"Date: {DateTime.Now}");
            report.AppendLine("======================");
            foreach(var item in DataManager.Goals.Values)
            {
                report.AppendLine(item.ToString());
                if (item.ActivityIDs.Count > 0)
                {
                    foreach (var id in item.ActivityIDs)
                    {
                        var activity = DataManager.Activities.ContainsKey(id) ? DataManager.Activities[id] : null;
                        if (activity != null)
                        {
                            report.AppendLine($" -- {activity.ToString()}");
                        }
                    }
                }
            }

            File.WriteAllText(filename, report.ToString());
        }
    }
}
