namespace WebScraper.Resources
{
    public sealed class Product : WebResource
    {
        public string ItemNumber { set; get; }
        public string Description { set; get; }
        public string Brand { set; get; }
        public string Info { set; get; }
    }
}
