using Core;

namespace WebScraper.Messages.Commands
{
    public class ScrapeWebSiteCommand : ICommand
    {
        public string WebsiteUrl { set; get; }
    }
}
