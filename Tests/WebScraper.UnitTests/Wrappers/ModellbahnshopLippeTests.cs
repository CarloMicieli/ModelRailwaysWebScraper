using Xunit;
using FluentAssertions;
using WebScraper.Scraping.WebSites;
using WebScraper.UnitTests.Scraping.Testing;
using WebScraper.Resources;
using System;
using System.Threading.Tasks;
using WebScraper.Model;

namespace WebScraper.UnitTests.Wrappers
{
    public class ModellbahnshopLippeTests
    {
        private readonly ModellbahnshopLippe _wrapper;

        public ModellbahnshopLippeTests()
        {
            var crawler = new FakeWebCrawler();
            _wrapper = new ModellbahnshopLippe(crawler);
        }

        [Fact]
        public async Task ModellbahnshopLippe_ExtractProductInfo_ShouldExtractTitleAndMetdata()
        {
            var productInfo = await RunProductInfoExtraction();

            productInfo.Should().NotBeNull();
            productInfo.Title.Should().Be(@"A.C.M.E. 60472 Gauge H0 E.633.001 locomotive of FS, Epoche IV modellbahnshop-lippe.com");
            productInfo.Metadata.Should().HaveCount(4);
            productInfo.Metadata.Should().Contain(new Metadata
            {
                Name = "keywords",
                Content = "A.C.M.E. 60472 Gauge H0  E.633.001 locomotive of FS, Epoche IV  modellbahnshop-lippe.com"
            });
        }

        [Fact]
        public async Task ModellbahnshopLippe_ExtractProductInfo_ShouldExtractImages()
        {
            var productInfo = await RunProductInfoExtraction();

            productInfo.Should().NotBeNull();
            productInfo.Images.Should().HaveCount(2);
            productInfo.Images.Should().Contain(new Image
            {
                AltText = " 60472 E.633.001",
                ImageSrc = new Uri(@"https://www.modellbahnshop-lippe.com/article_data/images/69/241179_e.jpg")
            });
            productInfo.Images.Should().Contain(new Image
            {
                AltText = " 60472 E.633.001",
                ImageSrc = new Uri(@"https://www.modellbahnshop-lippe.com/article_data/images/69/241179_c.jpg")
            });
        }

        private Task<ProductInfo> RunProductInfoExtraction()
        {
            var product = new Product
            {
                ResourceUri = TestPages.ModellbahnshopLippe.ProductPage,
                ItemNumber = "60472"
            };

            return _wrapper.ExtractProductInfo(product);
        }
    }
}
