using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Spectre.Console;

namespace WellnessTracker
{
    internal class UIManager
    {


        public static void DisplayMainMenu()
        {
            bool exit = false;
            do
            {

                Console.WriteLine("Welcome to the Wellness Tracker!");
                Console.WriteLine("Current Reminders: ");

                //TO DO: Display Goals and Reminders

                var choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select an option:")
                        .AddChoices(new[] {
                            "Log Activity",
                            "Manage Goals",
                            "Manage Reminders",
                            "Print Report",
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
                    case "Exit":
                        exit = true;
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
                    //TO DO: Add Reminder
                    break;
                case "Delete Reminder":
                    //TO DO: Delete Reminder
                    break;
                case "Exit":
                    return;
            }
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
                    //TO DO: Add Reminder
                    break;
                case "Delete Activity":
                    //TO DO: Delete Reminder
                    break;
                case "Exit":
                    return;
            }
        }

        public static void DisplayReportMenu()
        {
            Console.WriteLine("Select what to include in the report.");

            var reportHydration = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Hydration?")
                    .AddChoices(new[] {
                        "Yes",
                        "No"
            }));

            var reportFitness = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Hydration?")
                    .AddChoices(new[] {
                        "Yes",
                        "No"
            }));

            var reportNutrition = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Hydration?")
                    .AddChoices(new[] {
                        "Yes",
                        "No"
            }));

            //TO DO: Generate Report based on user selection
        }

    }
}
