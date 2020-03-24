using System;
using System.Threading.Tasks;
using WebScraper.Scraping;
using WebScraper.Scraping.WebSites;

namespace SampleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            IWebCrawler webCrawler = new HttpWebCrawler();

            var lippe = new ModellbahnshopLippe(webCrawler);
            var results = await lippe.GetManufacturers();

        }
    }
}
