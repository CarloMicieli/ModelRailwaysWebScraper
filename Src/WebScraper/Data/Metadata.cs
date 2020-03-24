using System.Collections.Generic;

namespace WebScraper.Data
{
    public sealed class Metadata
    {
        public List<Meta> Meta { set; get; }
    }

    public sealed class Meta
    {
        public string Name { set; get; }
        public string Content { set; get; }
    }
}
