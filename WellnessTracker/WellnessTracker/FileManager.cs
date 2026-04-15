using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace WellnessTracker
{
    /// <summary>
    /// The class that handles all file I/O operations for the application.
    /// <remarks>This class class contains only static members</remarks>
    /// </summary>
    internal class FileManager
    {
        /// <summary>
        /// The filename for the data file of the application.
        /// If the file does not exist, it will be created with empty data. If the file exists, data will be imported from it on application startup.
        /// </summary>
        public string FilePath = "wellness_data.json";
        private readonly IDataManager _DataManager;
        /// <summary>
        /// A storage object for serializing and deserializing the application's data, including activities, goals, and reminders.
        /// </summary>
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

        public FileManager(IDataManager dataManager)
        {
            _DataManager = dataManager;
        }
        /// <summary>
        /// Initializes the application's data by ensuring the default data file exists and is loaded.
        /// </summary>
        /// <remarks>If the default data file does not exist, it is created with exported data. Otherwise,
        /// existing data is imported from the file.</remarks>
        /// <returns>true if the default data file was found and data was imported; otherwise, false.</returns>
        public bool InitData()
        {
            if(!File.Exists(FilePath))
            {
                ExportData(FilePath);
                return false;
            } else
            {
                ImportData(FilePath);
                return true;
            }
        }

        /// <summary>
        /// Exports the application's activities, goals, and reminders to a JSON file at the specified path.
        /// </summary>
        /// <param name="filePath">The full file path where the exported JSON data will be written. If the file exists, it will be overwritten.</param>
        public void ExportData(string filePath)
        {
            var output = JsonConvert.SerializeObject(new DataHolder(_DataManager.Activities, _DataManager.Goals, _DataManager.Reminders), Formatting.Indented);
            System.IO.File.WriteAllText(filePath, output);
        }

        /// <summary>
        /// Imports application data from a JSON file and updates the current activities, goals, and reminders.
        /// </summary>
        /// <remarks>If the file does not contain valid data, the existing activities, goals, and
        /// reminders are not modified. Existing data will be replaced by the imported values.</remarks>
        /// <param name="filePath">The path to the JSON file containing the data to import. The file must exist and be accessible.</param>
        public void ImportData(string filePath)
        {
            var jsonData = System.IO.File.ReadAllText(filePath);
            var data = JsonConvert.DeserializeObject<DataHolder>(jsonData);
            if (data != null)
            {
                _DataManager.Activities = data.Activities ?? new Dictionary<string, Activity>();
                _DataManager.Goals = data.Goals ?? new Dictionary<string, Goal>();
                _DataManager.Reminders = data.Reminders ?? new Dictionary<string, string>();
            }
        }

        /// <summary>
        /// Exports all goals and their associated activities to a text file in a human-readable format.
        /// </summary>
        /// <remarks>Does not assign a file type. This must be dictated in the filename.</remarks>
        /// <param name="filename"></param>
        public bool ExportReport(string filename)
        {
            try
            {
                StringBuilder report = new StringBuilder();
                report.AppendLine("Wellness Tracker Report");
                report.AppendLine($"Date: {DateTime.Now.ToString()}");
                report.AppendLine("======================");
                foreach (var item in _DataManager.Goals.Values)
                {
                    report.AppendLine(item.ToString());
                    if (item.ActivityIDs.Count > 0)
                    {
                        foreach (var id in item.ActivityIDs)
                        {
                            var activity = _DataManager.Activities.ContainsKey(id) ? _DataManager.Activities[id] : null;
                            if (activity != null)
                            {
                                report.AppendLine($" -- {activity.ToString()}");
                            }
                        }
                    }
                }

                File.WriteAllText(filename, report.ToString());
                return true;
            } catch (Exception ex) 
            {
                return false;
            }
        }

        /// <summary>
        /// Exports a selection of goals and their associated activities to a text file in a human-readable format.
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="Goals"></param>
        public bool ExportReport(string filename, List<string> GoalIDs)
        {
            try
            {
                //fetch Goals
                var Goals =
                    from goal in _DataManager.Goals
                    where GoalIDs.Contains(goal.Key)
                    select goal.Value;

                StringBuilder report = new StringBuilder();
                report.AppendLine("Wellness Tracker Report");
                report.AppendLine($"Date: {DateTime.Now.ToString()}");
                report.AppendLine("======================");
                foreach (var item in Goals)
                {
                    report.AppendLine(item.ToString());
                    if (item.ActivityIDs.Count > 0)
                    {
                        foreach (var id in item.ActivityIDs)
                        {
                            var activity = _DataManager.Activities.ContainsKey(id) ? _DataManager.Activities[id] : null;
                            if (activity != null)
                            {
                                report.AppendLine($" -- {activity.ToString()}");
                            }
                        }
                    }
                }

                File.WriteAllText(filename, report.ToString());

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
