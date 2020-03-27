using System;
using Microsoft.Extensions.DependencyInjection;
using WebScraper.Scraping;
using WebScraper.Scraping.WebSites;

namespace WebScraper
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddWebScrapers(this IServiceCollection services, Action<ScrapersBuilder> scraperAction)
        {
            var sb = new ScrapersBuilder();
            scraperAction(sb);

            services.AddHttpClient();
            services.AddTransient<IWebCrawler, HttpWebCrawler>();

            if (sb.ModellbahnshopLippe)
            {
                services.AddTransient<ModellbahnshopLippe>();
            }

            return services;
        }
    }

    public sealed class ScrapersBuilder
    {
        public bool ModellbahnshopLippe { set; get; } = false;
    }
}