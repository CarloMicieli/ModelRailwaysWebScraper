using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using NodaTime;
using WebScraper.Resources;
using WebScraper.Scraping.Model;
using WebScraper.Scraping.Results;

namespace WebScraper.Scraping.WebSites
{
    public sealed class Brawa : BaseWrapper
    {
        public Brawa(IClock clock, IWebCrawler webCrawler)
            : base(
                clock,
                webCrawler,
                new Uri("https://www.brawa.de"))
        {
        }

        public override Task<CategoriesResult> GetCategories(Manufacturer manufacturer)
        {
            throw new System.NotImplementedException();
        }

        public override Task<ImmutableList<Manufacturer>> GetManufacturers()
        {
            throw new System.NotImplementedException();
        }

        protected override ProductInfo ExtractProductInfo(IHtmlDocument html)
        {
            throw new System.NotImplementedException();
        }

        protected override (ImmutableList<Product>, ImmutableList<Page>) ExtractProducts(IHtmlDocument html)
        {
            throw new System.NotImplementedException();
        }
    }
}