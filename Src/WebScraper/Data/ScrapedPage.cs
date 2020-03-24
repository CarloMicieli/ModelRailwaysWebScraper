using System;
using System.Collections.Generic;

namespace WebScraper.Data
{
    public sealed class ScrapedPage
    {
        public string Brand { set; get; }
        public string Scale { set; get; }
        public Category Category { set; get; }
        public string Url { set; get; }
        public DateTime DownloadTime { set; get; }
        public string WebsiteName { set; get; }
        public List<IdValue> Attributes { set; get; }
        public Metadata Metadata { set; get; }
        public ImageLink Image { set; get; }
        public string LongDescription { set; get; }

        public override string ToString() => $"Page(Website = {WebsiteName}, Url = {Url}, Download time = {DownloadTime})";
    }

    public sealed class IdValue
    {
        public string Id { set; get; }
        public string Value { set; get; }
    }
}
