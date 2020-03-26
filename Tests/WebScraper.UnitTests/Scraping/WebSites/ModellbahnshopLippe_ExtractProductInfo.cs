using FluentAssertions;
using System;
using System.Threading.Tasks;
using WebScraper.Resources;
using WebScraper.Scraping.Model;
using WebScraper.UnitTests.TestHelpers;
using WebScraper.UnitTests.TestHelpers.Crawlers;
using Xunit;

namespace WebScraper.Scraping.WebSites
{
    public class ModellbahnshopLippe_ExtractProductInfo
    {
        private readonly ModellbahnshopLippe _wrapper;

        public ModellbahnshopLippe_ExtractProductInfo()
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

        [Fact]
        public async Task ModellbahnshopLippe_ExtractProductInfo_ShouldExtractDescription()
        {
            var productInfo = await RunProductInfoExtraction();

            productInfo.Should().NotBeNull();
            productInfo.Brand.Should().Be("A.C.M.E.");
            productInfo.Description.Should().Be("60472 Gauge H0  E.633.001 locomotive of FS, Epoche IV");
        }

        [Fact]
        public async Task ModellbahnshopLippe_ExtractProductInfo_ShouldExtractFeatures()
        {
            var productInfo = await RunProductInfoExtraction();

            productInfo.Should().NotBeNull();
            productInfo.Features.Should().HaveCount(7);
            productInfo.Features.Should().Contain(new Feature
            {
                Icon = "/images/icons_spuren/icon_9.gif",
                Label = "H0 1:87"
            });
        }

        [Fact]
        public async Task ModellbahnshopLippe_ExtractProductInfo_ShouldExtractCategories()
        {
            var productInfo = await RunProductInfoExtraction();

            productInfo.Should().NotBeNull();
            productInfo.Categories.Should().HaveCount(3);
            productInfo.Categories.Should().Contain("A.C.M.E.");
            productInfo.Categories.Should().Contain("H0");
            productInfo.Categories.Should().Contain("Electric Locomotive");
        }

        [Fact]
        public async Task ModellbahnshopLippe_ExtractProductInfo_ShouldExtractSpecifications()
        {
            var productInfo = await RunProductInfoExtraction();

            productInfo.Should().NotBeNull();
            productInfo.Specifications.Should().HaveCount(14);
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
