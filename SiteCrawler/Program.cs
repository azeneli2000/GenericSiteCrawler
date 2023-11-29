
using CrawlerMangement;
using SiteCrawler.ProcessingStrategies;
//strategy pattern used to let the consumer execute custom actions,
//in ProcessingStrategies folder are present two strategies, one required by the business domain and one test strategy for testing the switch between strategies.
string startingUrl = "https://google.com";
IStrategy strategy = new SaveToFileStrategy();
var cr = new Crawler(startingUrl, strategy);
await cr.CrawlSite();
Console.ReadKey();
