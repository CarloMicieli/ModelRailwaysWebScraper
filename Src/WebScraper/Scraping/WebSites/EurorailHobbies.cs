using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using NodaTime;
using WebScraper.Resources;
using WebScraper.Scraping.Model;
using WebScraper.Scraping.Results;

namespace WebScraper.Scraping.WebSites
{
    public sealed class EurorailHobbies : BaseWrapper
    {
        private Dictionary<int, (string category, string powerMethod)> Categories { get; }
        private Dictionary<int, string> Manufactures { get; }
        private Dictionary<string, string> Scales { get; }

        public EurorailHobbies(IClock clock, IWebCrawler webCrawler)
            : base(clock,
                webCrawler,
                new Uri("https://www.eurorailhobbies.com/"))
        {
            Categories = InitCategories();
            Manufactures = InitManufactures();
            Scales = InitScales();
        }

        public override Task<ImmutableList<Manufacturer>> GetManufacturers()
        {
            throw new NotImplementedException();
        }

        public override Task<CategoriesResult> GetCategories(Manufacturer manufacturer)
        {
            throw new NotImplementedException();
        }

        protected override (ImmutableList<Product>, ImmutableList<Page>) ExtractProducts(IHtmlDocument html)
        {
            #region [ Extract products list ]
            throw new System.NotImplementedException();
            #endregion
        }

        protected override ProductInfo ExtractProductInfo(IHtmlDocument html)
        {
            #region [ Extract product info ]
            throw new System.NotImplementedException();
            #endregion
        }

        private static Dictionary<int, (string category, string powerMethod)> InitCategories() => new Dictionary<int, (string category, string powerMethod)>
        {
            { 1, ("Steam Locomotives", "DC") },
            { 2, ("Electric Locomotives", "DC") },
            { 3, ("Diesel Locomotives", "DC") },
            { 4, ("Steam Locomotives", "AC") },
            { 5, ("Electric Locomotives", "AC") },
            { 6, ("Diesel Locomotives", "AC") },
            { 7, ("Passenger Cars", "") },
            { 8, ("Freight Cars", "") },
            { 14, ("Starter Sets", "") },
            { 53, ("Train sets", "DC") },
            { 54, ("Train sets", "AC") },
            { 78, ("Railcars", "AC") },
            { 79, ("Railcars", "AC") },
        };

        private static Dictionary<int, string> InitManufactures() => new Dictionary<int, string>
        {
            { 1, "Marklin" },
            { 2, "Trix" },
            { 3, "Brawa" },
            { 4, "Roco" },
            { 5, "Fleischmann" },
            { 23, "Piko" },
            { 26, "Bemo" },
            { 34, "LGB" }
        };

        private static Dictionary<string, string> InitScales() => new Dictionary<string, string>
        {
            { "HO", "H0" },
            { "HOe", "H0e" },
            { "HOm", "H0m" },
            { "Z", "Z" },
            { "N", "N" },
            { "1", "1" },
            { "TT", "TT" },
        };
    }
}