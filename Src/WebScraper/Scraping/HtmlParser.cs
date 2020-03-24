using AngleSharp;
using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;

namespace WebScraper.Scraping
{
    public sealed class HtmlParser
    {
        private readonly IHtmlParser _parser;

        public HtmlParser()
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);

            _parser = context.GetService<IHtmlParser>();
        }

        public IHtmlDocument ParseDocument(string source) => _parser.ParseDocument(source);
    }
}
