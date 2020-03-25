using System.IO;
using System.Threading.Tasks;
using WebScraper.Scraping;

namespace WebScraper.UnitTests.Scraping.Testing
{
    public sealed class FakeWebCrawler : IWebCrawler
    {
        private const string BaseDir = @"..\..\..\TestPages\";

        public async Task<string> FetchResource(string url)
        {
            string path = Path.Combine(BaseDir, url.Replace(@"http://localhost/", "").Replace(@"/", "\\"));
            using var streamReader = new StreamReader(BaseDir + url.Replace(@"http://localhost/", ""));
            return await streamReader.ReadToEndAsync();
        }
    }
}
