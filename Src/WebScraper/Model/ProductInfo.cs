using System.Collections.Immutable;

namespace WebScraper.Model
{
    public sealed class ProductInfo
    {
        public string Brand { get; set; }

        public string Description { get; set; }

        public string LongDescription { get; set; }

        public string Title { set; get; }

        public IImmutableList<Metadata> Metadata { set; get; }

        public IImmutableList<Image> Images { set; get; }

        public IImmutableList<Feature> Features { set; get; }

        public IImmutableSet<string> Categories { set; get; }

        public IImmutableDictionary<string, string> Specifications { set; get; }
    }
}
