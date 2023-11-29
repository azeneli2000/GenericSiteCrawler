using CrawlerMangement;
namespace SiteCrawler.ProcessingStrategies
{
    public class SaveToFileStrategy : IStrategy
    {
        private  readonly HttpClient _httpClient;
        public SaveToFileStrategy()
        {
            _httpClient =new HttpClient();
        }
        private const string BasePath = @"C:\CrawledPages";
        public async  Task ApplyStrategy(Uri pageUrl)
        {
            string pageContent = await _httpClient.GetStringAsync(pageUrl);
            if (!string.IsNullOrEmpty(pageContent))
            {
                string filePath = GetFilePath(pageUrl);
                SavePageToFile(filePath, pageContent);
                Console.WriteLine($"Page saved: {filePath}");
            }
        }

        private string GetFilePath(Uri pageUri)
        {
            string fileName = pageUri.AbsoluteUri.GetHashCode().ToString() + ".html";
            string path = Path.Combine(BasePath, fileName);
            return path;
        }

        private void SavePageToFile(string filePath, string content)
        {
            try
            {
                Directory.CreateDirectory(BasePath);
                File.WriteAllText(filePath, content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving page: {ex.Message}");
            }
        }
    }
}

