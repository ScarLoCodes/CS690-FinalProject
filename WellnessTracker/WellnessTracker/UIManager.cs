using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Text;
using Spectre.Console;

namespace WellnessTracker
{
    internal class UIManager
    {
        private readonly IDataManager _DataManager;
        private readonly FileManager _FileManager;
        public UIManager(IDataManager dataManager, FileManager fileManager)
        {
            _DataManager = dataManager;
            _FileManager = fileManager;
        }
        public void StartUp()
        {
            AnsiConsole.MarkupLine("[blue]launching app...[/]");
            AnsiConsole.Status()
                .Start("Loading data...", ctx =>
                {
                    if (_FileManager.InitData())
                    {
                        AnsiConsole.MarkupLine("[green]Data loaded successfully![/]");
                    }
                    else
                    {
                        AnsiConsole.MarkupLine("[green]No save data found. A new save file was created[/]");
                    }

                });

            //Check if goals have completed their deadline and prompt the update process.
            var NeedsUpdates = _DataManager.CheckDeadlines();
            if (NeedsUpdates.Count > 0)
            {
                AnsiConsole.MarkupLine("[red]The following goals have reached their deadline![/]");
                foreach (var goal in NeedsUpdates)
                {
                    AnsiConsole.MarkupLine($"  {goal.ToString()}");
                }
                var confirmed = AnsiConsole.Confirm("Print report for moving to next interation? All related expired goals and activities will be deleted.");

                if (confirmed)
                {
                    _FileManager.ExportReport($"report_{DateTime.Now.Year.ToString()}_{DateTime.Now.Month.ToString()}_{DateTime.Now.Day.ToString()}_ExpiredGoals.txt");
                }

                _DataManager.UpdateGoalDeadline();
            }
        }

        public void DisplayMainMenu()
        {

            AnsiConsole.MarkupLine("[bold green]Welcome to the Wellness Tracker![/]");

            bool exit = false;
            do
            {
                DisplayShortSummary();
                if (_DataManager.Reminders.Count > 0)
                {
                    AnsiConsole.Write(DisplayReminders(_DataManager.Reminders.Values.ToArray()));
                }

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select an option:")
                        .AddChoices(new[] {
                            "Log Activity",
                            "Manage Goals",
                            "Manage Reminders",
                            "View All Progress",
                            "Print Report",
                            "Manage Data",
                            "Exit"
                        }));

                switch (choice)
                {
                    case "Manage Goals":
                        AnsiConsole.Clear();
                        DisplayGoalMenu();
                        AnsiConsole.Clear();
                        break;
                    case "Log Activity":
                        AnsiConsole.Clear();
                        DisplayActivityMenu();
                        AnsiConsole.Clear();
                        break;
                    case "Manage Reminders":
                        AnsiConsole.Clear();
                        DisplayReminderMenu();
                        AnsiConsole.Clear();
                        break;
                    case "View All Progress":
                        AnsiConsole.Clear();
                        DisplayFullSummary();
                        AnsiConsole.MarkupLine("Press any key to return to the main menu...");
                        Console.ReadKey();
                        AnsiConsole.Clear();
                        break;
                    case "Print Report":
                        AnsiConsole.Clear();
                        DisplayReportMenu();
                        AnsiConsole.Clear();
                        break;
                    case "Manage Data":
                        AnsiConsole.Clear();
                        DisplayDataManagementMenu();
                        AnsiConsole.Clear();
                        break;
                    case "Exit":
                        exit = true;
                        //Save data on exit
                        _FileManager.ExportData(_FileManager.FilePath);
                        break;
                }

            } while (!exit);
        }

        public void DisplayGoalMenu()
        {

            AnsiConsole.MarkupLine("[bold yellow]Goals:[/]");
            Console.WriteLine(_DataManager.PrintGoals());


            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option:")
                    .AddChoices(new[] {
                        "Add Goal",
                        "Update Goal",
                        "Delete Goal",
                        "Exit"
                    }));

