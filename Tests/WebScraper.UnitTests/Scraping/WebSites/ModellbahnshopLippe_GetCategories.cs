using FluentAssertions;
using System.Threading.Tasks;
using WebScraper.Resources;
using WebScraper.Scraping.Results;
using WebScraper.UnitTests.TestHelpers;
using WebScraper.UnitTests.TestHelpers.Crawlers;
using Xunit;

namespace WebScraper.Scraping.WebSites
{
    public class ModellbahnshopLippe_GetCategories
    {
        private readonly ModellbahnshopLippe _wrapper;

        public ModellbahnshopLippe_GetCategories()
        {
            var crawler = new FakeWebCrawler();
            _wrapper = new ModellbahnshopLippe(crawler);
        }

        [Fact]
        public async Task ModellbahnshopLippe_ShouldExtract_TheCategoriesListWithResourceInfo()
        {
            var categories = await RunCategoriesExtraction();
            categories.Should().NotBeNull();
            categories.Resource.Should().NotBeNull();
        }

        [Fact]
        public async Task ModellbahnshopLippe_ShouldExtract_TheCategoriesList()
        {
            var result = await RunCategoriesExtraction();
            result.Should().NotBeNull();
            result.Categories.Should().HaveCount(31);
        }

        private Task<CategoriesResult> RunCategoriesExtraction()
        {
            var manufacturer = new Manufacturer
            {
                ResourceUri = TestPages.ModellbahnshopLippe.ManufacturerPage,
                Name = "A.C.M.E."
            };
            return _wrapper.GetCategories(manufacturer);
        }
    }
}
