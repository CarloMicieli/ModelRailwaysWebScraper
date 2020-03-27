using System.Net.Http;
using System.Threading.Tasks;

namespace WebScraper.Scraping
{
    public sealed class HttpWebCrawler : IWebCrawler
    {
        private readonly HttpClient _http;

        public HttpWebCrawler(IHttpClientFactory httpClientFactory) =>
            _http = httpClientFactory.CreateClient();

        public Task<string> FetchResource(string url)
        {
            return _http.GetStringAsync(url);
        }
    }
}
