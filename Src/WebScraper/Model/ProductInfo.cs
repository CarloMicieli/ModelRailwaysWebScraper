using System.Collections.Immutable;

namespace WebScraper.Model
{
    public sealed class ProductInfo
    {
        public string Title { set; get; }

        public IImmutableList<Metadata> Metadata { set; get; }

        public IImmutableList<Image> Images { set; get; }
    }
}
