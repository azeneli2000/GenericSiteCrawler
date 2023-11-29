using System.Text.RegularExpressions;

namespace CrawlerMangement
{
    public class Crawler : ICrawler
    {
        private readonly HashSet<string> _visitedLinks;
        private readonly string _domain;
        private readonly IStrategy _processingStrategy;
        private readonly HttpClient _httpClient;
        private readonly Uri _startPageUrl;

        public Crawler(string startUrl, IStrategy strategy)
        {
            _processingStrategy = strategy;
            _visitedLinks = new HashSet<string>();
            _domain = new Uri(startUrl).Host;
            _httpClient = new HttpClient();
            _startPageUrl = new Uri(startUrl);
        }

        public async Task CrawlSite()
        {
            Queue<Uri> queue = new();
            queue.Enqueue(_startPageUrl);
            while (queue.Count > 0)
            {
                Uri currentUrl = queue.Dequeue();
                if (_visitedLinks.Contains(currentUrl.AbsoluteUri) || currentUrl.Host != _domain)
                    continue;
                try
                {
                    _visitedLinks.Add(currentUrl.AbsoluteUri);
                    Console.WriteLine($"Visiting: {currentUrl}");
                    await _processingStrategy.ApplyStrategy(currentUrl);
                    string html = await _httpClient.GetStringAsync(currentUrl);
                    var links = ExtractUrls(html);
                    if (links != null)
                    {
                        foreach (var link in links)
                        {
                            Uri nextLink = new(currentUrl, link);
                            queue.Enqueue(nextLink);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error reading  {currentUrl}: {ex.Message}");
                }
            }
        }

        private IEnumerable<string> ExtractUrls(string html)
        {
            List<string> links = new();
            Regex regex = new(@"<a\s+(?:[^>]*?\s+)?href=""([^""]*)""", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(html);
            links.AddRange(from Match match in matches
                           let link = match.Groups[1].Value.Trim()
                           where !string.IsNullOrEmpty(link) && !link.StartsWith("#")
                           select link);
            return links;
        }          
    }

}