using AngleSharp.Html.Dom;
using System;
using WebScraper.Model;
using WebScraper.Resources.Collections;

namespace WebScraper.Scraping.WebSites
{
    public sealed class ModellbahnshopLippe : AbstractWrapper
    {
        public ModellbahnshopLippe(IWebCrawler webCrawler) 
            : base(webCrawler, 
                  new Uri(@"https://www.modellbahnshop-lippe.com"),
                  new Uri(@"https://www.modellbahnshop-lippe.com/Manufacturer/Products/gb/hersteller.html"))
        {
        }

        protected override IManufacturersCollection ExtractManufacturers(IHtmlDocument html)
        {
            #region [ Extract Manufacturers ]
            throw new System.NotImplementedException();
            #endregion
        }

        protected override ICategoriesCollection ExtractCategories(IHtmlDocument html)
        {
            #region [ Extract Categories ]
            throw new System.NotImplementedException();
            #endregion
        }

        protected override IProductsCollection ExtractProducts(IHtmlDocument html)
        {
            #region [ Extract Products ]
            throw new System.NotImplementedException();
            #endregion
        }

        protected override ProductInfo ExtractProductInfo(IHtmlDocument html)
        {
            #region [ Extract Product Info ]
            throw new System.NotImplementedException();
            #endregion
        }
    }
}
