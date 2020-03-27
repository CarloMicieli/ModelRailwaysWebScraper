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
    public abstract class BaseWrapper : IWrapper
    {
        private readonly IWebCrawler _webCrawler;
        private readonly IClock _clock;

        protected BaseWrapper(IClock clock, IWebCrawler webCrawler, Uri baseUri)
        {
            Parser = new HtmlParser();
            BaseUri = baseUri;

            _clock = clock;
            _webCrawler = webCrawler ??
                throw new ArgumentNullException(nameof(webCrawler));
        }

        public Uri BaseUri { get; }
        public HtmlParser Parser { get; }

        public abstract Task<ImmutableList<Manufacturer>> GetManufacturers();

        public abstract Task<CategoriesResult> GetCategories(Manufacturer manufacturer);

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

        protected abstract (ImmutableList<Product>, ImmutableList<Page>) ExtractProducts(IHtmlDocument html);

        protected abstract ProductInfo ExtractProductInfo(IHtmlDocument html);

        protected Uri CombineUrl(string path) =>
            new Uri(Url.Combine(this.BaseUri.AbsoluteUri, path));

        protected Instant GetCurrentInstant() => _clock.GetCurrentInstant();
    }
}
