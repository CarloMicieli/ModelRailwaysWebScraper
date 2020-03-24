using System.Net.Http;
using System.Threading.Tasks;

namespace WebScraper.Scraping
{
    public sealed class HttpWebCrawler : IWebCrawler
    {
        private static readonly HttpClient _http = new HttpClient();

        public Task<string> FetchResource(string url)
        {
            return _http.GetStringAsync(url);
        }
    }
}
