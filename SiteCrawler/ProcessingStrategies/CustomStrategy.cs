using CrawlerMangement;
namespace SiteCrawler.ProcessingStrategies
{
    //test strategy class to fulfill the requirment :
    //The consumer of the library should be able to execute a custom action on each of the pages found
    public class CustomStrategy : IStrategy
    {
        public Task ApplyStrategy(Uri pageUri)
        {
            Console.WriteLine($"Custom action executed for page: {pageUri}");
            return Task.CompletedTask;
        }
    }
}
