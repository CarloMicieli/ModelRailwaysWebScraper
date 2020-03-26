using AngleSharp.Html.Dom;
using Flurl;
using NodaTime;
using System;
using System.Collections.Immutable;
using System.Threading.Tasks;
using WebScraper.Resources;
using WebScraper.Scraping.Model;
using WebScraper.Scraping.Results;

namespace WebScraper.Scraping
{
    public abstract class AbstractWrapper : IWrapper
    {
        private readonly IWebCrawler _webCrawler;
        private readonly IClock _clock;

        protected AbstractWrapper(IClock clock, IWebCrawler webCrawler, Uri baseUri, Uri startPage)
        {
            Parser = new HtmlParser();
            BaseUri = baseUri;
            StartPage = startPage;

            _clock = clock;
            _webCrawler = webCrawler ??
                throw new ArgumentNullException(nameof(webCrawler));
        }

        public Uri BaseUri { get; }
        public Uri StartPage { get; }
        public HtmlParser Parser { get; }

        public async Task<ImmutableList<Manufacturer>> GetManufacturers()
        {
            IHtmlDocument html = await FetchDocument(StartPage.AbsoluteUri);
            return ExtractManufacturers(html);
        }

        public async Task<CategoriesResult> GetCategories(Manufacturer manufacturer)
        {
            IHtmlDocument html = await FetchDocument(manufacturer);
            var results = ExtractCategories(html);
            return new CategoriesResult(
                manufacturer,
                _clock.GetCurrentInstant(),
                results);
        }

        public async Task<ProductsResult> GetProducts(Category category)
        {
            IHtmlDocument html = await FetchDocument(category);
            var (products, pages) = ExtractProducts(html);

            return new ProductsResult(
                category,
                _clock.GetCurrentInstant(),
                Pagination.Of(pages, category.PageNumber),
                products
                );
        }

        public async Task<ProductInfo> ExtractProductInfo(Product product)
        {
            IHtmlDocument html = await FetchDocument(product);
            return ExtractProductInfo(html);
        }

        protected Task<IHtmlDocument> FetchDocument(WebResource resource) =>
            FetchDocument(resource.ResourceUri.AbsoluteUri);

        protected async Task<IHtmlDocument> FetchDocument(string url)
        {
            var content = await _webCrawler.FetchResource(url);
            return Parser.ParseDocument(content);
        }

        protected abstract ImmutableList<Manufacturer> ExtractManufacturers(IHtmlDocument html);

        protected abstract ImmutableList<Category> ExtractCategories(IHtmlDocument html);

        protected abstract (ImmutableList<Product>, ImmutableList<Page>) ExtractProducts(IHtmlDocument html);

        protected abstract ProductInfo ExtractProductInfo(IHtmlDocument html);

        protected Uri CombineUrl(string path) =>
            new Uri(Url.Combine(this.BaseUri.AbsoluteUri, path));
    }
}
