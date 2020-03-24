using System.IO;
using System.Threading.Tasks;
using WebScraper.Scraping;

namespace WebScraper.UnitTests.Scraping.Testing
{
    public sealed class FakeWebCrawler : IWebCrawler
    {
        private const string BaseDir = @"..\..\..\TestPages\";

        public Task<string> FetchResource(string url)
        {
            using var streamReader = new StreamReader(BaseDir + url);
            return streamReader.ReadToEndAsync();
        }
    }
}
