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
            var MyDataManager = new DataManager();
            MyDataManager.Goals.Clear();
            MyDataManager.AddGoal(WellnessTracker.Metric.Fitness(), 260, DateTime.Now, Goal.RecurringType.None);

            Assert.HasCount(1, MyDataManager.Goals);
        }

        [TestMethod]
        public void Test_UpdateGoal()
        {
            var MyDataManager = new DataManager();
            MyDataManager.Goals.Clear();
            MyDataManager.AddGoal(WellnessTracker.Metric.Hydration(), 300, DateTime.Now, Goal.RecurringType.Daily);
            var id = MyDataManager.Goals.Keys.First();
            MyDataManager.UpdateGoal(id,  350, DateTime.Now, false);

            Assert.AreEqual(350, MyDataManager.Goals[id].GoalValue);
        }

        [TestMethod]
        public void Test_DeleteGoal()
        {
            var MyDataManager = new DataManager();
            MyDataManager.Goals.Clear();
            MyDataManager.AddGoal(WellnessTracker.Metric.Hydration(), 300, DateTime.Now, Goal.RecurringType.Weekly);
            var id = MyDataManager.Goals.Keys.First();
            MyDataManager.DeleteGoal(id);

            Assert.HasCount(0, MyDataManager.Goals);
        }

        [TestMethod]
        public void Test_AddActivity()
        {
            var MyDataManager = new DataManager();
            MyDataManager.Goals.Clear();
            MyDataManager.AddGoal(WellnessTracker.Metric.Hydration(), 300, DateTime.Now, Goal.RecurringType.None);
            var id = MyDataManager.Goals.Keys.First();

            MyDataManager.AddActivity("Water", 50, WellnessTracker.Metric.Hydration());

            Assert.HasCount(1, MyDataManager.Activities);
        }

        [TestMethod]
        public void Test_DeleteActivity()
        {
            var MyDataManager = new DataManager();
            MyDataManager.Goals.Clear();
            MyDataManager.Activities.Clear();
            MyDataManager.AddGoal(WellnessTracker.Metric.Hydration(), 300, DateTime.Now, Goal.RecurringType.Daily);
            var id = MyDataManager.Goals.Keys.First();

            MyDataManager.AddActivity("Water", 50, WellnessTracker.Metric.Hydration());
            var activityId = MyDataManager.Activities.Keys.First();
            MyDataManager.DeleteActivity(activityId);

            Assert.HasCount(0, MyDataManager.Activities);
        }

        [TestMethod]
        public void Test_AddReminder()
        {
            var MyDataManager = new DataManager();
            MyDataManager.Reminders.Clear();
            MyDataManager.AddReminder("Test");

            Assert.HasCount(1, MyDataManager.Reminders);
        }

        [TestMethod]
        public void Test_DeleteReminder()
        {
            var MyDataManager = new DataManager();
            MyDataManager.Reminders.Clear();
            MyDataManager.AddReminder("Test");
            var id = MyDataManager.Reminders.Keys.First();

            MyDataManager.DeleteReminder(id);

            Assert.HasCount(0, MyDataManager.Reminders);
        }

        [TestMethod]
        public void Test_PrintGoals()
        {
            var MyDataManager = new DataManager();
            MyDataManager.Goals.Clear();
            MyDataManager.AddGoal(WellnessTracker.Metric.Fitness(), 260, DateTime.Now, Goal.RecurringType.Daily);

            Assert.IsNotNull(MyDataManager.PrintGoals());
        }

        [TestMethod]
        public void Test_PrintActivity()
        {
            var MyDataManager = new DataManager();
            MyDataManager.AddActivity("Water", 50, WellnessTracker.Metric.Hydration());
            Assert.IsNotNull(MyDataManager.PrintActivities());
        }

        [TestMethod]
        public void Test_PrintReminders()
        {
            var MyDataManager = new DataManager();
            MyDataManager.AddReminder("Test");
            Assert.IsNotNull(MyDataManager.PrintReminders());
        }

        [TestMethod]
        public void Test_ClearData()
        {
            var MyDataManager = new DataManager();
            MyDataManager.AddGoal(WellnessTracker.Metric.Fitness(), 260, DateTime.Now, Goal.RecurringType.Daily);
            MyDataManager.AddActivity("Water", 50, WellnessTracker.Metric.Hydration());
            MyDataManager.AddReminder("Test");
            MyDataManager.ClearData();

            Assert.HasCount(0, MyDataManager.Goals);
            Assert.HasCount(0, MyDataManager.Activities);
            Assert.HasCount(0, MyDataManager.Reminders);
        }

        [TestMethod]

        public void TestUpdateAdd()
        {
            var MyDataManager = new DataManager();
            MyDataManager.Goals.Clear();
            MyDataManager.AddGoal(WellnessTracker.Metric.Fitness(), 260, DateTime.Now, Goal.RecurringType.Daily);
            var id = MyDataManager.Goals.Keys.First();
            MyDataManager.AddActivity("Running", 30, WellnessTracker.Metric.Fitness());
            var activityId = MyDataManager.Activities.Keys.First();
            MyDataManager.UpdateProgress(id, activityId);

            Assert.AreEqual(30, MyDataManager.Goals[id].CurrentValue);
            Assert.HasCount(1, MyDataManager.Goals[id].ActivityIDs);
        }

        [TestMethod]

        public void TestUpdateDelete()
        {
            var MyDataManager = new DataManager();
            MyDataManager.Goals.Clear();
            MyDataManager.AddGoal(WellnessTracker.Metric.Fitness(), 260, DateTime.Now, Goal.RecurringType.Daily);
            var id = MyDataManager.Goals.Keys.First();
            MyDataManager.AddActivity("Running", 30, WellnessTracker.Metric.Fitness());
            var activityId = MyDataManager.Activities.Keys.First();
            MyDataManager.UpdateProgress(id, activityId);

            MyDataManager.DeleteActivity(activityId);

            Assert.AreEqual(0, MyDataManager.Goals[id].CurrentValue);
            Assert.HasCount(0, MyDataManager.Goals[id].ActivityIDs);
        }
    }
}
