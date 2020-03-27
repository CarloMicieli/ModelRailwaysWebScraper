using System;
using Microsoft.Extensions.DependencyInjection;
using NodaTime;
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
            services.AddSingleton<IClock>(SystemClock.Instance);

            services.AddTransient<IWebCrawler, HttpWebCrawler>();

            services.AddTransient<ModellbahnshopLippe>();
            services.AddTransient<EurorailHobbies>();
            services.AddTransient<Hornby>();
            services.AddTransient<Brawa>();

            return services;
        }
    }

    public sealed class ScrapersBuilder
    {
        public bool ModellbahnshopLippe { set; get; } = false;
        public bool EurorailHobbies { set; get; } = false;
    }
}