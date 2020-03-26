using FluentAssertions;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using WebScraper.Resources;
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
            _wrapper = new ModellbahnshopLippe(crawler, TestPages.ModellbahnshopLippe.ManufacturersPage);
        }

        [Fact]
        public async Task ModellbahnshopLippe_ShouldExtract_TheManufacturesList()
        {
            var manufacturers = await RunManufacturersExtraction();
            manufacturers.Should().NotBeNull();
            manufacturers.Should().HaveCount(126);
            manufacturers.Should().Contain(new Manufacturer
            {
                Name = "A.C.M.E.",
                ResourceUri = new Uri("https://www.modellbahnshop-lippe.com/Manufacturer/Products/gb/overview.html?hersteller=A%2EC%2EM%2EE%2E")
            });
        }

        private Task<ImmutableList<Manufacturer>> RunManufacturersExtraction()
        {
            return _wrapper.GetManufacturers();
        }
    }
}
