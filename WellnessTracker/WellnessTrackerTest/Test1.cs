using WellnessTracker;

namespace WellnessTrackerTest
{
    [TestClass]
    public sealed class Test1
    {
        //Data Manager Tests
        [TestMethod]
        public void Test_AddGoal()
        {
            WellnessTracker.DataManager.Goals.Clear();
            WellnessTracker.DataManager.AddGoal(WellnessTracker.Metric.Fitness(), 260, DateTime.Now, Goal.RecurringType.None);

            Assert.HasCount(1, WellnessTracker.DataManager.Goals);
        }

        [TestMethod]
        public void Test_UpdateGoal()
        {
            WellnessTracker.DataManager.Goals.Clear();
            WellnessTracker.DataManager.AddGoal(WellnessTracker.Metric.Hydration(), 300, DateTime.Now, Goal.RecurringType.Daily);
            var id = WellnessTracker.DataManager.Goals.Keys.First();
            WellnessTracker.DataManager.UpdateGoal(id,  350, DateTime.Now, false);

            Assert.AreEqual(350, WellnessTracker.DataManager.Goals[id].GoalValue);
        }

        [TestMethod]
        public void Test_DeleteGoal()
        {
            WellnessTracker.DataManager.Goals.Clear();
            WellnessTracker.DataManager.AddGoal(WellnessTracker.Metric.Hydration(), 300, DateTime.Now, Goal.RecurringType.Weekly);
            var id = WellnessTracker.DataManager.Goals.Keys.First();
            WellnessTracker.DataManager.DeleteGoal(id);

            Assert.HasCount(0, WellnessTracker.DataManager.Goals);
        }

        [TestMethod]
        public void Test_AddActivity()
        {
            WellnessTracker.DataManager.Goals.Clear();
            WellnessTracker.DataManager.AddGoal(WellnessTracker.Metric.Hydration(), 300, DateTime.Now, Goal.RecurringType.None);
            var id = WellnessTracker.DataManager.Goals.Keys.First();

            WellnessTracker.DataManager.AddActivity("Water", 50, WellnessTracker.Metric.Hydration());

            Assert.HasCount(1, WellnessTracker.DataManager.Activities);
        }

        [TestMethod]
        public void Test_DeleteActivity()
        {
            WellnessTracker.DataManager.Goals.Clear();
            WellnessTracker.DataManager.Activities.Clear();
            WellnessTracker.DataManager.AddGoal(WellnessTracker.Metric.Hydration(), 300, DateTime.Now, Goal.RecurringType.Daily);
            var id = WellnessTracker.DataManager.Goals.Keys.First();

            WellnessTracker.DataManager.AddActivity("Water", 50, WellnessTracker.Metric.Hydration());
            var activityId = WellnessTracker.DataManager.Activities.Keys.First();
            WellnessTracker.DataManager.DeleteActivity(activityId);

            Assert.HasCount(0, WellnessTracker.DataManager.Activities);
        }

        [TestMethod]
        public void Test_AddReminder()
        {
            WellnessTracker.DataManager.Reminders.Clear();
            WellnessTracker.DataManager.AddReminder("Test");

            Assert.HasCount(1, WellnessTracker.DataManager.Reminders);
        }

        [TestMethod]
        public void Test_DeleteReminder()
        {
            WellnessTracker.DataManager.Reminders.Clear();
            WellnessTracker.DataManager.AddReminder("Test");
            var id = WellnessTracker.DataManager.Reminders.Keys.First();

            WellnessTracker.DataManager.DeleteReminder(id);

            Assert.HasCount(0, WellnessTracker.DataManager.Reminders);
        }

        [TestMethod]
        public void Test_PrintGoals()
        {
            WellnessTracker.DataManager.Goals.Clear();
            WellnessTracker.DataManager.AddGoal(WellnessTracker.Metric.Fitness(), 260, DateTime.Now, Goal.RecurringType.Daily);

            Assert.IsNotNull(WellnessTracker.DataManager.PrintGoals());
        }

        [TestMethod]
        public void Test_PrintActivity()
        {
            WellnessTracker.DataManager.AddActivity("Water", 50, WellnessTracker.Metric.Hydration());
            Assert.IsNotNull(WellnessTracker.DataManager.PrintActivities());
        }

        [TestMethod]
        public void Test_PrintReminders()
        {
            WellnessTracker.DataManager.AddReminder("Test");
            Assert.IsNotNull(WellnessTracker.DataManager.PrintReminders());
        }

        [TestMethod]
        public void Test_ClearData()
        {
            WellnessTracker.DataManager.AddGoal(WellnessTracker.Metric.Fitness(), 260, DateTime.Now, Goal.RecurringType.Daily);
            WellnessTracker.DataManager.AddActivity("Water", 50, WellnessTracker.Metric.Hydration());
            WellnessTracker.DataManager.AddReminder("Test");
            WellnessTracker.DataManager.ClearData();

            Assert.HasCount(0, WellnessTracker.DataManager.Goals);
            Assert.HasCount(0, WellnessTracker.DataManager.Activities);
            Assert.HasCount(0, WellnessTracker.DataManager.Reminders);
        }

        [TestMethod]

        public void TestUpdateAdd()
        {
            WellnessTracker.DataManager.Goals.Clear();
            WellnessTracker.DataManager.AddGoal(WellnessTracker.Metric.Fitness(), 260, DateTime.Now, Goal.RecurringType.Daily);
            var id = WellnessTracker.DataManager.Goals.Keys.First();
            WellnessTracker.DataManager.AddActivity("Running", 30, WellnessTracker.Metric.Fitness());
            var activityId = WellnessTracker.DataManager.Activities.Keys.First();
            WellnessTracker.DataManager.UpdateProgress(id, activityId);

            Assert.AreEqual(30, WellnessTracker.DataManager.Goals[id].CurrentValue);
            Assert.HasCount(1, WellnessTracker.DataManager.Goals[id].ActivityIDs);
        }

        [TestMethod]

        public void TestUpdateDelete()
        {
            WellnessTracker.DataManager.Goals.Clear();
            WellnessTracker.DataManager.AddGoal(WellnessTracker.Metric.Fitness(), 260, DateTime.Now, Goal.RecurringType.Daily);
            var id = WellnessTracker.DataManager.Goals.Keys.First();
            WellnessTracker.DataManager.AddActivity("Running", 30, WellnessTracker.Metric.Fitness());
            var activityId = WellnessTracker.DataManager.Activities.Keys.First();
            WellnessTracker.DataManager.UpdateProgress(id, activityId);

            WellnessTracker.DataManager.DeleteActivity(activityId);

            Assert.AreEqual(0, WellnessTracker.DataManager.Goals[id].CurrentValue);
            Assert.HasCount(0, WellnessTracker.DataManager.Goals[id].ActivityIDs);
        }
    }
}