            switch (choice)
            {

                case "Add Goal":
                    AddGoal();
                    break;
                case "Update Goal":
                    UpdateGoal();
                    break;
                case "Delete Goal":
                    DeleteGoal();
                    break;
                case "Exit":
                    return;
            }
        }

        public void AddGoal()
        {
            var metricChoice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a metric for your goal:")
                    .AddChoices(new[] {
                        "Nutrition",
                        "Fitness",
                        "Hydration"
            }));
            Metric metric;
            switch (metricChoice)
            {
                case "Nutrition":
                    metric = Metric.Nurtrition();
                    break;
                case "Fitness":
                    metric = Metric.Fitness();
                    break;
                case "Hydration":
                    metric = Metric.Hydration();
                    break;
                default:
                    throw new Exception("Invalid metric choice");
            }
            var goalValue = AnsiConsole.Ask<int>("Enter your target value:");
            var deadline = AnsiConsole.Ask<DateTime>("Enter the deadline for this goal (MM/DD/YYYY):");
            var recurring = AnsiConsole.Prompt(new SelectionPrompt<string>()
                .Title("Select a recurring type for this goal:")
                .AddChoices(new[] {
                "None",
                "Daily",
                "Weekly"
            }));

            var type = Goal.RecurringType.None;
            if (recurring == "Daily")
            {
                type = Goal.RecurringType.Daily;
            }
            else if (recurring == "Weekly")
            {
                type = Goal.RecurringType.Weekly;
            }

            _DataManager.AddGoal(metric, goalValue, deadline, type);
        }

        public void UpdateGoal()
        {
            var goalSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a goal to update:")
                    .UseConverter(id => _DataManager.Goals[id].ToString())
                    .AddChoices(_DataManager.Goals.Keys));

            var goalValue = AnsiConsole.Ask<int>("Enter your new target value:");

            var deadline = AnsiConsole.Ask<DateTime>("Enter the new deadline for this goal (MM/DD/YYYY):");

            _DataManager.UpdateGoal(goalSelection, goalValue, deadline, false);
        }

        public bool DeleteGoal()
        {
            var goalSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a goal to delete:")
                    .UseConverter(id => _DataManager.Goals[id].ToString())
                    .AddChoices(_DataManager.Goals.Keys));
            return _DataManager.DeleteGoal(goalSelection);
        }

        public void PrintGoals()
        {
            AnsiConsole.MarkupLine("[bold yellow]Goals:[/]");
        }

        public void DisplayReminderMenu()
        {

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option:")
                    .AddChoices(new[] {
                        "Add Reminder",
                        "Delete Reminder",
                        "Exit"
                    }));

            switch (choice)
            {
                case "Add Reminder":
                    AddReminder();
                    break;
                case "Delete Reminder":
                    DeleteReminder();
                    break;
                case "Exit":
                    return;
            }
        }

        public void AddReminder()
        {
            var name = AnsiConsole.Ask<string>("Enter the text of the reminder:");
            _DataManager.AddReminder(name);
        }

        public bool DeleteReminder()
        {
            var reminderSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a reminder to delete:")
                    .UseConverter(id => _DataManager.Reminders[id].ToString())
                    .AddChoices(_DataManager.Reminders.Keys));
            return _DataManager.DeleteReminder(reminderSelection);
        }

        public void DisplayActivityMenu()
        {

            //TO DO: Display Activities List (top 5)

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option:")
                    .AddChoices(new[] {
                        "Add Activity",
                        "Delete Activity",
                        "Exit"
            }));

            switch (choice)
            {
                case "Add Activity":
                    AddActivity();
                    break;
                case "Delete Activity":
                    DeleteActivity();
                    break;
                case "Exit":
                    return;
            }
        }

        public void AddActivity()
        {
            //Check if there are goals. If not, prompt user to create a goal first.
            if (_DataManager.Goals.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No goals found. Please create a goal before logging an activity.[/]");
                return;
            }

            var goalId = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a goal to associate with this activity:")
                    .UseConverter(id => _DataManager.Goals[id].ToString())
                    .AddChoices(_DataManager.Goals.Keys));

            var name = AnsiConsole.Ask<string>("Enter the name of the activity:");

            Metric metric = _DataManager.Goals[goalId].Metric;

            var value = AnsiConsole.Ask<int>("Enter the value for this activity:");
            var id = _DataManager.AddActivity(name, value, metric);
            _DataManager.UpdateProgress(goalId, id);

        }

        public bool DeleteActivity()
        {
            //Check if there are goals. If not, prompt user to create a goal first.
            if (_DataManager.Goals.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No goals found. Please create a goal before deleting an activity.[/]");
                return false;
            }

            var activitySelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an activity to delete:")
                    .UseConverter(id => _DataManager.Activities[id].ToString())
                    .AddChoices(_DataManager.Activities.Keys));
            return _DataManager.DeleteActivity(activitySelection);
        }

        public void DisplayDataManagementMenu()
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an option:")
                    .AddChoices(new[] {
                        "Save Data",
                        "Load Data",
                        "Clear Data",
                        "Exit"
            }));

            switch (choice)
            {
                case "Save Data":
                    _FileManager.ExportData(_FileManager.FilePath);
                    AnsiConsole.MarkupLine("[green]Data saved successfully![/]");
                    break;
                case "Load Data":
                    _FileManager.ImportData(_FileManager.FilePath);
                    AnsiConsole.MarkupLine("[green]Data loaded successfully![/]");
                    break;
                case "Clear Data":
                    if (AnsiConsole.Confirm("[red]Are you sure you want to delete all of your data?[/]"))
                    {
                        _DataManager.ClearData();
                        AnsiConsole.MarkupLine("[green]Data cleared successfully![/]");
                    }
                    break;
                case "Exit":
                    return;
            }
        }

        public void DisplayReportMenu()
        {
            var selections = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Select the goals to include in the report:")
                    .UseConverter(id => _DataManager.Goals[id].ToString())
                    .AddChoices(_DataManager.Goals.Keys));

            var filename = AnsiConsole.Ask<string>("Enter the filename for the report (without extension):");
            if (filename == null || filename == string.Empty)
            {
                filename = $"report_{DateTime.Now.Year.ToString()}_{DateTime.Now.Month.ToString()}_{DateTime.Now.Day.ToString()}.txt";
            }

            var success = _FileManager.ExportReport($"{filename}.txt", selections);
            if (success)
            {
                AnsiConsole.MarkupLine($"[green]Report exported successfully as {filename}.txt[/]");

            }
            else
            {
                AnsiConsole.MarkupLine("[red]Failed to export report. Please try again.[/]");
            }
            AnsiConsole.MarkupLine("Press any key to return to the main menu...");
            Console.ReadKey();
        }

        public void DisplayShortSummary()
        {

            //AnsiConsole.MarkupLine("[blue]Summary[/]");
            var Goals = DisplayGoals(_DataManager.Goals.Values.ToArray());

            //AnsiConsole.MarkupLine("[blue]Latest Activities:[/]");
            var QueryResults = _DataManager.Activities.Values.OrderByDescending(item => item.Time).Take(5);
            var Activities = DisplayActivities(QueryResults.ToArray());

            AnsiConsole.Write(Goals);
            AnsiConsole.Write(Activities);  

        }

        public void DisplayFullSummary()
        {
            var tree = new Spectre.Console.Tree("Full Summary");

            foreach (var goal in _DataManager.Goals.Values)
            {
                var goalNode = tree.AddNode($"[blue]{goal.ToString()}[/]");
                if (goal.ActivityIDs.Count > 0)
                {
                    foreach (var activityId in goal.ActivityIDs)
                    {
                        var activity = _DataManager.Activities[activityId];
                        goalNode.AddNode(activity.ToString());
                    }
                }
            }
            
            AnsiConsole.Write(GetPanel(tree, ""));
        }

        public Spectre.Console.Panel DisplayGoals(Goal[] goals)
        {
            if (goals.Length != 0)
            {
                var chart = new Spectre.Console.BarChart()
                    .WithMaxValue(100);
                foreach (Goal goal in goals)
                {
                    var progress = (double)goal.CurrentValue / goal.GoalValue * 100;
                    var color = ConsoleColor.White;
                    if (progress > 100)
                    {
                        color = ConsoleColor.Green;
                    }
                    else if (progress > 75)
                    {
                        color = ConsoleColor.Cyan;
                    }
                    else if (progress > 50)
                    {
                        color = ConsoleColor.Blue;
                    }
                    else if (progress > 25)
                    {
                        color = ConsoleColor.Yellow;
                    }
                    else
                    {
                        color = ConsoleColor.Red;
                    }
                    chart.AddItem(goal.ToString(), progress, color);
                }

                var panel = GetPanel(chart, "Goals Progress");

                return panel;
            }
            else
            {
                return GetPanel(new Spectre.Console.Text("No goals found.", new Style(foreground: Color.Yellow)), "Goals Progress").Expand();
            }
        }

        public Spectre.Console.Panel DisplayActivities(Activity[] activities)
        {
            if (activities.Length != 0)
            {
                var Table = new Spectre.Console.Table()
                    .AddColumn("Date")
                    .AddColumn("Name")
                    .AddColumn("Metric");

                foreach (Activity activity in activities)
                {
                    Table.AddRow(activity.Time.ToString(), activity.Name, $"{activity.Value} {activity.Metric.ToString()}");
                }
                return GetPanel(Table, "Activities").Expand();
            }
            else
            {
                return GetPanel(new Spectre.Console.Text("No activities found.", new Style(foreground: Color.Yellow)), "Activities").Expand();
            }
        
        }

        public Spectre.Console.Panel DisplayReminders(string[] reminders)
        {
            var output = "";
            foreach (string reminder in reminders)
            {
                output += $"- {reminder}\n";
            }
            var text = new Spectre.Console.Text(output, new Style(foreground: Color.Yellow));
            return GetPanel(text, "Reminders").Expand();
        }

        public Spectre.Console.Panel GetPanel(Spectre.Console.Rendering.IRenderable content, string header)
        {
            var panel = new Spectre.Console.Panel(content)
                .Padding(2, 1)
                .RoundedBorder();
            if (!string.IsNullOrEmpty(header))
            {
                panel.Header(header, Justify.Left);
            }
            return panel;
        }

        
    }
}
