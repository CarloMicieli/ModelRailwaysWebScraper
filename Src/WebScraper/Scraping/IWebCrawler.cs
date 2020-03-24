using System;
using System.Threading.Tasks;

namespace WebScraper.Scraping
{
    public interface IWebCrawler
    {
        Task<string> FetchResource(string url);

        Task<string> FetchResource(Uri baseUri) => FetchResource(baseUri.AbsoluteUri);
    }
}
