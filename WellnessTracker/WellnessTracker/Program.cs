using Microsoft.Extensions.DependencyInjection;

namespace WellnessTracker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var ServiceCollector = new ServiceCollection();
            ServiceCollector.AddSingleton<UIManager>();
            ServiceCollector.AddSingleton<FileManager>();
            ServiceCollector.AddSingleton<IDataManager, DataManager>();

            var Provider = ServiceCollector.BuildServiceProvider();

            //Make sure the data file exists and load data
            var uiManager = Provider.GetRequiredService<UIManager>();
            uiManager.StartUp();

            uiManager.DisplayMainMenu();
        }
    }
}
