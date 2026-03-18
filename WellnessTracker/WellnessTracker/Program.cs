namespace WellnessTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Make sure the data file exists and load data
            UIManager.StartUp();

            UIManager.DisplayMainMenu();
        }
    }
}
