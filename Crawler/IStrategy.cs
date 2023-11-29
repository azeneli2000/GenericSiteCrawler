namespace CrawlerMangement
{
    public interface IStrategy
    {
        Task ApplyStrategy(Uri pageUri);
    }
}
