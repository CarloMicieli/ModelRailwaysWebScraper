using AngleSharp.Html.Dom;
using NodaTime;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using WebScraper.Resources;
using WebScraper.Scraping.Results;

namespace WebScraper.Scraping
{
    public abstract class WrapperWithStartPage : BaseWrapper
    {
        protected WrapperWithStartPage(IClock clock, IWebCrawler webCrawler, Uri baseUri, Uri startPage)
            : base(clock, webCrawler, baseUri)
        {
            StartPage = startPage;
        }

        public Uri StartPage { get; }

        public override async Task<ImmutableList<Manufacturer>> GetManufacturers()
        {
            IHtmlDocument html = await FetchDocument(StartPage.AbsoluteUri);
            return ExtractManufacturers(html);
        }

        public override async Task<CategoriesResult> GetCategories(Manufacturer manufacturer)
        {
            IHtmlDocument html = await FetchDocument(manufacturer);
            var results = ExtractCategories(html);
            return new CategoriesResult(
                manufacturer,
                GetCurrentInstant(),
                results);
        }

        protected abstract ImmutableList<Manufacturer> ExtractManufacturers(IHtmlDocument html);

        protected abstract ImmutableList<Category> ExtractCategories(IHtmlDocument html);
    }
}
