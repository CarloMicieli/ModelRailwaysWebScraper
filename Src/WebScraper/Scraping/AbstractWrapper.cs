using AngleSharp.Html.Dom;
using Flurl;
using System;
using System.Threading.Tasks;
using WebScraper.Model;
using WebScraper.Resources;
using WebScraper.Resources.Collections;

namespace WebScraper.Scraping
{
    public abstract class AbstractWrapper : IWrapper
    {
        private readonly IWebCrawler _webCrawler;

        protected AbstractWrapper(IWebCrawler webCrawler, Uri baseUri, Uri startPage)
        {
            Parser = new HtmlParser();
            BaseUri = baseUri;
            StartPage = startPage;
            
            _webCrawler = webCrawler ??
                throw new ArgumentNullException(nameof(webCrawler));
        }

        public Uri BaseUri { get; }
        public Uri StartPage { get; }
        public HtmlParser Parser { get; }

        public async Task<IManufacturersCollection> GetManufacturers()
        {
            IHtmlDocument html = await FetchDocument(StartPage.AbsoluteUri);
            return ExtractManufacturers(html);
        }

        public async Task<ICategoriesCollection> GetCategories(Manufacturer manufacturer)
        {
            IHtmlDocument html = await FetchDocument(manufacturer);
            return ExtractCategories(html);
        }

        public async Task<IProductsCollection> GetProducts(Category category)
        {
            IHtmlDocument html = await FetchDocument(category);
            return ExtractProducts(html);
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

        protected abstract IManufacturersCollection ExtractManufacturers(IHtmlDocument html);

        protected abstract ICategoriesCollection ExtractCategories(IHtmlDocument html);

        protected abstract IProductsCollection ExtractProducts(IHtmlDocument html);

        protected abstract ProductInfo ExtractProductInfo(IHtmlDocument html);

        protected Uri CombineUrl(string path) =>
            new Uri(Url.Combine(this.BaseUri.AbsoluteUri, path));
    }
}
