using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Spectre.Console;

namespace WellnessTracker
{
    internal class UIManager
    {
        public static void StartUp()
        {
            AnsiConsole.MarkupLine("[blue]launching app...[/]");
            AnsiConsole.Status()
                .Start("Loading data...", ctx =>
                {
                    if (FileExporter.InitData())
                    {
                        AnsiConsole.MarkupLine("[green]Data loaded successfully![/]");
                    } else
                    {
                        AnsiConsole.MarkupLine("[green]No save data found. A new save file was created[/]");
                    }
                    
                });
        }

        public static void DisplayMainMenu()
        {

            AnsiConsole.MarkupLine("[bold green]Welcome to the Wellness Tracker![/]");

            bool exit = false;
            do
            {
                DisplaySummary();
                if (DataManager.Reminders.Count > 0)
                {
                    AnsiConsole.MarkupLine("[bold red]Current Reminders:[/]");
                    AnsiConsole.MarkupLine($"[gold1]{DataManager.PrintReminders()}[/]");
                }

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select an option:")
                        .AddChoices(new[] {
                            "Log Activity",
                            "Manage Goals",
                            "Manage Reminders",
                            "Print Report",
                            "Manage Data",
                            "Exit"
                        }));

                switch (choice)
                {
                    case "Manage Goals":
                        DisplayGoalMenu();
                        break;
                    case "Log Activity":
                        DisplayActivityMenu();
                        break;
                    case "Manage Reminders":
                        DisplayReminderMenu();
                        break;
                    case "Print Report":
                        DisplayReportMenu();
                        break;
                    case "Manage Data":
                        DisplayDataManagementMenu();
                        break;
                    case "Exit":
                        exit = true;
                        //Save data on exit
                        FileExporter.ExportData(FileExporter.DefaultFilePath);
                        break;
                }

            } while (!exit);
        }

        public static void DisplayGoalMenu()
        {

            AnsiConsole.MarkupLine("[bold yellow]Goals:[/]");
            Console.WriteLine(DataManager.PrintGoals());


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

        public static void AddGoal()
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
            DataManager.AddGoal(metric, goalValue, deadline);
        }

        public static void UpdateGoal()
        {
            var goalSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a goal to update:")
                    .UseConverter(id => DataManager.Goals[id].ToString())
                    .AddChoices(DataManager.Goals.Keys));

            var goalValue = AnsiConsole.Ask<int>("Enter your new target value:");

            var deadline = AnsiConsole.Ask<DateTime>("Enter the new deadline for this goal (MM/DD/YYYY):");

            DataManager.UpdateGoal(goalSelection, goalValue, deadline, false);
        }

        public static bool DeleteGoal()
        {
            var goalSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a goal to delete:")
                    .UseConverter(id => DataManager.Goals[id].ToString())
                    .AddChoices(DataManager.Goals.Keys));
            return DataManager.DeleteGoal(goalSelection);
        }

        public static void PrintGoals()
        {
            AnsiConsole.MarkupLine("[bold yellow]Goals:[/]");
        }

        public static void DisplayReminderMenu()
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

        public static void AddReminder()
        {
            var name = AnsiConsole.Ask<string>("Enter the text of the reminder:");
            DataManager.AddReminder(name);
        }

        public static bool DeleteReminder()
        {
            var reminderSelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a reminder to delete:")
                    .UseConverter(id => DataManager.Reminders[id].ToString())
                    .AddChoices(DataManager.Reminders.Keys));
            return DataManager.DeleteReminder(reminderSelection);
        }

        public static void DisplayActivityMenu()
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

        public static void AddActivity()
        {
            //Check if there are goals. If not, prompt user to create a goal first.
            if (DataManager.Goals.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No goals found. Please create a goal before logging an activity.[/]");
                return;
            }

            var goalId = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a goal to associate with this activity:")
                    .UseConverter(id => DataManager.Goals[id].ToString())
                    .AddChoices(DataManager.Goals.Keys));

            var name = AnsiConsole.Ask<string>("Enter the name of the activity:");

            Metric metric = DataManager.Goals[goalId].Metric;

            var value = AnsiConsole.Ask<int>("Enter the value for this activity:");
            var id = DataManager.AddActivity(name, value, metric);
            DataManager.UpdateProgress(goalId, id);

        }

        public static bool DeleteActivity()
        {
            //Check if there are goals. If not, prompt user to create a goal first.
            if (DataManager.Goals.Count == 0)
            {
                AnsiConsole.MarkupLine("[red]No goals found. Please create a goal before deleting an activity.[/]");
                return false;
            }

            var activitySelection = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select an activity to delete:")
                    .UseConverter(id => DataManager.Activities[id].ToString())
                    .AddChoices(DataManager.Activities.Keys));
            return DataManager.DeleteActivity(activitySelection);
        }

        public static void DisplayDataManagementMenu()
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
                    FileExporter.ExportData(FileExporter.DefaultFilePath);
                    AnsiConsole.MarkupLine("[green]Data saved successfully![/]");
                    break;
                case "Load Data":
                    FileExporter.ImportData(FileExporter.DefaultFilePath);
                    AnsiConsole.MarkupLine("[green]Data loaded successfully![/]");
                    break;
                case "Clear Data":
                    if (AnsiConsole.Confirm("[red]Are you sure you want to delete all of your data?[/]"))
                    {
                        DataManager.ClearData();
                        AnsiConsole.MarkupLine("[green]Data cleared successfully![/]");
                    }
                    break;
                case "Exit":
                    return;
            }
        }

        public static void DisplayReportMenu()
        {
            var filename = AnsiConsole.Ask<string>("Enter the filename for the report (without extension):");
            FileExporter.ExportReport($"{filename}.txt");
        }

        public static void DisplaySummary()
        {
            AnsiConsole.MarkupLine("[blue]Summary[/]");
            foreach (var goal in DataManager.Goals.Values)
            {
                AnsiConsole.MarkupLine($" - {goal.ToString()}");
            }
            AnsiConsole.MarkupLine("[blue]Latest Activities:[/]");
            var activities = DataManager.Activities.Values.OrderBy(item => item.Time).Take(5);
            foreach (var item in activities)
            {
                AnsiConsole.MarkupLine($" - {item.ToString()}");
            }
        }
    }
}
