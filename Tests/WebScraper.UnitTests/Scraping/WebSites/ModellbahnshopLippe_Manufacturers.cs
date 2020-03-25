using FluentAssertions;
using System;
using System.Threading.Tasks;
using WebScraper.Model;
using WebScraper.Resources;
using WebScraper.Resources.Collections;
using WebScraper.UnitTests.TestHelpers;
using WebScraper.UnitTests.TestHelpers.Crawlers;
using Xunit;

namespace WebScraper.Scraping.WebSites
{
    public class ModellbahnshopLippe_Manufacturers
    {
        private readonly ModellbahnshopLippe _wrapper;

        public ModellbahnshopLippe_Manufacturers()
        {
            var crawler = new FakeWebCrawler();
            _wrapper = new ModellbahnshopLippe(crawler);
        }

        private Task<IManufacturersCollection> RunProductInfoExtraction()
        {
            return _wrapper.GetManufacturers();
        }
    }
}
