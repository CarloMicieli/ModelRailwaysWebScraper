using Xunit;
using FluentAssertions;
using WebScraper.UnitTests.TestHelpers.Crawlers;
using System.Threading.Tasks;
using WebScraper.Scraping.Results;
using WebScraper.UnitTests.TestHelpers;
using WebScraper.Resources;

namespace WebScraper.Scraping.WebSites
{
    public class ModellbahnshopLippe_ExtractProductsList
    {
        private readonly ModellbahnshopLippe _wrapper;

        public ModellbahnshopLippe_ExtractProductsList()
        {
            var crawler = new FakeWebCrawler();
            _wrapper = new ModellbahnshopLippe(crawler);
        }

        [Fact]
        public async Task ModellbahnshopLippe_ShouldExtractPaginationInfo_WhenScrapingProductsListPage()
        {
            var result = await RunProductsExtraction();
            result.Should().NotBeNull();
            result.Pagination.Should().NotBeNull();

            result.ShouldNotHavePreviousPage();
            result.ShouldHaveCurrentPage();
            result.ShouldHaveNextPage();
        }

        [Fact]
        public async Task ModellbahnshopLippe_ShouldExtractProductsList()
        {
            var result = await RunProductsExtraction();
            result.Should().NotBeNull();
            result.Products.Should().NotBeNull();
            result.Products.Should().HaveCount(20);
        }

        private Task<ProductsResult> RunProductsExtraction()
        {
            var category = new Category
            {
                ResourceUri = TestPages.ModellbahnshopLippe.ProductsList,
                CategoryName = "Electric locomotive",
                Brand = "A.C.M.E.",
                Scale = "H0"                
            };
            return _wrapper.GetProducts(category);
        }
    }

    public static class ProductsResultExtensions
    {
        public static void ShouldHaveCurrentPage(this ProductsResult pr) => 
            pr.Pagination.CurrentPage.Should().NotBeNull();
        public static void ShouldHavePreviousPage(this ProductsResult pr) =>
            pr.Pagination.PreviousPage.Should().NotBeNull();
        public static void ShouldHaveNextPage(this ProductsResult pr) =>
            pr.Pagination.NextPage.Should().NotBeNull();

        public static void ShouldNotHavePreviousPage(this ProductsResult pr) =>
            pr.Pagination.PreviousPage.Should().BeNull();
    }
}
